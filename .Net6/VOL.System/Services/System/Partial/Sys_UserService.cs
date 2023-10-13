using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOL.Core.Configuration;
using VOL.Core.Enums;
using VOL.Core.Extensions;
using VOL.Core.ManageUser;
using VOL.Core.Services;
using VOL.Core.Utilities;
using VOL.Entity.DomainModels;
using VOL.System.IRepositories;

namespace VOL.System.Services
{
    public partial class Sys_UserService
    {
        private Microsoft.AspNetCore.Http.HttpContext _context;
        private ISys_UserRepository _repository;
        [ActivatorUtilitiesConstructor]
        public Sys_UserService(IHttpContextAccessor httpContextAccessor, ISys_UserRepository repository)
            : base(repository)
        {
            _context = httpContextAccessor.HttpContext;
            _repository = repository;
        }
        WebResponseContent webResponse = new WebResponseContent();
        /// <summary>
        /// WebApi登入
        /// </summary>
        /// <param name="loginInfo"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> Login(LoginInfo loginInfo, bool verificationCode = true)
        {
            string msg = string.Empty;
            //   2020.06.12增加驗證碼
            IMemoryCache memoryCache = _context.GetService<IMemoryCache>();
            string cacheCode = (memoryCache.Get(loginInfo.UUID) ?? "").ToString();
            if (string.IsNullOrEmpty(cacheCode))
            {
                return webResponse.Error("驗證碼已失效");
            }
            if (cacheCode.ToLower() != loginInfo.VerificationCode.ToLower())
            {
                memoryCache.Remove(loginInfo.UUID);
                return webResponse.Error("驗證碼不正確");
            }
            try
            {
                Sys_User user = await repository.FindAsIQueryable(x => x.UserName == loginInfo.UserName)
                    .FirstOrDefaultAsync();

                if (user == null || loginInfo.Password.Trim().EncryptDES(AppSetting.Secret.User) != (user.UserPwd ?? ""))
                    return webResponse.Error(ResponseType.LoginError);

                string token = JwtHelper.IssueJwt(new UserInfo()
                {
                    User_Id = user.User_Id,
                    UserName = user.UserName,
                    Role_Id = user.Role_Id
                });
                user.Token = token;
                webResponse.Data = new { token, userName = user.UserTrueName, img = user.HeadImageUrl };
                repository.Update(user, x => x.Token, true);
                UserContext.Current.LogOut(user.User_Id);

                loginInfo.Password = string.Empty;

                return webResponse.OK(ResponseType.LoginSuccess);
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
                if (_context.GetService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>().IsDevelopment())
                {
                    throw new Exception(ex.Message + ex.StackTrace);
                }
                return webResponse.Error(ResponseType.ServerError);
            }
            finally
            {
                memoryCache.Remove(loginInfo.UUID);
                Logger.Info(LoggerType.Login, loginInfo.Serialize(), webResponse.Message, msg);
            }
        }

