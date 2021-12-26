using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public class RxAsyncPipeline
    {
        Subject<Action> _jobs = new Subject<Action>();

        public RxAsyncPipeline() 
            => _jobs.ObserveOn(Scheduler.Default).Subscribe(job => job?.Invoke());

        public void Enqueue(Action job) 
            => _jobs.OnNext(job);
    }
}
