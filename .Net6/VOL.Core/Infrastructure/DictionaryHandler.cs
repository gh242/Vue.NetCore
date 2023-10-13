using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using VOL.Core.Const;
using VOL.Core.Enums;
using VOL.Core.ManageUser;
using VOL.Core.UserManager;

namespace VOL.Core.Infrastructure
{
    public static class DictionaryHandler
    {
        /*2020.05.01增加根據用戶信息加載字典數據源sql*/

        /// <summary>
        /// 獲取自定義數據源sql
        /// </summary>
        /// <param name="dicNo"></param>
        /// <param name="originalSql"></param>
        /// <returns></returns>
        public static string GetCustomDBSql(string dicNo, string originalSql)
        {
            switch (dicNo)
            {
                case "roles":
                //2020.05.24增加綁定table表時，獲取所有的角色列表
                //注意，如果是2020.05.24之前獲取的數據庫脚本
                //請在菜單【下拉框綁定設置】添加一個字典編號【t_roles】,除了字典編號，其他内容随便填寫
                case "t_roles":
                case "tree_roles":
                    originalSql = GetRolesSql(originalSql);
                    break;
                default:
                    break;
            }
            return originalSql;
        }

        /// <summary>
        /// 獲取解決的數據源，只能看到自己與下級所有角色
        /// </summary>
        /// <param name="context"></param>
        /// <param name="originalSql"></param>
        /// <returns></returns>
        public static string GetRolesSql(string originalSql)
        {
         
            if (UserContext.Current.IsSuperAdmin)
            {
                return originalSql;
            }

            int currnetRoleId = UserContext.Current.RoleId;
            List<int> roleIds = RoleContext.GetAllChildrenIds(currnetRoleId);
            roleIds.Add(currnetRoleId);
            if (DBType.Name == DbCurrentType.PgSql.ToString())
            {
                originalSql = $"SELECT \"Role_Id\" as key,\"Role_Id\" as id,\"RoleName\" as value,\"ParentId\" AS parentId from Sys_Role"
                   +$" where \"Role_Id\"  in ({string.Join(',', roleIds)})";
            }
            else {
                originalSql= $@"SELECT Role_Id as 'key',Role_Id AS id,ParentId AS parentId,RoleName as 'value' FROM Sys_Role 
                      WHERE Enable=1  and Role_Id in ({string.Join(',', roleIds)})"; 
            }
            return originalSql;
        }
    }
}
