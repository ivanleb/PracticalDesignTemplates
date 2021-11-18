using DesignPatternsLibrary.Pipeline.Synchronous.Steps;

namespace DesignPatternsLibrary.Pipeline.Synchronous
{
    public class PipelineWithOptionalStep : Pipeline<int, string>
    {
        public PipelineWithOptionalStep()
        {
            PipelineSteps = input => input
                .Step(new InitialStep())
                .Step(new OptionalStep<int, int>(i => i > 5, new DivideIntByStep(5)))
                .Step(new IntToStrStep())
                .Step(new PrintStrStep());
        }
    }
}
