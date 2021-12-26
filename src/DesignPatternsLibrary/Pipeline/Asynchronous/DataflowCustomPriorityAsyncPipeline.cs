using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public class DataflowCustomPriorityAsyncPipeline
    {
        private ActionBlock<Action> _actionBlock;
        private BlockingCollection<Action> _jobs;

        public DataflowCustomPriorityAsyncPipeline()
        {
            _actionBlock = new ActionBlock<Action>(job => job?.Invoke(),
                                                    new ExecutionDataflowBlockOptions() { BoundedCapacity = 1 });

            _jobs = new BlockingCollection<Action>(GetPriorityQueue());

            Task.Run(async () =>
            {
                foreach (var job in _jobs.GetConsumingEnumerable())
                {
                    await _actionBlock.SendAsync(job);
                }
            });
        }

        private IProducerConsumerCollection<Action> GetPriorityQueue() => new CustomPriorityQueue<Action, int>();
        
        public void Enqueue(Action job, int priority) => _jobs.Add(job);
    }
}
