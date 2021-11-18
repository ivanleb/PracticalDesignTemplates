using DesignPatternsLibrary.Pipeline.Synchronous.Steps;

namespace DesignPatternsLibrary.Pipeline.Synchronous
{
    public class CompoundPipeline : Pipeline<int, string>
    {
        public CompoundPipeline()
        {
            PipelineSteps = input => input
                .Step(new InitialStep())
                .Step(new InnerPipeline())
                .Step(new IntToStrStep())
                .Step(new PrintStrStep());
        }
    }
}
