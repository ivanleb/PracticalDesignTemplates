using System.Collections.Concurrent;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    internal interface IPipelineStep<TIn>
    {
        BlockingCollection<TIn> Buffer { get; }
    }
}
