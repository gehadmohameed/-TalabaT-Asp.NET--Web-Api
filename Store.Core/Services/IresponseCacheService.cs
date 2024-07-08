using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services
{
    public  interface IresponseCacheService
    {
        //Cache Data
        Task CasheResponseAsync(string CacheKey, object Response, TimeSpan ExpireTime);
        // Get chached Data
        Task<string> GetCachedResponse(string CacheKey);
    }
}
