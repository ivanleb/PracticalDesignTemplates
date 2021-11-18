using DesignPatternsLibrary.Pipeline.Synchronous.Steps;

namespace DesignPatternsLibrary.Pipeline.Synchronous
{
    public class TrivialPipeline : Pipeline<int, string>
    {
        public TrivialPipeline()
        {
            PipelineSteps = input => input
                .Step(new IntToStrStep())
                .Step(new PrintStrStep());
        }
    }
}
