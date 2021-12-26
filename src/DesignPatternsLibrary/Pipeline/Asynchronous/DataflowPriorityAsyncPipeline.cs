using System;
using System.Threading.Tasks.Dataflow;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public class DataflowPriorityAsyncPipeline
    {
        private readonly PriorityBufferBlock<Action> _priorityBufferBlock;
        private readonly ActionBlock<Action> _jobs;

        public DataflowPriorityAsyncPipeline()
        {
            _priorityBufferBlock = new PriorityBufferBlock<Action>();
            _jobs = new ActionBlock<Action>(job => job?.Invoke());

            _priorityBufferBlock.LinkTo(_jobs, new DataflowLinkOptions { PropagateCompletion = true });
        }

        public void Enqueue(Action job, Priority priority) => _priorityBufferBlock.Post(job, priority);

        public void Stop() => _jobs.Complete();
    }
}
