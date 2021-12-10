using System;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Cache.Memoization
{
    public static class UseCase
    {
        public static void Run() 
        {
            Func<string, int> func = s => int.Parse(s);
            Func<string, int> memoizedFunc = func.Memoize();

            Random random = new Random();
            Parallel.For(0, 1000, i => memoizedFunc(random.Next(0, 100).ToString()));
        }
    }
}
