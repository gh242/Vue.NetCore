
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VOL.Core.CacheManager
{
    public class MemoryCacheService : ICacheService
    {
        protected IMemoryCache _cache;
        public MemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;

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
            return _cache.Get(key) != null;
        }

        /// <summary>
        /// 添加緩存
        /// </summary>
        /// <param name="key">緩存Key</param>
        /// <param name="value">緩存Value</param>
        /// <returns></returns>
        public bool Add(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            _cache.Set(key, value);
            return Exists(key);
        }

        public bool AddObject(string key, object value, int expireSeconds = -1, bool isSliding = false)
        {
            if (expireSeconds != -1)
            {
                _cache.Set(key,
                    value,
                    new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(new TimeSpan(0, 0, expireSeconds))
                    );
            }
            else
            {
                _cache.Set(key, value);
            }

            return true;
        }
        public bool Add(string key, string value, int expireSeconds = -1, bool isSliding = false)
        {
            return AddObject(key, value, expireSeconds, isSliding);
        }
        public void LPush(string key, string val)
        {
        }
        public void RPush(string key, string val)
        {
        }
        public T ListDequeue<T>(string key) where T : class
        {
            return null;
        }
        public object ListDequeue(string key)
        {
            return null;
        }
        public void ListRemove(string key, int keepIndex)
        {
        }
        /// <summary>
        /// 添加緩存
        /// </summary>
        /// <param name="key">緩存Key</param>
        /// <param name="value">緩存Value</param>
        /// <param name="expiresSliding">滑動過期時長（如果在過期時間内有操作，則以當前時間點延長過期時間）</param>
        /// <param name="expiressAbsoulte">絕對過期時長</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan expiresSliding, TimeSpan expiressAbsoulte)
        {
            _cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(expiresSliding)
                    .SetAbsoluteExpiration(expiressAbsoulte)
                    );

            return Exists(key);
        }
        /// <summary>
        /// 添加緩存
        /// </summary>
        /// <param name="key">緩存Key</param>
        /// <param name="value">緩存Value</param>
        /// <param name="expiresIn">緩存時長</param>
        /// <param name="isSliding">是否滑動過期（如果在過期時間内有操作，則以當前時間點延長過期時間）</param>
        /// <returns></returns>
        public bool Add(string key, object value, TimeSpan expiresIn, bool isSliding = false)
        {
            if (isSliding)
                _cache.Set(key, value,
                    new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(expiresIn)
                    );
            else
                _cache.Set(key, value,
                new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(expiresIn)
                );

            return Exists(key);
        }



        /// <summary>
        /// 刪除緩存
        /// </summary>
        /// <param name="key">緩存Key</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            _cache.Remove(key);

            return !Exists(key);
        }
        /// <summary>
        /// 批量刪除緩存
        /// </summary>
        /// <param name="key">緩存Key集合</param>
        /// <returns></returns>
        public void RemoveAll(IEnumerable<string> keys)
        {
            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            keys.ToList().ForEach(item => _cache.Remove(item));
        }
        public string Get(string key)
        {
            return _cache.Get(key)?.ToString();
        }
        /// <summary>
        /// 獲取緩存
        /// </summary>
        /// <param name="key">緩存Key</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : class
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return _cache.Get(key) as T;
        }

        public void Dispose()
        {
            if (_cache != null)
                _cache.Dispose();
            GC.SuppressFinalize(this);
        }


    }
}
