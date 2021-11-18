namespace DesignPatternsLibrary.Pipeline.Synchronous
{
    public static class PipelineStepExtensions
    {
        public static TOutput Step<TInput, TOutput>(this TInput input, IPipelineStep<TInput, TOutput> step)
        {
            return step.Process(input);
        }
    }
}
