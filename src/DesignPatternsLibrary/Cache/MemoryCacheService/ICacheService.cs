using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Cache.MemoryCacheService
{
    public interface ICacheService<TKey,TValue>
    {
        Task<TValue> Get(TKey key);
        void Set(TKey key, TValue entry, MemoryCacheEntryOptions options = null);
        void Remove(TKey key);
    }
}