        /// <summary>
        ///當token將要過期時，提前置換一個新的token
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> ReplaceToken()
        {
            string error = "";
            UserInfo userInfo = null;
            try
            {
                string requestToken = _context.Request.Headers[AppSetting.TokenHeaderName];
                requestToken = requestToken?.Replace("Bearer ", "");
                if (UserContext.Current.Token != requestToken) return webResponse.Error("Token已失效!");

                if (JwtHelper.IsExp(requestToken)) return webResponse.Error("Token已過期!");

                int userId = UserContext.Current.UserId;
                userInfo = await
                     repository.FindFirstAsync(x => x.User_Id == userId,
                     s => new UserInfo()
                     {
                         User_Id = userId,
                         UserName = s.UserName,
                         UserTrueName = s.UserTrueName,
                         Role_Id = s.Role_Id,
                         RoleName = s.RoleName
                     });

                if (userInfo == null) return webResponse.Error("未查到用戶信息!");

                string token = JwtHelper.IssueJwt(userInfo);
                //移除當前緩存
                base.CacheContext.Remove(userId.GetUserIdKey());
                //只更新的token字段
                repository.Update(new Sys_User() { User_Id = userId, Token = token }, x => x.Token, true);
                webResponse.OK(null, token);
            }
            catch (Exception ex)
            {
                error = ex.Message + ex.StackTrace + ex.Source;
                webResponse.Error("token替換出錯了..");
            }
            finally
            {
                Logger.Info(LoggerType.ReplaceToeken, ($"用戶Id:{userInfo?.User_Id},用戶{userInfo?.UserTrueName}")
                    + (webResponse.Status ? "token替換成功" : "token替換失敗"), null, error);
            }
            return webResponse;
        }

        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> ModifyPwd(string oldPwd, string newPwd)
        {
            oldPwd = oldPwd?.Trim();
            newPwd = newPwd?.Trim();
            string message = "";
            try
            {
                if (string.IsNullOrEmpty(oldPwd)) return webResponse.Error("舊密碼不能為空");
                if (string.IsNullOrEmpty(newPwd)) return webResponse.Error("新密碼不能為空");
                if (newPwd.Length < 6) return webResponse.Error("密碼不能少於6位");

                int userId = UserContext.Current.UserId;
                string userCurrentPwd = await base.repository.FindFirstAsync(x => x.User_Id == userId, s => s.UserPwd);

                string _oldPwd = oldPwd.EncryptDES(AppSetting.Secret.User);
                if (_oldPwd != userCurrentPwd) return webResponse.Error("舊密碼不正確");

                string _newPwd = newPwd.EncryptDES(AppSetting.Secret.User);
                if (userCurrentPwd == _newPwd) return webResponse.Error("新密碼不能與舊密碼相同");


                repository.Update(new Sys_User
                {
                    User_Id = userId,
                    UserPwd = _newPwd,
                    LastModifyPwdDate = DateTime.Now
                }, x => new { x.UserPwd, x.LastModifyPwdDate }, true);

                webResponse.OK("密碼修改成功");
            }
            catch (Exception ex)
            {
                message = ex.Message;
                webResponse.Error("服務器了點問題,請稍後再試");
            }
            finally
            {
                if (message == "")
                {
                    Logger.OK(LoggerType.ApiModifyPwd, "密碼修改成功");
                }
                else
                {
                    Logger.Error(LoggerType.ApiModifyPwd, message);
                }
            }
            return webResponse;
        }
        /// <summary>
        /// 個人中心獲取當前用戶信息
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCurrentUserInfo()
        {
            var data = await base.repository
                .FindAsIQueryable(x => x.User_Id == UserContext.Current.UserId)
                .Select(s => new
                {
                    s.UserName,
                    s.UserTrueName,
                    s.Address,
                    s.PhoneNo,
                    s.Email,
                    s.Remark,
                    s.Gender,
                    s.RoleName,
                    s.HeadImageUrl,
                    s.CreateDate
                })
                .FirstOrDefaultAsync();
            return webResponse.OK(null, data);
        }

        /// <summary>
        /// 設置固定排序方式及顯示用戶過濾
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        public override PageGridData<Sys_User> GetPageData(PageDataOptions pageData)
        {
            int roleId = -1;
            //樹形菜單傳查詢角色下所有用戶
            if (pageData.Value != null)
            {
                roleId = pageData.Value.ToString().GetInt();
            }

            IQueryable<Sys_UserDepartment> deptQuery = null;
            QueryRelativeList = (List<SearchParameters> parameters) =>
            {
                foreach (var item in parameters)
                {
                    if (!string.IsNullOrEmpty(item.Value) && item.Name == "DeptIds")
                    {

                        var deptIds = item.Value.Split(",").Select(s => s.GetGuid()).Where(x => x != null);
                        item.Value = null;
                        deptQuery = repository.DbContext.Set<Sys_UserDepartment>().Where(x => x.Enable == 1 && deptIds.Contains(x.DepartmentId));
                    }
                }
            };

            QueryRelativeExpression = (IQueryable<Sys_User> queryable) =>
             {

                 if (deptQuery!=null)
                 {
                     queryable = queryable.Where(c => deptQuery.Any(x => x.UserId == c.User_Id));
                 }

                 if (roleId <= 0)
                 {
                     if (UserContext.Current.IsSuperAdmin) return queryable;
                     roleId = UserContext.Current.RoleId;
                 }

                //查看用戶時，只能看下自己角色下的所有用戶
                List<int> roleIds = Sys_RoleService
                    .Instance
                    .GetAllChildrenRoleId(roleId);
                 roleIds.Add(roleId);
                //判斷查詢的角色是否越權
                if (roleId != UserContext.Current.RoleId && !roleIds.Contains(roleId))
                 {
                     roleId = -999;
                 }
                 return queryable.Where(x => roleIds.Contains(x.Role_Id));
             };
            return base.GetPageData(pageData);
        }

