using System;
using System.Threading.Tasks.Dataflow;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    internal class DataflowBroadcastAsyncPipeline
    {
        private readonly BroadcastBlock<Action> _jobs;

        public DataflowBroadcastAsyncPipeline()
        {
            _jobs = new BroadcastBlock<Action>(job => job);

            ActionBlock<Action> action1 = new ActionBlock<Action>(job => job?.Invoke());
            ActionBlock<Action> action2 = new ActionBlock<Action>(job => job?.Invoke());

            _jobs.LinkTo(action1, new DataflowLinkOptions { PropagateCompletion = true });
            _jobs.LinkTo(action2, new DataflowLinkOptions { PropagateCompletion = true });
        }

        public void Enqueue(Action job) => _jobs.Post(job);

        public void Stop() => _jobs.Complete();
    }
}
