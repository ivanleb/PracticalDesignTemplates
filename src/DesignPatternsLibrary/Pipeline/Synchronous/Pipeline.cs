using System;

namespace DesignPatternsLibrary.Pipeline.Synchronous
{
    public abstract class Pipeline<TInput, TOutput> : IPipelineStep<TInput, TOutput>
    {
        public Func<TInput, TOutput> PipelineSteps { get; protected set; }

        public TOutput Process(TInput input)
        {
            return PipelineSteps(input);
        }
    }
}