        /// <summary>
        /// 新建用戶，根據實際情况自行處理
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        public override WebResponseContent Add(SaveModel saveModel)
        {
            saveModel.MainData["RoleName"] = "無";
            base.AddOnExecute = (SaveModel userModel) =>
            {
                int roleId = userModel?.MainData?["Role_Id"].GetInt() ?? 0;
                if (roleId > 0 && !UserContext.Current.IsSuperAdmin)
                {
                    string roleName = GetChildrenName(roleId);
                    if ((roleId == 1) || string.IsNullOrEmpty(roleName))
                        return webResponse.Error("不能選擇此角色");
                }
                return webResponse.OK();
            };


            ///生成6位數随機密碼
            string pwd = 6.GenerateRandomNumber();
            //在AddOnExecuting之前已經對提交的數據做過驗證是否為空
            base.AddOnExecuting = (Sys_User user, object obj) =>
            {
                user.UserName = user.UserName.Trim();
                if (repository.Exists(x => x.UserName == user.UserName))
                    return webResponse.Error("用戶名已經被註冊");
                user.UserPwd = pwd.EncryptDES(AppSetting.Secret.User);
                //設置默認頭像
                return webResponse.OK();
            };

            base.AddOnExecuted = (Sys_User user, object list) =>
            {
                var deptIds = user.DeptIds?.Split(",").Select(s => s.GetGuid()).Where(x => x != null).Select(s => (Guid)s).ToArray();
                SaveDepartment(deptIds, user.User_Id);
                return webResponse.OK($"用戶新建成功.帳號{user.UserName}密碼{pwd}");
            };
            return base.Add(saveModel); ;
        }

        /// <summary>
        /// 刪除用戶攔截過濾
        /// 用戶被刪除後同時清空對應緩存
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="delList"></param>
        /// <returns></returns>
        public override WebResponseContent Del(object[] keys, bool delList = false)
        {
            base.DelOnExecuting = (object[] ids) =>
            {
                if (!UserContext.Current.IsSuperAdmin)
                {
                    int[] userIds = ids.Select(x => Convert.ToInt32(x)).ToArray();
                    //校驗只能刪除當前角色下能看到的用戶
                    var xxx = repository.Find(x => userIds.Contains(x.User_Id));
                    var delUserIds = repository.Find(x => userIds.Contains(x.User_Id), s => new { s.User_Id, s.Role_Id, s.UserTrueName });
                    List<int> roleIds = Sys_RoleService
                       .Instance
                       .GetAllChildrenRoleId(UserContext.Current.RoleId);

                    string[] userNames = delUserIds.Where(x => !roleIds.Contains(x.Role_Id))
                     .Select(s => s.UserTrueName)
                     .ToArray();
                    if (userNames.Count() > 0)
                    {
                        return webResponse.Error($"沒有權限刪除用戶：{string.Join(',', userNames)}");
                    }
                }

                return webResponse.OK();
            };
            base.DelOnExecuted = (object[] userIds) =>
            {
                var objKeys = userIds.Select(x => x.GetInt().GetUserIdKey());
                base.CacheContext.RemoveAll(objKeys);
                return new WebResponseContent() { Status = true };
            };
            return base.Del(keys, delList);
        }

        private string GetChildrenName(int roleId)
        {
            //只能修改當前角色能看到的用戶
            string roleName = Sys_RoleService
                .Instance
                .GetAllChildren(UserContext.Current.UserInfo.Role_Id).Where(x => x.Id == roleId)
                .Select(s => s.RoleName).FirstOrDefault();
            return roleName;
        }

