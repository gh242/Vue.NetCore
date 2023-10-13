using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VOL.Core.CacheManager;
using VOL.Core.DBManager;
using VOL.Core.Enums;
using VOL.Core.Extensions;
using VOL.Core.Extensions.AutofacManager;
using VOL.Core.UserManager;
using VOL.Entity;
using VOL.Entity.DomainModels;

namespace VOL.Core.ManageUser
{
    public class UserContext
    {
        /// <summary>
        /// 為了盡量减少redis或Memory讀取,保證執行效率,將UserContext注入到DI，
        /// 每個UserContext的屬性至多讀取一次redis或Memory緩存從而提高查詢效率
        /// </summary>
        public static UserContext Current
        {
            get
            {
                return Context.RequestServices.GetService(typeof(UserContext)) as UserContext;
            }
        }

        private static Microsoft.AspNetCore.Http.HttpContext Context
        {
            get
            {
                return Utilities.HttpContext.Current;
            }
        }
        private static ICacheService CacheService
        {
            get { return GetService<ICacheService>(); }
        }

        private static T GetService<T>() where T : class
        {
            return AutofacContainerModule.GetService<T>();
        }

        public UserInfo UserInfo
        {
            get
            {
                if (_userInfo != null)
                {
                    return _userInfo;
                }
                return GetUserInfo(UserId);
            }
        }

        private UserInfo _userInfo { get; set; }

        /// <summary>
        /// 角色ID為1的默認為超級管理員
        /// </summary>
        public bool IsSuperAdmin
        {
            get { return IsRoleIdSuperAdmin(this.RoleId); }
        }
        /// <summary>
        /// 角色ID為1的默認為超級管理員
        /// </summary>
        public static bool IsRoleIdSuperAdmin(int roleId)
        {
            return roleId == 1;
        }

        public UserInfo GetUserInfo(int userId)
        {
            if (_userInfo != null) return _userInfo;
            if (userId <= 0)
            {
                _userInfo = new UserInfo();
                return _userInfo;
            }
            string key = userId.GetUserIdKey();
            _userInfo = CacheService.Get<UserInfo>(key);
            if (_userInfo != null && _userInfo.User_Id > 0) return _userInfo;

            _userInfo = DBServerProvider.DbContext.Set<Sys_User>()
                .Where(x => x.User_Id == userId).Select(s => new
                {
                    User_Id = userId,
                    Role_Id = s.Role_Id.GetInt(),
                    RoleName = s.RoleName,
                    //2022.08.15增加部門id
                    DeptId = s.Dept_Id??0,
                    Token = s.Token,
                    UserName = s.UserName,
                    UserTrueName = s.UserTrueName,
                    Enable = s.Enable,
                    DeptIds= s.DeptIds
                }).ToList().Select(s => new UserInfo()
                {
                    User_Id = userId,
                    Role_Id = s.Role_Id,
                    Token = s.Token,
                    UserName = s.UserName,
                    UserTrueName = s.UserTrueName,
                    Enable = 1,
                    DeptIds = string.IsNullOrEmpty(s.DeptIds) ? new List<Guid>() : s.DeptIds.Split(",").Select(x => (Guid)x.GetGuid()).ToList(),
                }).FirstOrDefault();

            if (_userInfo != null && _userInfo.User_Id > 0)
            {
                CacheService.AddObject(key, _userInfo);
            }
            return _userInfo ?? new UserInfo();
        }

        /// <summary>
        /// 獲取角色權限時通過安全字典鎖定的角色id
        /// </summary>
        private static ConcurrentDictionary<string, object> objKeyValue = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 角色權限的版本號
        /// </summary>
        private static readonly Dictionary<int, string> rolePermissionsVersion = new Dictionary<int, string>();

        /// <summary>
        /// 每個角色ID對應的菜單權限（已做静態化處理）
        /// 每次獲取權限時用當前服務器的版本號與redis/memory緩存的版本比較,如果不同會重新刷新緩存
        /// </summary>
        private static readonly Dictionary<int, List<Permissions>> rolePermissions = new Dictionary<int, List<Permissions>>();



