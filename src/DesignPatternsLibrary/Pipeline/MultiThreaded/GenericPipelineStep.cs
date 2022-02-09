using System;
using System.Collections.Concurrent;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    internal class GenericPipelineStep<TIn, TOut> : IPipelineStep<TIn>
    {
        public BlockingCollection<TIn> Buffer { get; set; } = new BlockingCollection<TIn>();
        public Func<TIn, TOut> Action { get; set; }
    }
}
