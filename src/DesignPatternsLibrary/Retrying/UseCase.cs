using DesignPatternsLibrary.Observer.DispatchedObserver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Retrying
{
    public static class UseCase
    {
        public static void Run()
        {
            int retryCount = 3;
            Func<Task<string>> action = async () =>
            {
                // Simulate an operation that may fail
                await Task.Delay(100);
                throw new Exception("Simulated failure");
            };
            try
            {
                var result = SimpleAsyncRetry.RetryAsync(retryCount, action).GetAwaiter().GetResult();
                Console.WriteLine($"Operation succeeded with result: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Operation failed after {retryCount} retries: {ex.Message}");
            }
        }
    }

    public class SimpleAsyncRetry 
    {
        public static async Task<T> RetryAsync<T>(int retries, Func<Task<T>> action)
        {
            while (retries > 0)
            {
                try
                {
                    return await action();
                }
                catch
                {
                    retries--;
                    if (retries == 0) throw;
                }
            }
            return default(T);
        }
    }
}
