namespace DesignPatternsLibrary.Pipeline.Synchronous
{
    public interface IPipelineStep<TInput, TOutput>
    {
        TOutput Process(TInput input);
    }
}
