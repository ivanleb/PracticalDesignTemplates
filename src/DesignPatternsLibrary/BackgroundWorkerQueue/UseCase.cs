using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.BackgroundWorkerQueue
{
    public static class UseCase
    {
        public static async Task Run()
        {
            Action action1 = () => 
            { 
                Thread.Sleep(TimeSpan.FromSeconds(5));
                Console.WriteLine("action1");
            };

            Action action2 = () =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                Console.WriteLine("action2");
            };

            Func<string> action3 = () => 
            {
                Thread.Sleep(TimeSpan.FromSeconds(10));
                return "action3";
            };

            var queue = new BackgroundWorkerQueue();
            queue.QueueTask(action1);
            queue.QueueTask(action2);

            var result = await queue.QueueTask(action3);

            Console.WriteLine(result);
        }
    }
}
