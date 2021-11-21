using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Cache.MemoryCacheService
{
    public class CacheService<TKey, TValue> : ICacheService<TKey, TValue>
    {
        protected readonly IMemoryCache _cache;
        public Task<TValue> Get(TKey key)
        {
            if (_cache.TryGetValue(key, out TValue entry)) 
                return Task.FromResult(entry);

            return Task.FromResult(default(TValue));
        }

        public void Remove(TKey key) => _cache.Remove(key);
        

        public void Set(TKey key, TValue entry, MemoryCacheEntryOptions options = null)
        {
            _cache.Set(key, entry, options ?? new MemoryCacheEntryOptions());
        }
    }
}
