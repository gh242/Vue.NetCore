using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using VOL.Core.Configuration;
using VOL.Core.Const;
using VOL.Core.Dapper;
using VOL.Core.EFDbContext;
using VOL.Core.Enums;
using VOL.Core.Extensions;

namespace VOL.Core.DBManager
{
    public partial class DBServerProvider
    {
        private static Dictionary<string, string> ConnectionPool = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private static readonly string DefaultConnName = "defalut";

        static DBServerProvider()
        {
            SetConnection(DefaultConnName, AppSetting.DbConnectionString);
        }
        public static void SetConnection(string key, string val)
        {
            if (ConnectionPool.ContainsKey(key))
            {
                ConnectionPool[key] = val;
                return;
            }
            ConnectionPool.Add(key, val);
        }
        /// <summary>
        /// 設置默認數據庫連接
        /// </summary>
        /// <param name="val"></param>
        public static void SetDefaultConnection(string val)
        {
            SetConnection(DefaultConnName, val);
        }

        public static string GetConnectionString(string key)
        {
            key = key ?? DefaultConnName;
            if (ConnectionPool.ContainsKey(key))
            {
                return ConnectionPool[key];
            }
            return key;
        }
        /// <summary>
        /// 獲取默認數據庫連接
        /// </summary>
        /// <returns></returns>
        public static string GetConnectionString()
        {
            return GetConnectionString(DefaultConnName);
        }
        public static IDbConnection GetDbConnection(string connString = null)
        {
            if (connString==null)
            {
                connString = ConnectionPool[DefaultConnName];
            }  
            if (DBType.Name == DbCurrentType.MySql.ToString())
            {
                return new MySqlConnection(connString);
            }
            if (DBType.Name == DbCurrentType.PgSql.ToString())
            {
                return new NpgsqlConnection(connString);
            }
            return new SqlConnection(connString); 
        }


        /// <summary>
        /// 擴展dapper 獲取MSSQL數據庫DbConnection，默認系統獲取配置文件的DBType數據庫類型，
        /// </summary>
        /// <param name="connString">如果connString為null 執行重載GetDbConnection(string connString = null)</param>
        /// <param name="dapperType">指定連接數據庫的類型：MySql/MsSql/PgSql</param>
        /// <returns></returns>
        public static IDbConnection GetDbConnection(string connString = null, DbCurrentType dbCurrentType=DbCurrentType.Default)
        {
            //默認獲取DbConnection
            if (connString.IsNullOrEmpty()|| DbCurrentType.Default== dbCurrentType)
            {
                return GetDbConnection(connString);
            }
            if (dbCurrentType==DbCurrentType.MySql)
            {
                return new MySqlConnection(connString);
            }
            if (dbCurrentType == DbCurrentType.PgSql)
            {
                return new NpgsqlConnection(connString);
            }
            return new SqlConnection(connString);

        }

        public static VOLContext DbContext
        {
            get { return GetEFDbContext(); }
        }
        public static VOLContext GetEFDbContext()
        {
            return GetEFDbContext(null);
        }
        public static VOLContext GetEFDbContext(string dbName)
        {
            VOLContext context = Utilities.HttpContext.Current.RequestServices.GetService(typeof(VOLContext)) as VOLContext;
            if (dbName != null)
            {
                if (!ConnectionPool.ContainsKey(dbName))
                {
                    throw new Exception("數據庫連接名稱錯誤");
                }
                context.Database.GetDbConnection().ConnectionString = ConnectionPool[dbName];
            }
            return context;
        }

        public static void SetDbContextConnection(VOLContext beefContext, string dbName)
        {
            if (!ConnectionPool.ContainsKey(dbName))
            {
                throw new Exception("數據庫連接名稱錯誤");
            }
            beefContext.Database.GetDbConnection().ConnectionString = ConnectionPool[dbName];
        }
        /// <summary>
        /// 獲取實體的數據庫連接
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="defaultDbContext"></param>
        /// <returns></returns>
        public static void GetDbContextConnection<TEntity>(VOLContext defaultDbContext)
        {
            //string connstr= defaultDbContext.Database.GetDbConnection().ConnectionString;
            // if (connstr != ConnectionPool[DefaultConnName])
            // {
            //     defaultDbContext.Database.GetDbConnection().ConnectionString = ConnectionPool[DefaultConnName];
            // };
        }

        public static ISqlDapper SqlDapper
        {
            get
            {
                return new SqlDapper(DefaultConnName);
            }
        }
        public static ISqlDapper GetSqlDapper(string dbName = null)
        {
            return new SqlDapper(dbName ?? DefaultConnName);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbCurrentType">指定數據庫類型：MySql/MsSql/PgSql</param>
        /// <param name="dbName">指定數據連串名稱</param>
        /// <returns></returns>
        public static ISqlDapper GetSqlDapper(DbCurrentType dbCurrentType, string dbName = null)
        {
            if (dbName.IsNullOrEmpty())
            {
                return new SqlDapper(dbName ?? DefaultConnName);
            }
            return new SqlDapper(dbName, dbCurrentType);
        }
        public static ISqlDapper GetSqlDapper<TEntity>()
        {
            //獲取實體真實的數據庫連接池對象名，如果不存在則用默認數據連接池名
            string dbName = typeof(TEntity).GetTypeCustomValue<DBConnectionAttribute>(x => x.DBName) ?? DefaultConnName;
            return GetSqlDapper(dbName);
        }

    }
}
