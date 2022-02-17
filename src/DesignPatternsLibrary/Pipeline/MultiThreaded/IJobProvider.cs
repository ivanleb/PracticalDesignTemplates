using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    public interface IJobProvider
    {
        IJob<TInput, TOutput> CreateJob<TInput, TOutput>(TInput input, TaskCompletionSource<TOutput> tcs);
    }
}