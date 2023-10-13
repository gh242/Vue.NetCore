using CSRedis;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using VOL.Core.Configuration;
using VOL.Core.Const;

namespace VOL.Core.CacheManager
{
    public class RedisCacheService : ICacheService
    {
        public RedisCacheService()
        {
            var csredis = new CSRedisClient(AppSetting.RedisConnectionString);
            RedisHelper.Initialization(csredis);
        }

        /// <summary>
        /// 驗證緩存項是否存在
        /// </summary>
        /// <param name="key">緩存Key</param>
        /// <returns></returns>
        public bool Exists(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return RedisHelper.Exists(key);
        }

        public void LPush(string key, string val)
        {
            RedisHelper.LPush(key, val);
        }

        public void RPush(string key, string val)
        {
            RedisHelper.RPush(key, val);
        }

        public T ListDequeue<T>(string key) where T : class
        {
            string value = RedisHelper.RPop(key);
            if (string.IsNullOrEmpty(value))
                return null;
            return JsonConvert.DeserializeObject<T>(value);
        }
        public object ListDequeue(string key)
        {
            string value = RedisHelper.RPop(key);
            if (string.IsNullOrEmpty(value))
                return null;
            return value;
        }

        /// <summary>
        /// 移除list中的數據，keepIndex為保留的位置到最後一個元素如list 元素為1.2.3.....100
        /// 需要移除前3個數，keepindex應該為4
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keepIndex"></param>
        public void ListRemove(string key, int keepIndex)
        {
            RedisHelper.LTrim(key, keepIndex, -1);
        }
        public bool Add(string key, string value, int expireSeconds = -1, bool isSliding = false)
        {
            return RedisHelper.Set(key, value, expireSeconds);
        }
        public bool AddObject(string key, object value, int expireSeconds = -1, bool isSliding = false)
        {
            return RedisHelper.Set(key, value, expireSeconds);
        }

        /// <summary>
        /// 刪除緩存
        /// </summary>
        /// <param name="key">緩存Key</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            RedisHelper.Del(key);
            return true;
        }
        /// <summary>
        /// 批量刪除緩存
        /// </summary>
        /// <param name="key">緩存Key集合</param>
        /// <returns></returns>
        public void RemoveAll(IEnumerable<string> keys)
        {
            RedisHelper.Del(keys.ToArray());
        }
        /// <summary>
        /// 獲取緩存
        /// </summary>
        /// <param name="key">緩存Key</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            return RedisHelper.Get<T>(key);
        }
        /// <summary>
        /// 獲取緩存
        /// </summary>
        /// <param name="key">緩存Key</param>
        /// <returns></returns>
        public string Get(string key)
        {
            return RedisHelper.Get(key);
        }
        public void Dispose()
        {
        }
    }
}
