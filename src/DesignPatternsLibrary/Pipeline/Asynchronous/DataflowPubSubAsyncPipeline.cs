using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    internal class DataflowPubSubAsyncPipeline<T>
    {
        private readonly BroadcastBlock<IContext<T>> _jobs;

        public DataflowPubSubAsyncPipeline()
        {
            _jobs = new BroadcastBlock<IContext<T>>(job => job);
        }

        public void RegisterHandler<JobType>(Action<JobType> handleAction) where JobType : IContext<T>
        {
            Action<IContext<T>> actionWrapper = job => handleAction((JobType)job);

            var actionBlock = new ActionBlock<IContext<T>>(job => actionWrapper(job));

            _jobs.LinkTo(actionBlock,
                         new DataflowLinkOptions { PropagateCompletion = true }, 
                         predicate: job => job is JobType);
        }

        public async Task Enqueue(IContext<T> job) => await _jobs.SendAsync(job);

        public void Stop() => _jobs.Complete();
    }
}
