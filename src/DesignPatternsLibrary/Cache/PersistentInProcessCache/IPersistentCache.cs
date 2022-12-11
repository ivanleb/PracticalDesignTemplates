using System;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Cache.PersistentInProcessCache
{
    public interface IPersistentCache<TKey, TValue> : IDisposable
    {
        TValue Get(TKey key);
        void Set(TKey key, TValue entry);
        void Remove(TKey key);
    }
}
