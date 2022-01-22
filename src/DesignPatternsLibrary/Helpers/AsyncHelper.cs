using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Helpers
{
    public static class AwaitableInt 
    {
        public static TaskAwaiter GetAwaiter(this Int32 miliseconds) 
        {
            return Task.Delay(TimeSpan.FromMilliseconds(miliseconds)).GetAwaiter();
        }

        public static TaskAwaiter GetAwaiter(this TimeSpan interval)
        {
            return Task.Delay(interval).GetAwaiter();
        }
    }
}
