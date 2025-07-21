using ChatApp.Application.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatApp.Infrastructure.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer connection)
        {
            _db = connection.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                var data = await _db.StringGetAsync(key);
                return data.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(data);
            }
            catch (RedisConnectionException ex)
            {
                Console.WriteLine($"[Redis] GetAsync failed: {ex.Message}");
                return default; // fallback to DB query
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            try
            {
                var json = JsonSerializer.Serialize(value);
                await _db.StringSetAsync(key, json, expiration);
            }
            catch (RedisConnectionException ex)
            {
                Console.WriteLine($"[Redis] SetAsync failed: {ex.Message}");
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _db.KeyDeleteAsync(key);
            }
            catch (RedisConnectionException ex)
            {
                Console.WriteLine($"[Redis] RemoveAsync failed: {ex.Message}");
                // Optional: log somewhere or just swallow the error in production
            }
        }
    }
}
