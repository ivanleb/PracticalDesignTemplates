using System;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.BackgroundWorkerQueue
{
    public class BackgroundWorkerQueue
    {
        private Task _previousTask = Task.FromResult(true);
        private object _locker = new object();

        public Task QueueTask(Action action)
        {
            lock (_locker)
            {
                _previousTask = _previousTask.ContinueWith(
                  t => action(),
                  CancellationToken.None,
                  TaskContinuationOptions.None,
                  TaskScheduler.Default);
                return _previousTask;
            }
        }

        public Task<T> QueueTask<T>(Func<T> work)
        {
            lock (_locker)
            {
                var task = _previousTask.ContinueWith(
                  t => work(),
                  CancellationToken.None,
                  TaskContinuationOptions.None,
                  TaskScheduler.Default);
                _previousTask = task;
                return task;
            }
        }
    }
}
