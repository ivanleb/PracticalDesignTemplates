using System;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    internal class FailedJobQueue<T> : IJobQueue<IContext<T>>
    {
        public void Enqueue(IContext<T> ctx)
        {
            LogErorrData(ctx.Data);
        }

        private void LogErorrData(T data) 
        {
            Console.WriteLine(data);
        }
    }
}
