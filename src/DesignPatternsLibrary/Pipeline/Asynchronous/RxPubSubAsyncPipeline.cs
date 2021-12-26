using System;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    internal class RxPubSubAsyncPipeline<T>
    {
        Subject<IContext<T>> _jobs = new Subject<IContext<T>>();
        private IConnectableObservable<IContext<T>> _connectableObservable;

        public RxPubSubAsyncPipeline()
        {
            _connectableObservable = _jobs.ObserveOn(Scheduler.Default).Publish();
            _connectableObservable.Connect();
        }

        public void Enqueue(IContext<T> job) => _jobs.OnNext(job);
        
        public void RegisterHandler<JobType>(Action<JobType> handleAction) where JobType : IContext<T>
            => _connectableObservable.OfType<JobType>().Subscribe(handleAction);        
    }
}
