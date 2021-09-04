using Newtonsoft.Json;
using StackExchange.Redis.Extensions.Core.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ConnectionConfig.Helpers
{
    public class RedisHelpers
    {
        private static TimeSpan _expireTimeCache = new TimeSpan(1, 0, 0, 0);
        private static IRedisCacheClient _redisCacheClient;
        public RedisHelpers(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;
        }
        #region basic method in redis
        /// <summary>
        /// Set data to redis (add or update)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<T> SetData<T>(string key, T data)
        {
            bool res = false;
            var convertData = JsonConvert.SerializeObject(data);
            var checkData = await _redisCacheClient.Db0.ExistsAsync(key);
            if (checkData)
                res = await _redisCacheClient.Db0.ReplaceAsync(key, convertData, _expireTimeCache);
            else
                res = await _redisCacheClient.Db0.AddAsync(key, convertData, _expireTimeCache);
            return (T)Convert.ChangeType(res, typeof(T));
        }
        /// <summary>
        /// check exist data in redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<T> CheckExistData<T>(string key, T data)
        {
            var res = await _redisCacheClient.Db0.ExistsAsync(key);
            return (T)Convert.ChangeType(res, typeof(T));
        }
        /// <summary>
        /// remove data from redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> RemoveData<T>(string key)
        {
            var res = await _redisCacheClient.Db0.RemoveAsync(key);
            return (T)Convert.ChangeType(res, typeof(T));
        }
        /// <summary>
        /// Get data from redis
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> GetData<T>(string key)
        {
            var value = await _redisCacheClient.Db0.GetAsync<T>(key);
            return (T)Convert.ChangeType(value, typeof(T));
        }
        /// <summary>
        /// Flush db
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task FlushAllDb<T>()
        {
            await _redisCacheClient.Db0.FlushDbAsync();
            return;
        }
        #endregion

        #region extension method in redis
        /// <summary>
        /// Get multi keys
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static async Task<IDictionary<string, T>> GetDataFromMultiKeys<T>(List<string> keys)
        {
            var dictVals = await _redisCacheClient.Db0.GetAllAsync<T>(keys);
            return dictVals;
        }
        /// <summary>
        /// Remove multi keys
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static async Task<T> RemoveMultiKeys<T>(List<string> keys)
        {
            var res = await _redisCacheClient.Db0.RemoveAllAsync(keys);
            return (T)Convert.ChangeType(res, typeof(T));
        }
        /// <summary>
        /// Remove by tag
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static async Task<T> RemovePrefixKey<T>(string tag)
        {
            var res = await _redisCacheClient.Db0.RemoveByTagAsync(tag);
            return (T)Convert.ChangeType(res, typeof(T));
        }
        /// <summary>
        /// Get values by tag
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static async Task<T> GetValuesByPrefixKeys<T>(string tag)
        {
            var res = await _redisCacheClient.Db0.GetByTagAsync<T>(tag);
            return (T)Convert.ChangeType(res, typeof(T));
        }
        /// <summary>
        /// Search key with same pattern
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static async Task<T> SearchKey<T>(string pattern)
        {
            var res = await _redisCacheClient.Db0.SearchKeysAsync(pattern);
            return (T)Convert.ChangeType(res, typeof(T));
        }
        #endregion
    }
}