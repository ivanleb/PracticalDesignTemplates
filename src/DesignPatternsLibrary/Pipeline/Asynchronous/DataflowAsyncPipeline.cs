using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    internal class DataflowAsyncPipeline
    {
        private readonly ActionBlock<Action> _jobs;

        public DataflowAsyncPipeline()
        {
            var options = new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = 2 };
            _jobs = new ActionBlock<Action>(job => job?.Invoke(), options);
        }

        public void Enqueue(Action job) => _jobs.Post(job);

        public void Stop() => _jobs.Complete();

        public Task CompleteTask() => _jobs.Completion;
    }
}
