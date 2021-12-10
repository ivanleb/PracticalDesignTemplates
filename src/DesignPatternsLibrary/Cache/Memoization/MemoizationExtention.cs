using System;
using System.Collections.Concurrent;

namespace DesignPatternsLibrary.Cache.Memoization
{
    public static class MemoizationExtention
    {
        public static Func<T, TResult> Memoize<T, TResult>(this Func<T, TResult> func)
        {
            var cache = new ConcurrentDictionary<T, TResult>();
            return a =>  cache.GetOrAdd(a, func);
        }

        public static Func<T, TResult> LazyMemoize<T, TResult>(this Func<T, TResult> f)
        {
            var cache = new ConcurrentDictionary<T, Lazy<TResult>>();
            return a => cache.GetOrAdd(a, new Lazy<TResult>(() => f(a))).Value;
        }
    }
}
