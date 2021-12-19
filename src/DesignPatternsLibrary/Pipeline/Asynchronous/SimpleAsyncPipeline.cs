using System;
using System.Collections.Concurrent;
using System.Threading;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    internal class SimpleAsyncPipeline
    {
        private readonly ConcurrentQueue<Action> _jobs = new ConcurrentQueue<Action>();

        public SimpleAsyncPipeline()
        {
            Thread thread = new Thread(new ThreadStart(OnStart));
            thread.IsBackground = true;
            thread.Start();
        }

        public void Enqueue(Action job) => _jobs.Enqueue(job);

        private void OnStart()
        {
            while (true)
            {
                if (_jobs.TryDequeue(out Action action))
                {
                    action?.Invoke();
                }
            }
        }
    }
}