        /// <summary>
        /// 獲取用戶所有的菜單權限
        /// </summary>

        public List<Permissions> Permissions
        {
            get
            {
                return GetPermissions(RoleId);
            }
        }

        /// <summary>
        /// 菜單按鈕變更時，同時刷新權限緩存2022.05.23
        /// </summary>
        /// <param name="menuId"></param>
        public void RefreshWithMenuActionChange(int menuId)
        {
            foreach (var roleId in rolePermissions.Where(c => c.Value.Any(x => x.Menu_Id == menuId)).Select(s => s.Key))
            {
                if (rolePermissionsVersion.ContainsKey(roleId))
                {
                    CacheService.Add(roleId.GetRoleIdKey(), DateTime.Now.ToString("yyyyMMddHHMMssfff"));
                }
            }

        }

        /// <summary>
        /// 獲取單個表的權限
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Permissions GetPermissions(string tableName)
        {
            return GetPermissions(RoleId).Where(x => x.TableName == tableName).FirstOrDefault();
        }
        /// <summary>
        /// 2022.03.26
        /// 菜單類型1:移動端，0:PC端
        /// </summary>
        public static int MenuType
        {
            get
            {
                return Context.Request.Headers.ContainsKey("uapp") ? 1 : 0;
            }
        }
        /// <summary>
        /// 自定條件查詢權限
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public Permissions GetPermissions(Func<Permissions, bool> func)
        {
            // 2022.03.26增移動端加菜單類型判斷
            return GetPermissions(RoleId).Where(func).Where(x => x.MenuType == MenuType).FirstOrDefault();
        }

