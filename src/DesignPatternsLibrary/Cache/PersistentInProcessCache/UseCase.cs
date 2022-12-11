using System;

namespace DesignPatternsLibrary.Cache.PersistentInProcessCache
{
    public static class UseCase
    {
        public static void Run() 
        {
            Console.WriteLine("Persistent In Process Cache use case");
            using (IPersistentCache<Guid, String> _cache = new DataBasePersistentCache<String>(@"perCache.db")) 
            {
                Guid key = Guid.NewGuid();

                _cache.Set(key, "First Cached String");

                String value = _cache.Get(key);

                Console.WriteLine(value);
            }
        }
    }
}