        /// <summary>
        /// 修改用戶攔截過濾
        /// 
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
        public override WebResponseContent Update(SaveModel saveModel)
        {
            UserInfo userInfo = UserContext.Current.UserInfo;
            saveModel.MainData["RoleName"] = "無";
            //禁止修改用戶名
            base.UpdateOnExecute = (SaveModel saveInfo) =>
            {
                if (!UserContext.Current.IsSuperAdmin)
                {
                    int roleId = saveModel.MainData["Role_Id"].GetInt();
                    string roleName = GetChildrenName(roleId);
                    saveInfo.MainData.TryAdd("RoleName", roleName);
                    if (UserContext.IsRoleIdSuperAdmin(userInfo.Role_Id))
                    {
                        return webResponse.OK();
                    }
                    if (string.IsNullOrEmpty(roleName)) return webResponse.Error("不能選擇此角色");
                }

                return webResponse.OK();
            };
            base.UpdateOnExecuting = (Sys_User user, object obj1, object obj2, List<object> list) =>
            {
                if (user.User_Id == userInfo.User_Id && user.Role_Id != userInfo.Role_Id)
                    return webResponse.Error("不能修改自己的角色");

                var _user = repository.Find(x => x.User_Id == user.User_Id,
                    s => new { s.UserName, s.UserPwd })
                    .FirstOrDefault();
                user.UserName = _user.UserName;
                //Sys_User實體的UserPwd用戶密碼字段的屬性不是編輯，此處不會修改密碼。但防止代碼生成器將密碼字段的修改成了可編輯造成密碼被修改
                user.UserPwd = _user.UserPwd;
                return webResponse.OK();
            };
            //用戶信息被修改後，將用戶的緩存信息清除
            base.UpdateOnExecuted = (Sys_User user, object obj1, object obj2, List<object> List) =>
            {
                base.CacheContext.Remove(user.User_Id.GetUserIdKey());
                var deptIds = user.DeptIds?.Split(",").Select(s => s.GetGuid()).Where(x => x != null).Select(s => (Guid)s).ToArray();
                SaveDepartment(deptIds, user.User_Id);
                return new WebResponseContent(true);
            };
            return base.Update(saveModel);
        }


        /// <summary>
        /// 保存部門
        /// </summary>
        /// <param name="deptIds"></param>
        /// <param name="userId"></param>
        public void SaveDepartment(Guid[] deptIds, int userId)
        {

            if (userId <= 0)
            {
                return;
            }
            if (deptIds == null)
            {
                deptIds = new Guid[] { };
            }

            //如果需要判斷當前角色是否越權，再調用一下獲取當前部門下的所有子角色判斷即可

            var roles = repository.DbContext.Set<Sys_UserDepartment>().Where(x => x.UserId == userId)
              .Select(s => new { s.DepartmentId, s.Enable, s.Id })
              .ToList();
            //沒有設置部門
            if (deptIds.Length == 0 && !roles.Exists(x => x.Enable == 1))
            {
                return;
            }

            UserInfo user = UserContext.Current.UserInfo;
            //新設置的部門
            var add = deptIds.Where(x => !roles.Exists(r => r.DepartmentId == x)).Select(s => new Sys_UserDepartment()
            {
                DepartmentId = s,
                UserId = userId,
                Enable = 1,
                CreateDate = DateTime.Now,
                Creator = user.UserTrueName,
                CreateID = user.User_Id
            }).ToList();

            //刪除的部門
            var update = roles.Where(x => !deptIds.Contains(x.DepartmentId) && x.Enable == 1).Select(s => new Sys_UserDepartment()
            {
                Id = s.Id,
                Enable = 0,
                ModifyDate = DateTime.Now,
                Modifier = user.UserTrueName,
                ModifyID = user.User_Id
            }).ToList();

            //之前設置過的部門重新分配 
            update.AddRange(roles.Where(x => deptIds.Contains(x.DepartmentId) && x.Enable != 1).Select(s => new Sys_UserDepartment()
            {
                Id = s.Id,
                Enable = 1,
                ModifyDate = DateTime.Now,
                Modifier = user.UserTrueName,
                ModifyID = user.User_Id
            }).ToList());
            repository.AddRange(add);

            repository.UpdateRange(update, x => new { x.Enable, x.ModifyDate, x.Modifier, x.ModifyID });
            repository.SaveChanges();
        }

        /// <summary>
        /// 導出處理
        /// </summary>
        /// <param name="pageData"></param>
        /// <returns></returns>
        public override WebResponseContent Export(PageDataOptions pageData)
        {
            //限定只能導出當前角色能看到的所有用戶
            QueryRelativeExpression = (IQueryable<Sys_User> queryable) =>
            {
                if (UserContext.Current.IsSuperAdmin) return queryable;
                List<int> roleIds = Sys_RoleService
                 .Instance
                 .GetAllChildrenRoleId(UserContext.Current.RoleId);
                return queryable.Where(x => roleIds.Contains(x.Role_Id) || x.User_Id == UserContext.Current.UserId);
            };

            base.ExportOnExecuting = (List<Sys_User> list, List<string> ignoreColumn) =>
            {
                if (!ignoreColumn.Contains("Role_Id"))
                {
                    ignoreColumn.Add("Role_Id");
                }
                if (!ignoreColumn.Contains("RoleName"))
                {
                    ignoreColumn.Remove("RoleName");
                }
                WebResponseContent responseData = new WebResponseContent(true);
                return responseData;
            };
            return base.Export(pageData);
        }
    }
}

