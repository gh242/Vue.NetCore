using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOL.Core.Extensions;
using VOL.Core.ManageUser;
using VOL.Core.Services;
using VOL.Core.Utilities;
using VOL.Entity;
using VOL.Entity.DomainModels;

namespace VOL.System.Services
{
    public partial class Sys_MenuService
    {
        /// <summary>
        /// 菜單静態化處理，每次獲取菜單時先比較菜單是否發生變化，如果發生變化從數據庫重新獲取，否則直接獲取_menus菜單
        /// </summary>
        private static List<Sys_Menu> _menus { get; set; }

        /// <summary>
        /// 從數據庫獲取菜單時鎖定的對象
        /// </summary>
        private static object _menuObj = new object();

        /// <summary>
        /// 當前服務器的菜單版本
        /// </summary>
        private static string _menuVersionn = "";

        private const string _menuCacheKey = "inernalMenu";

        /// <summary>
        /// 編輯修改菜單時,獲取所有菜單
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetMenu()
        {
            //  DBServerProvider.SqlDapper.q
            return (await repository.FindAsync(x => 1 == 1, a =>
             new
             {
                 id = a.Menu_Id,
                 parentId = a.ParentId,
                 name = a.MenuName,
                 a.MenuType,
                 a.OrderNo
             })).OrderByDescending(a => a.OrderNo)
                .ThenByDescending(q => q.parentId).ToList();

        }

        private List<Sys_Menu> GetAllMenu()
        {
            //每次比較緩存是否更新過，如果更新則重新獲取數據
            string _cacheVersion = CacheContext.Get(_menuCacheKey);
            if (_menuVersionn != "" && _menuVersionn == _cacheVersion)
            {
                return _menus ?? new List<Sys_Menu>();
            }
            lock (_menuObj)
            {
                if (_menuVersionn != "" && _menus != null && _menuVersionn == _cacheVersion) return _menus;
                //2020.12.27增加菜單介面上不顯示，但可以分配權限
                _menus = repository.FindAsIQueryable(x => x.Enable == 1 || x.Enable == 2)
                    .OrderByDescending(a => a.OrderNo)
                    .ThenByDescending(q => q.ParentId).ToList();

                _menus.ForEach(x =>
                {
                    // 2022.03.26增移動端加菜單類型
                    x.MenuType ??= 0;
                    if (!string.IsNullOrEmpty(x.Auth) && x.Auth.Length > 10)
                    {
                        try
                        {
                            x.Actions = x.Auth.DeserializeObject<List<Sys_Actions>>();
                        }
                        catch { }
                    }
                    if (x.Actions == null) x.Actions = new List<Sys_Actions>();
                });

                string cacheVersion = CacheContext.Get(_menuCacheKey);
                if (string.IsNullOrEmpty(cacheVersion))
                {
                    cacheVersion = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                    CacheContext.Add(_menuCacheKey, cacheVersion);
                }
                else
                {
                    _menuVersionn = cacheVersion;
                }
            }
            return _menus;
        }

        /// <summary>
        /// 獲取當前用戶有權限查看的菜單
        /// </summary>
        /// <returns></returns>
        public List<Sys_Menu> GetCurrentMenuList()
        {
            int roleId = UserContext.Current.RoleId;
            return GetUserMenuList(roleId);
        }


        public List<Sys_Menu> GetUserMenuList(int roleId)
        {
            if (UserContext.IsRoleIdSuperAdmin(roleId))
            {
                return GetAllMenu();
            }
            List<int> menuIds = UserContext.Current.GetPermissions(roleId).Select(x => x.Menu_Id).ToList();
            return GetAllMenu().Where(x => menuIds.Contains(x.Menu_Id)).ToList();
        }

        /// <summary>
        /// 獲取當前用戶所有菜單與權限
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetCurrentMenuActionList()
        {
            return await GetMenuActionList(UserContext.Current.RoleId);
        }

        /// <summary>
        /// 根據角色ID獲取菜單與權限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<object> GetMenuActionList(int roleId)
        {
            //2020.12.27增加菜單介面上不顯示，但可以分配權限
            if (UserContext.IsRoleIdSuperAdmin(roleId))
            {
                return await Task.Run(() => GetAllMenu()
                .Where(c => c.MenuType == UserContext.MenuType)
                .Select(x =>
                new
                {
                    id = x.Menu_Id,
                    name = x.MenuName,
                    url = x.Url,
                    parentId = x.ParentId,
                    icon = x.Icon,
                    x.Enable,
                    x.TableName, // 2022.03.26增移動端加菜單類型
                    permission = x.Actions.Select(s => s.Value).ToArray()
                }).ToList());
            }

            var menu = from a in UserContext.Current.Permissions
                       join b in GetAllMenu().Where(c => c.MenuType == UserContext.MenuType)
                       on a.Menu_Id equals b.Menu_Id
                       orderby b.OrderNo descending
                       select new
                       {
                           id = a.Menu_Id,
                           name = b.MenuName,
                           url = b.Url,
                           parentId = b.ParentId,
                           icon = b.Icon,
                           b.Enable,
                           b.TableName, // 2022.03.26增移動端加菜單類型
                           permission = a.UserAuthArr
                       };
            return menu.ToList();
        }

