using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    public interface IJob<TInput, TOutput>
    {
        TInput Input { get; set; }
        TaskCompletionSource<TOutput> TaskCompletionSource { get; set; }
    }
}