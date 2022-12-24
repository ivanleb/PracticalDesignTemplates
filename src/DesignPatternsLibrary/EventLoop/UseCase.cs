using System;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.EventLoop
{
    public static class UseCase
    {
        public static async void Run() 
        {
            Console.WriteLine("Event Loop use case");
            var cts = new CancellationTokenSource();
            var eventLoop = new EventLoop<int>(cts.Token, TimeSpan.FromSeconds(3));

            Task tsk = eventLoop.Run((msg) => Console.WriteLine($"Event loop: {msg}"), () => Console.WriteLine("Timeout is expired"));
               
            for (int i = 0; i < 10; i++)
            {
                eventLoop.Push(i);
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            Thread.Sleep(TimeSpan.FromSeconds(20));
            cts.Cancel();
        }
    }
}
