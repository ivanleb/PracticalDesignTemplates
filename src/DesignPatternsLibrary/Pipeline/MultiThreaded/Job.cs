using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    public class Job<TInput, TOutput> : IJob<TInput, TOutput>
    {
        public Job(TInput input, TaskCompletionSource<TOutput> tcs)
        {
            Input = input;
            TaskCompletionSource = tcs;
        }
        public TInput Input { get; set; }
        public TaskCompletionSource<TOutput> TaskCompletionSource { get; set; }
    }

    public class DefaultJobProvider : IJobProvider
    {
        public IJob<TInput, TOutput> CreateJob<TInput, TOutput>(TInput input, TaskCompletionSource<TOutput> tcs)
        {
            return new Job<TInput, TOutput>(input, tcs);
        }
    }
}
