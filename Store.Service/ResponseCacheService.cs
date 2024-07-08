using StackExchange.Redis;
using Store.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Store.Service
{
    public class ResponseCacheService : IresponseCacheService
    {
        private readonly IDatabase _database;
        public ResponseCacheService(IConnectionMultiplexer Redis)
        {
            _database = Redis.GetDatabase();
            
        }
        public async Task CasheResponseAsync(string CacheKey, object Response, TimeSpan ExpireTime)
        {
            if (Response is null) return;
            var Options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var SerializedRespone = JsonSerializer.Serialize(Response, Options);
           await _database.StringSetAsync(CacheKey, SerializedRespone, ExpireTime);


        }

        public async Task<string?> GetCachedResponse(string CacheKey)
        {
         var CachedResponse =   await _database.StringGetAsync(CacheKey);
            if (CachedResponse.IsNullOrEmpty) return null;
            return CachedResponse;
        }
    }
}
