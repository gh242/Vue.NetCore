using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VOL.Core.Extensions;
using VOL.Core.ManageUser;
using VOL.Core.Services;
using VOL.Core.UserManager;
using VOL.Core.Utilities;
using VOL.Entity;
using VOL.Entity.DomainModels;

namespace VOL.System.Services
{
    public partial class Sys_RoleService
    {
        private WebResponseContent _responseContent = new WebResponseContent();
        public override PageGridData<Sys_Role> GetPageData(PageDataOptions pageData)
        {
            //角色Id=1默認為超級管理員角色，介面上不顯示此角色
            QueryRelativeExpression = (IQueryable<Sys_Role> queryable) =>
            {
                if (UserContext.Current.IsSuperAdmin)
                {
                    return queryable;
                }
                List<int> roleIds = GetAllChildrenRoleIdAndSelf();
                return queryable.Where(x => roleIds.Contains(x.Role_Id));
            };
            return base.GetPageData(pageData);
        }
        /// <summary>
        /// 編輯權限時，獲取當前用戶的所有菜單權限
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCurrentUserTreePermission()
        {
            return await GetUserTreePermission(UserContext.Current.RoleId);
        }

        /// <summary>
        /// 編輯權限時，獲取指定角色的所有菜單權限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> GetUserTreePermission(int roleId)
        {
            if (!UserContext.IsRoleIdSuperAdmin(roleId) && UserContext.Current.RoleId != roleId)
            {
                if (!(await GetAllChildrenAsync(UserContext.Current.RoleId)).Exists(x => x.Id == roleId))
                {
                    return _responseContent.Error("沒有權限獲取此角色的權限信息");
                }
            }
            //獲取用戶權限
            List<Permissions> permissions = UserContext.Current.GetPermissions(roleId);
            //權限用戶權限查詢所有的菜單信息
            List<Sys_Menu> menus = await Task.Run(() => Sys_MenuService.Instance.GetUserMenuList(roleId));
            //獲取當前用戶權限如:(Add,Search)對應的顯示文本信息如:Add：添加，Search:查詢
            var data = menus.Select(x => new
            {
                Id = x.Menu_Id,
                Pid = x.ParentId,
                Text = x.MenuName,
                IsApp = x.MenuType == 1,
                Actions = GetActions(x.Menu_Id, x.Actions, permissions, roleId)
            });
            return _responseContent.OK(null, data);
        }

        private List<Sys_Actions> GetActions(int menuId, List<Sys_Actions> menuActions, List<Permissions> permissions, int roleId)
        {
            if (UserContext.IsRoleIdSuperAdmin(roleId))
            {
                return menuActions;
            }

            return menuActions.Where(p => permissions
                 .Exists(w => menuId == w.Menu_Id
                 && w.UserAuthArr.Contains(p.Value)))
                  .ToList();
        }

        private List<RoleNodes> rolesChildren = new List<RoleNodes>();

        /// <summary>
        /// 編輯權限時獲取當前用戶下的所有角色與當前用戶的菜單權限
        /// </summary>
        /// <returns></returns>
        public async Task<WebResponseContent> GetCurrentTreePermission()
        {
            _responseContent = await GetCurrentUserTreePermission();
            int roleId = UserContext.Current.RoleId;
            return _responseContent.OK(null, new
            {
                tree = _responseContent.Data,
                roles = await GetAllChildrenAsync(roleId)
            });
        }

        private List<RoleNodes> roles = null;

        /// <summary>
        /// 獲取當前角色下的所有角色包括自己的角色Id
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllChildrenRoleIdAndSelf()
        {
            int roleId = UserContext.Current.RoleId;
            List<int> roleIds = GetAllChildren(roleId).Select(x => x.Id).ToList();
            roleIds.Add(roleId);
            return roleIds;
        }


        /// <summary>
        /// 獲取當前角色下的所有角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<RoleNodes> GetAllChildren(int roleId)
        {
            roles = GetAllRoleQueryable(roleId).ToList();
            return GetAllChildrenNodes(roleId);
        }

