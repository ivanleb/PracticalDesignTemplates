using System;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Cache.OneVariableCache
{
    public static class UseCase
    {
        public static void Run()
        {
            Console.WriteLine("CachedValue use case");
            CachedValue<String> cachedValue = new CachedValue<String>(() => $"Cached String {DateTime.Now}", TimeSpan.FromSeconds(5));
            String value = cachedValue.GetValue();
            Console.WriteLine(value);
            Task.Delay(6000).Wait(); // Wait for 6 seconds to ensure the cache expires
            value = cachedValue.GetValue(); // This should trigger a new value to be generated
            Console.WriteLine(value);
        }
    }
}