        private List<Permissions> ActionToArray(List<Permissions> permissions)
        {
            permissions.ForEach(x =>
            {
                try
                {
                    var menuAuthArr = x.MenuAuth.DeserializeObject<List<Sys_Actions>>();
                    x.UserAuthArr = string.IsNullOrEmpty(x.UserAuth)
                    ? new string[0]
                    : x.UserAuth.Split(",").Where(c => menuAuthArr.Any(m => m.Value == c)).ToArray();

                }
                catch { }
                finally
                {
                    if (x.UserAuthArr == null)
                    {
                        x.UserAuthArr = new string[0];
                    }
                }
            });
            return permissions;
        }
        private List<Permissions> MenuActionToArray(List<Permissions> permissions)
        {
            permissions.ForEach(x =>
            {
                try
                {
                    x.UserAuthArr = string.IsNullOrEmpty(x.UserAuth)
                    ? new string[0]
                    : x.UserAuth.DeserializeObject<List<Sys_Actions>>().Select(s => s.Value).ToArray();
                }
                catch { }
                finally
                {
                    if (x.UserAuthArr == null)
                    {
                        x.UserAuthArr = new string[0];
                    }
                }
            });
            return permissions;
        }
        public List<Permissions> GetPermissions(int roleId)
        {
            if (IsRoleIdSuperAdmin(roleId))
            {
                //2020.12.27增加菜單介面上不顯示，但可以分配權限
                var permissions = DBServerProvider.DbContext.Set<Sys_Menu>()
                    .Where(x => x.Enable == 1 || x.Enable == 2)
                    .Select(a => new Permissions
                    {
                        Menu_Id = a.Menu_Id,
                        ParentId = a.ParentId,
                        //2020.05.06增加默認將表名轉換成小寫，權限驗證時不再轉換
                        TableName = (a.TableName ?? "").ToLower(),
                        //MenuAuth = a.Auth,
                        UserAuth = a.Auth,
                        // 2022.03.26增移動端加菜單類型
                        MenuType = a.MenuType ?? 0
                    }).ToList();
                return MenuActionToArray(permissions);
            }
            ICacheService cacheService = CacheService;
            string roleKey = roleId.GetRoleIdKey();

            //角色有緩存，並且當前服務器的角色版本號與redis/memory緩存角色的版本號相同直接返回静態對象角色權限
            string currnetVeriosn = "";
            if (rolePermissionsVersion.TryGetValue(roleId, out currnetVeriosn)
                && currnetVeriosn == cacheService.Get(roleKey))
            {
                return rolePermissions.ContainsKey(roleId) ? rolePermissions[roleId] : new List<Permissions>();
            }

            //鎖定每個角色，通過安全字典减少鎖粒度，否則多個同時角色獲取緩存會導致阻塞
            object objId = objKeyValue.GetOrAdd(roleId.ToString(), new object());
            //鎖定每個角色
            lock (objId)
            {
                if (rolePermissionsVersion.TryGetValue(roleId, out currnetVeriosn)
                    && currnetVeriosn == cacheService.Get(roleKey))
                {
                    return rolePermissions.ContainsKey(roleId) ? rolePermissions[roleId] : new List<Permissions>();
                }

                //沒有redis/memory緩存角色的版本號或與當前服務器的角色版本號不同時，刷新緩存
                var dbContext = DBServerProvider.DbContext;
                List<Permissions> _permissions = (from a in dbContext.Set<Sys_Menu>()
                                                  join b in dbContext.Set<Sys_RoleAuth>()
                                                  on a.Menu_Id equals b.Menu_Id
                                                  where b.Role_Id == roleId //&& a.ParentId > 0
                                                  && b.AuthValue != ""
                                                  orderby a.ParentId
                                                  select new Permissions
                                                  {
                                                      Menu_Id = a.Menu_Id,
                                                      ParentId = a.ParentId,
                                                      //2020.05.06增加默認將表名轉換成小寫，權限驗證時不再轉換
                                                      TableName = (a.TableName ?? "").ToLower(),
                                                      MenuAuth = a.Auth,
                                                      UserAuth = b.AuthValue ?? "",
                                                      // 2022.03.26增移動端加菜單類型
                                                      MenuType = a.MenuType ?? 0
                                                  }).ToList();
                ActionToArray(_permissions);
                string _version = cacheService.Get(roleKey);
                //生成一個唯一版本號標識
                if (_version == null)
                {
                    _version = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                    //將版本號寫入緩存
                    cacheService.Add(roleKey, _version);
                }
                //刷新當前服務器角色的權限
                rolePermissions[roleId] = _permissions;

                //寫入當前服務器的角色最新版本號
                rolePermissionsVersion[roleId] = _version;
                return _permissions;
            }

        }

        /// <summary>
        /// 判斷是否有權限
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="authName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool ExistsPermissions(string tableName, string authName, int roleId = 0)
        {
            if (roleId <= 0) roleId = RoleId;
            tableName = tableName.ToLower();
            return GetPermissions(roleId).Any(x => x.TableName == tableName && x.UserAuthArr.Contains(authName));
        }

        /// <summary>
        /// 判斷是否有權限
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="authName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool ExistsPermissions(string tableName, ActionPermissionOptions actionPermission, int roleId = 0)
        {
            return ExistsPermissions(tableName, actionPermission.ToString(), roleId);
        }
        public int UserId
        {
            get
            {
                return (Context.User.FindFirstValue(JwtRegisteredClaimNames.Jti)
                    ?? Context.User.FindFirstValue(ClaimTypes.NameIdentifier)).GetInt();
            }
        }

        public string UserName
        {
            get { return UserInfo.UserName; }
        }

        public string UserTrueName
        {
            get { return UserInfo.UserTrueName; }
        }

        public string Token
        {
            get { return UserInfo.Token; }
        }

        public int RoleId
        {
            get { return UserInfo.Role_Id; }
        }
        public List<Guid> DeptIds
        {
            get { return UserInfo.DeptIds; }
        }
        /// <summary>
        /// 獲取所有子部門
        /// </summary>
        /// <returns></returns>
        public List<Guid> GetAllChildrenDeptIds()
        {
            return DepartmentContext.GetAllChildrenIds(DeptIds);
        }

        public void LogOut(int userId)
        {
            CacheService.Remove(userId.GetUserIdKey());
        }
    }
}
