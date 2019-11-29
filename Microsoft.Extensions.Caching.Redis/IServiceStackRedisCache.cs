using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Caching.Redis
{
    public interface IServiceStackRedisCache
    {
        bool Set<T>(string Key, T t);
        bool Set<T>(string Key, T t, int Min);
        T Get<T>(string Key);
        Task<bool> SetAsync<T>(string Key, T t);
        Task<T> GetAsync<T>(string Key);

        bool StringSet(string redisKey, string redisValue, TimeSpan? expiry = null);
        Task<bool> StringSetAsync(string redisKey, string redisValue, TimeSpan? expiry = null);

        string StringGet(string redisKey, TimeSpan? expiry = null);
        Task<string> StringGetAsync(string redisKey);

        void FlushDatabase();
        Task FlushDatabaseAsync();
        bool KeyExists(string redisKey);
        bool KeyDelete(string redisKey);
        long KeyDelete(IEnumerable<string> redisKeys);
        /// <summary>
        /// 发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<long> PublishAsync<T>(RedisChannel channel, T message);
        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        Task SubscribeAsync(RedisChannel channel, Action<RedisChannel, RedisValue> handle);

        /// <summary>
        /// 设置String结构
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        Task<bool> StringSetAsync<T>(string redisKey, T redisValue, TimeSpan? expiry = null);

        long StringIncrement(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None);
        long StringDecrement(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None);
    }
}















