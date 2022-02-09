using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    public interface IAwaitablePipeline<TOut>
    {
        Task<TOut> Execute(object input);
    }
}
