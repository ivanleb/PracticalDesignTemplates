using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public class BlockingCollectionAsyncPipeline
    {
        private readonly BlockingCollection<Action> _jobs = new BlockingCollection<Action>(new ConcurrentQueue<Action>());

        public BlockingCollectionAsyncPipeline()
        {
            Thread thread = new Thread(new ThreadStart(OnStart));
            thread.IsBackground = true;
            thread.Start();
        }

        public void Enqueue(Action job) => _jobs.Add(job);
        
        private void OnStart()
        {
            foreach (Action job in _jobs.GetConsumingEnumerable(CancellationToken.None))
            {
                job?.Invoke();
            }
        }
    }
}