        /// <summary>
        /// 新建或編輯菜單
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public async Task<WebResponseContent> Save(Sys_Menu menu)
        {
            WebResponseContent webResponse = new WebResponseContent();
            if (menu == null) return webResponse.Error("沒有獲取到提交的參數");
            if (menu.Menu_Id > 0 && menu.Menu_Id == menu.ParentId) return webResponse.Error("父級ID不能是當前菜單的ID");
            try
            {
                webResponse = menu.ValidationEntity(x => new { x.MenuName, x.TableName });
                if (!webResponse.Status) return webResponse;
                if (menu.TableName != "/" && menu.TableName != ".")
                {
                    // 2022.03.26增移動端加菜單類型判斷
                    Sys_Menu sysMenu = await repository.FindAsyncFirst(x => x.TableName == menu.TableName);
                    if (sysMenu != null)
                    {
                        sysMenu.MenuType ??= 0;
                        if (sysMenu.MenuType == menu.MenuType)
                        {
                            if ((menu.Menu_Id > 0 && sysMenu.Menu_Id != menu.Menu_Id)
                            || menu.Menu_Id <= 0)
                            {
                                return webResponse.Error($"視圖/表名【{menu.TableName}】已被其他菜單使用");
                            }
                        }
                    }
                }
                bool _changed = false;
                if (menu.Menu_Id <= 0)
                {
                    repository.Add(menu.SetCreateDefaultVal());
                }
                else
                {
                    //2020.05.07新增禁止選擇上級角色為自己
                    if (menu.Menu_Id == menu.ParentId)
                    {
                        return webResponse.Error($"父級id不能為自己");
                    }
                    if (repository.Exists(x => x.ParentId == menu.Menu_Id && menu.ParentId == x.Menu_Id))
                    {
                        return webResponse.Error($"不能選擇此父級id，選擇的父級id與當前菜單形成依賴關系");
                    }

                    _changed = repository.FindAsIQueryable(c => c.Menu_Id == menu.Menu_Id).Select(s => s.Auth).FirstOrDefault() != menu.Auth;

                    repository.Update(menu.SetModifyDefaultVal(), p => new
                    {
                        p.ParentId,
                        p.MenuName,
                        p.Url,
                        p.Auth,
                        p.OrderNo,
                        p.Icon,
                        p.Enable,
                        p.MenuType,// 2022.03.26增移動端加菜單類型
                        p.TableName,
                        p.ModifyDate,
                        p.Modifier
                    });
                }
                await repository.SaveChangesAsync();

                CacheContext.Add(_menuCacheKey, DateTime.Now.ToString("yyyyMMddHHMMssfff"));
                if (_changed)
                {
                    UserContext.Current.RefreshWithMenuActionChange(menu.Menu_Id);
                }
                _menus = null;
                webResponse.OK("保存成功", menu);
            }
            catch (Exception ex)
            {
                webResponse.Error(ex.Message);
            }
            finally
            {
                Logger.Info($"表:{menu.TableName},菜單：{menu.MenuName},權限{menu.Auth},{(webResponse.Status ? "成功" : "失敗")}{webResponse.Message}");
            }
            return webResponse;

        }

        public async Task<WebResponseContent> DelMenu(int menuId)
        {
            WebResponseContent webResponse =new  WebResponseContent();
      
            if (await repository.ExistsAsync(x => x.ParentId == menuId))
            {
                return webResponse.Error("當前菜單存在子菜單,請先刪除子菜單!");
            }
            repository.Delete(new Sys_Menu()
            {
                Menu_Id = menuId
            }, true);
            CacheContext.Add(_menuCacheKey, DateTime.Now.ToString("yyyyMMddHHMMssfff"));
            return webResponse.OK("刪除成功");
        }
        /// <summary>
        /// 編輯菜單時，獲取菜單信息
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<object> GetTreeItem(int menuId)
        {
            var sysMenu = (await base.repository.FindAsync(x => x.Menu_Id == menuId))
                .Select(
                p => new
                {
                    p.Menu_Id,
                    p.ParentId,
                    p.MenuName,
                    p.Url,
                    p.Auth,
                    p.OrderNo,
                    p.Icon,
                    p.Enable,
                    // 2022.03.26增移動端加菜單類型
                    MenuType = p.MenuType ?? 0,
                    p.CreateDate,
                    p.Creator,
                    p.TableName,
                    p.ModifyDate
                }).FirstOrDefault();
            return sysMenu;
        }
    }
}

