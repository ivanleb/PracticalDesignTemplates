using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    internal class NotThreadSafeAsyncPipeline
    {
        private readonly List<Action> _jobs = new List<Action>();

        public NotThreadSafeAsyncPipeline()
        {
            Task.Run(() => { OnStart(); });
        }

        public void Enqueue(Action job) => _jobs.Add(job);
        
        private void OnStart()
        {
            while (true)
            {
                if (_jobs.Count > 0)
                {
                    Action job = _jobs.First();
                    _jobs.RemoveAt(0);
                    job?.Invoke();
                }
            }
        }
    }
}
