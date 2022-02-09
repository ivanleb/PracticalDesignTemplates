using System;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    internal static class GenericPipelineExtensions
    {
        public static TOutput Step<TInput, TOutput, TInputOuter, TOutputOuter>(
            this TInput inputType, 
            GenericPipeline<TInputOuter, TOutputOuter> pipelineBuilder,
            Func<TInput, TOutput> step)
        {
            var pipelineStep = pipelineBuilder.GenerateStep<TInput, TOutput>();
            pipelineStep.Action = step;
            return default(TOutput);
        }
    }
}