        public async Task<List<RoleNodes>> GetAllChildrenAsync(int roleId)
        {
            roles = await GetAllRoleQueryable(roleId).ToListAsync();
            return GetAllChildrenNodes(roleId);
        }
        private IQueryable<RoleNodes> GetAllRoleQueryable(int roleId)
        {
            return repository
                   .FindAsIQueryable(
                   x => x.Enable == 1 && x.Role_Id > 1)
                   .Select(
                   s => new RoleNodes()
                   {
                       Id = s.Role_Id,
                       ParentId = s.ParentId,
                       RoleName = s.RoleName
                   });
        }

        public async Task<List<int>> GetAllChildrenRoleIdAsync(int roleId)
        {
            return (await GetAllChildrenAsync(roleId)).Select(x => x.Id).ToList();
        }


        public List<int> GetAllChildrenRoleId(int roleId)
        {
            return GetAllChildren(roleId).Select(x => x.Id).ToList();
        }

        private List<RoleNodes> GetAllChildrenNodes(int roleId)
        {
            return RoleContext.GetAllChildren(roleId);
        }
        /// <summary>
        /// 遞歸獲取所有子節點權限
        /// </summary>
        /// <param name="roleId"></param>


        /// <summary>
        /// 保存角色權限
        /// </summary>
        /// <param name="userPermissions"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> SavePermission(List<UserPermissions> userPermissions, int roleId)
        {

            string message = "";
            try
            {
                UserInfo user = UserContext.Current.UserInfo;
                if (!(await GetAllChildrenAsync(user.Role_Id)).Exists(x => x.Id == roleId))
                    return _responseContent.Error("沒有權限修改此角色的權限信息");
                //當前用戶的權限
                List<Permissions> permissions = UserContext.Current.Permissions;

                List<int> originalMeunIds = new List<int>();
                //被分配角色的權限
                List<Sys_RoleAuth> roleAuths = await repository.FindAsync<Sys_RoleAuth>(x => x.Role_Id == roleId);
                List<Sys_RoleAuth> updateAuths = new List<Sys_RoleAuth>();
                foreach (UserPermissions x in userPermissions)
                {
                    Permissions per = permissions.Where(p => p.Menu_Id == x.Id).FirstOrDefault();
                    //不能分配超過當前用戶的權限
                    if (per == null) continue;
                    //per.UserAuthArr.Contains(a.Value)校驗權限範圍
                    string[] arr = x.Actions == null || x.Actions.Count == 0
                      ? new string[0]
                      : x.Actions.Where(a => per.UserAuthArr.Contains(a.Value))
                      .Select(s => s.Value).ToArray();

                    //如果當前權限沒有分配過，設置Auth_Id默認為0，表示新增的權限
                    var auth = roleAuths.Where(r => r.Menu_Id == x.Id).Select(s => new { s.Auth_Id, s.AuthValue, s.Menu_Id }).FirstOrDefault();
                    string newAuthValue = string.Join(",", arr);
                    //權限沒有發生變化則不處理
                    if (auth == null || auth.AuthValue != newAuthValue)
                    {
                        updateAuths.Add(new Sys_RoleAuth()
                        {
                            Role_Id = roleId,
                            Menu_Id = x.Id,
                            AuthValue = string.Join(",", arr),
                            Auth_Id = auth == null ? 0 : auth.Auth_Id,
                            ModifyDate = DateTime.Now,
                            Modifier = user.UserTrueName,
                            CreateDate = DateTime.Now,
                            Creator = user.UserTrueName
                        });
                    }
                    else
                    {
                        originalMeunIds.Add(auth.Menu_Id);
                    }

                }
                //更新權限
                repository.UpdateRange(updateAuths.Where(x => x.Auth_Id > 0), x => new
                {
                    x.Menu_Id,
                    x.AuthValue,
                    x.Modifier,
                    x.ModifyDate
                });
                //新增的權限
                repository.AddRange(updateAuths.Where(x => x.Auth_Id <= 0));

                //獲取權限取消的權限
                int[] authIds = roleAuths.Where(x => userPermissions.Select(u => u.Id)
                 .ToList().Contains(x.Menu_Id) || originalMeunIds.Contains(x.Menu_Id))
                .Select(s => s.Auth_Id)
                .ToArray();
                List<Sys_RoleAuth> delAuths = roleAuths.Where(x => x.AuthValue != "" && !authIds.Contains(x.Auth_Id)).ToList();
                delAuths.ForEach(x =>
                {
                    x.AuthValue = "";
                });
                //將取消的權限設置為""
                repository.UpdateRange(delAuths, x => new
                {
                    x.Menu_Id,
                    x.AuthValue,
                    x.Modifier,
                    x.ModifyDate
                });

                int addCount = updateAuths.Where(x => x.Auth_Id <= 0).Count();
                int updateCount = updateAuths.Where(x => x.Auth_Id > 0).Count();
                await repository.SaveChangesAsync();

                string _version = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                //標識緩存已更新
                base.CacheContext.Add(roleId.GetRoleIdKey(), _version);

                _responseContent.OK($"保存成功：新增加配菜單權限{addCount}條,更新菜單{updateCount}條,刪除權限{delAuths.Count()}條");
            }
            catch (Exception ex)
            {
                message = "異常信息：" + ex.Message + ex.StackTrace + ",";
            }
            finally
            {
                Logger.Info($"權限分配置:{message}{_responseContent.Message}");
            }

            return _responseContent;
        }


