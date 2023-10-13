using System;
using System.Collections.Generic;
using System.Text;
using VOL.Core.Configuration;
using VOL.Core.Dapper;
using VOL.Core.Enums;

namespace VOL.Core.DBManager.Partial
{
    /// <summary>
    /// 2022.11.21增加其他數據庫(sqlserver、mysql、pgsql、oracle)連接配置說明
    /// 需要把兩個DBServerProvider.cs文件都更新下
    /// </summary>
    public partial class DBServerProvider
    {
        ///// <summary>
        ///// 單独配置mysql數據庫
        ///// </summary>
        //public static ISqlDapper SqlDapperMySql
        //{
        //    get
        //    {
        //        //讀取appsettings.json中的配置
        //        string 數據庫連接字符串 = AppSetting.GetSettingString("key");
        //        return new SqlDapper(數據庫連接字符串, DbCurrentType.MySql);

        //        //訪問數據庫方式
        //        //DBServerProvider.SqlDapperMySql.xx
        //    }
        //}


        ///// <summary>
        ///// 如果有多個不同的mysql數據庫，這裡再加一個配置
        ///// </summary>
        //public static ISqlDapper SqlDapperMySql2
        //{
        //    get
        //    {
        //        //讀取appsettings.json中的配置
        //        string 數據庫連接字符串 = AppSetting.GetSettingString("key2");
        //        return new SqlDapper(數據庫連接字符串, DbCurrentType.MySql);

        //        //訪問數據庫方式
        //        //DBServerProvider.SqlDapperMySql2.xx
        //    }
        //}

        ///// <summary>
        ///// 單独配置SqlServer數據庫
        ///// </summary>
        //public static ISqlDapper SqlDapperSqlServer
        //{
        //    get
        //    {
        //        //讀取appsettings.json中的配置
        //        string 數據庫連接字符串 = AppSetting.GetSettingString("key");
        //        return new SqlDapper(數據庫連接字符串, DbCurrentType.MsSql);

        //        //訪問數據庫方式
        //        //DBServerProvider.SqlDapperSqlServer.xx
        //    }
        //}
    }
}
