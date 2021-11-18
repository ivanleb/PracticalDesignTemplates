using DesignPatternsLibrary.Pipeline.Synchronous.Steps;

namespace DesignPatternsLibrary.Pipeline.Synchronous
{
    public class InnerPipeline : Pipeline<int,int>
    {
        public InnerPipeline()
        {
            PipelineSteps = input => input
                .Step(new MultiplyIntStep(100))
                .Step(new DivideIntByStep(5));
        }
    }
}
