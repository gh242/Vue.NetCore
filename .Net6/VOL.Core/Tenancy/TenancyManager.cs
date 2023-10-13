using System;
using System.Collections.Generic;
using System.Text;
using VOL.Core.ManageUser;

namespace VOL.Core.Tenancy
{
    public static class TenancyManager<T> where T : class
    {
        /// <summary>
        /// 數據庫表名
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetSearchQueryable(string tableName)
        {
            string multiTenancyString = $"select * from {tableName}";
            //超級管理員不限制
            //if (UserContext.Current.IsSuperAdmin)
            //{
            //    return multiTenancyString;
            //}
            switch (tableName)
            {
                //例如：指定用戶表指定查詢條件
                //case "Sys_User": 
                //    multiTenancyString += $" where UserId='{UserContext.Current.UserId}'";
                //    break;
                default:
                    //開啟數租戶數據隔離,用戶只能看到自己的表數據(自己根據需要寫條件做租戶數據隔離)
                    multiTenancyString += $" where CreateID='{UserContext.Current.UserId}'";
                    break;
            }
            return multiTenancyString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName">數據庫表名</param>
        /// <param name="ids">當前操作的所有id</param>
        /// <param name="tableKey">主鍵字段</param>
        /// <returns></returns>
        public static string GetMultiTenancySql(string tableName, string ids, string tableKey)
        {
            //使用方法同上
            string multiTenancyString;
            switch (tableName)
            {
                default:
                    multiTenancyString = $"select count(*) FROM {tableName} " +
                       $" where CreateID='{UserContext.Current.UserId}'" +
                       $" and  { tableKey} in ({ids}) ";
                    break;
            }
            return multiTenancyString;
        }
    }
}