        public override WebResponseContent Add(SaveModel saveDataModel)
        {
            AddOnExecuting = (Sys_Role role, object obj) =>
            {
                if (!UserContext.Current.IsSuperAdmin && role.ParentId > 0 && !RoleContext.GetAllChildrenIds(UserContext.Current.RoleId).Contains(role.ParentId))
                {
                    return _responseContent.Error("不能添加此角色");
                }
                return ValidateRoleName(role, x => x.RoleName == role.RoleName);
            };
            return RemoveCache(base.Add(saveDataModel));
        }

        public override WebResponseContent Del(object[] keys, bool delList = true)
        {
            if (!UserContext.Current.IsSuperAdmin)
            {
                var roleIds = RoleContext.GetAllChildrenIds(UserContext.Current.RoleId);
                var _keys = keys.Select(s => s.GetInt());
                if (_keys.Any(x => !roleIds.Contains(x)))
                {
                    return _responseContent.Error("沒有權限刪除此角色");
                }
            }
        
            return RemoveCache(base.Del(keys, delList));
        }

        private WebResponseContent ValidateRoleName(Sys_Role role, Expression<Func<Sys_Role, bool>> predicate)
        {
            if (repository.Exists(predicate))
            {
                return _responseContent.Error($"角色名【{role.RoleName}】已存在,請設置其他角色名");
            }
            return _responseContent.OK();
        }

        public override WebResponseContent Update(SaveModel saveModel)
        {
            UpdateOnExecuting = (Sys_Role role, object obj1, object obj2, List<object> obj3) =>
            {
                //2020.05.07新增禁止選擇上級角色為自己
                if (role.Role_Id == role.ParentId)
                {
                    return _responseContent.Error($"上級角色不能選擇自己");
                }
                if (role.Role_Id == UserContext.Current.RoleId)
                {
                    return _responseContent.Error($"不能修改自己的角色");
                }
                if (repository.Exists(x => x.Role_Id == role.ParentId && x.ParentId == role.Role_Id))
                {
                    return _responseContent.Error($"不能選擇此上級角色，選擇的上級角色與當前角色形成依賴關系");
                }
                if (!UserContext.Current.IsSuperAdmin)
                {
                    var roleIds = RoleContext.GetAllChildrenIds(UserContext.Current.RoleId);
                    if (role.ParentId > 0)
                    {
                        if (!roleIds.Contains(role.ParentId))
                        {
                            return _responseContent.Error($"不能選擇此角色");
                        }
                    }
                    if (!roleIds.Contains(role.Role_Id))
                    {
                        return _responseContent.Error($"不能選擇此角色");
                    }
                    return _responseContent.OK("");
                }
                return ValidateRoleName(role, x => x.RoleName == role.RoleName && x.Role_Id != role.Role_Id);
            };
            return RemoveCache(base.Update(saveModel));
        }
        private WebResponseContent RemoveCache(WebResponseContent webResponse)
        {
            if (webResponse.Status)
            {
                RoleContext.Refresh();
            }
            return webResponse;
        }
    }


    public class UserPermissions
    {
        public int Id { get; set; }
        public int Pid { get; set; }
        public string Text { get; set; }
        public bool IsApp { get; set; }
        public List<Sys_Actions> Actions { get; set; }
    }
}
