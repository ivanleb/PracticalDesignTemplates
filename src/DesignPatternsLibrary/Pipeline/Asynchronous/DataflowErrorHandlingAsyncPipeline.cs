using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public class DataflowErrorHandlingAsyncPipeline : IJobQueue<IContext<string>>
    {
        private ActionBlock<IContext<string>> _jobs;

        public DataflowErrorHandlingAsyncPipeline(IJobQueue<IContext<string>> poisonQueue)
        {
            var policy =
                Policy.Handle<Exception>()
                .Retry(3);

            _jobs = new ActionBlock<IContext<string>>((job) =>
            {
                try
                {
                    policy.Execute(() =>
                    {
                        int number = int.Parse(job.Data);
                        Console.WriteLine("Normal: " + job);
                    });
                }
                catch (Exception)
                {
                    poisonQueue.Enqueue(job);
                }
            });
        }

        public void Enqueue(IContext<string> job) => _jobs.Post(job);

        public void Stop() => _jobs.Complete();
    }
}
