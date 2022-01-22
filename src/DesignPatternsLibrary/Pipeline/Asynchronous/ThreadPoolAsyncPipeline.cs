using System;
using System.Collections.Generic;
using System.Threading;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public class ThreadPoolAsyncPipeline
    {
        private readonly Queue<Action> _jobs = new Queue<Action>();
        private bool _delegateQueuedOrRunning = false;

        public void Enqueue(Action job)
        {
            lock (_jobs)
            {
                _jobs.Enqueue(job);
                if (!_delegateQueuedOrRunning)
                {
                    _delegateQueuedOrRunning = true;
                    ThreadPool.UnsafeQueueUserWorkItem(ProcessQueuedItems, state: null);
                }
            }
        }

        private void ProcessQueuedItems(object? ignored)
        {
            while (true)
            {
                Action item;
                lock (_jobs)
                {
                    if (_jobs.Count == 0)
                    {
                        _delegateQueuedOrRunning = false;
                        break;
                    }

                    item = _jobs.Dequeue();
                }

                try
                {
                    item?.Invoke();
                }
                catch
                {
                    ThreadPool.UnsafeQueueUserWorkItem(ProcessQueuedItems, state: null);
                    throw;
                }
            }
        }
    }
}
