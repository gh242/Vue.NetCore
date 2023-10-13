using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VOL.Core.CacheManager;
using VOL.Core.DBManager;
using VOL.Core.Extensions.AutofacManager;
using VOL.Core.Services;
using VOL.Entity.DomainModels;

namespace VOL.Core.Infrastructure
{
    public static class DictionaryManager
    {
        private static List<Sys_Dictionary> _dictionaries { get; set; }

        private static object _dicObj = new object();
        private static string _dicVersionn = "";
        public const string Key = "inernalDic";

        public static List<Sys_Dictionary> Dictionaries
        {
            get
            {
                return GetAllDictionary();
            }
        }

        public static Sys_Dictionary GetDictionary(string dicNo)
        {
            return GetDictionaries(new string[] { dicNo }).FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicNos"></param>
        /// <param name="executeSql">是否執行自定義sql</param>
        /// <returns></returns>
        public static IEnumerable<Sys_Dictionary> GetDictionaries(IEnumerable<string> dicNos, bool executeSql = true)
        {
            static List<Sys_DictionaryList> query(string sql)
            {
                try
                {
                    return DBServerProvider.SqlDapper.QueryList<SourceKeyVaule>(sql, null).Select(s => new Sys_DictionaryList()
                    {
                        DicName = s.Value,
                        DicValue = s.Key.ToString()
                    }).ToList();
                }
                catch (Exception ex)
                {
                    Logger.Error($"字典執行sql異常,sql:{sql},異常信息：{ex.Message + ex.StackTrace}");
                    throw ex;
                    //  Console.WriteLine(ex.Message);
                    // return null;
                }
            }
            foreach (var item in Dictionaries.Where(x => dicNos.Contains(x.DicNo)))
            {
                if (executeSql)
                {
                    //  2020.05.01增加根據用戶信息加載字典數據源sql
                    string sql = DictionaryHandler.GetCustomDBSql(item.DicNo, item.DbSql);
                    if (!string.IsNullOrEmpty(item.DbSql))
                    {
                        item.Sys_DictionaryList = query(sql);
                    }
                }
                yield return item;
            }
        }
        /// <summary>
        /// 每次變更字典配置的時候會重新拉取所有配置進行緩存(自行根據實際處理)
        /// </summary>
        /// <returns></returns>
        private static List<Sys_Dictionary> GetAllDictionary()
        {
            ICacheService cacheService = AutofacContainerModule.GetService<ICacheService>();
            //每次比較緩存是否更新過，如果更新則重新獲取數據
            if (_dictionaries != null && _dicVersionn == cacheService.Get(Key))
            {
                return _dictionaries;
            }

            lock (_dicObj)
            {
                if (_dicVersionn != "" && _dictionaries != null && _dicVersionn == cacheService.Get(Key)) return _dictionaries;
                _dictionaries = DBServerProvider.DbContext
                    .Set<Sys_Dictionary>()
                    .Where(x => x.Enable == 1)
                    .Include(c => c.Sys_DictionaryList).ToList();

                string cacheVersion = cacheService.Get(Key);
                if (string.IsNullOrEmpty(cacheVersion))
                {
                    cacheVersion = DateTime.Now.ToString("yyyyMMddHHMMssfff");
                    cacheService.Add(Key, cacheVersion);
                }
                else
                {
                    _dicVersionn = cacheVersion;
                }
            }
            return _dictionaries;
        }
    }

    public class SourceKeyVaule
    {
        public object Key { get; set; }
        public string Value { get; set; }
    }
}
