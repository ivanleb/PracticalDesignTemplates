using DesignPatternsLibrary.Pipeline.Synchronous.Steps;
using System;

namespace DesignPatternsLibrary.Pipeline.Synchronous
{
    public static class UseCase
    {
        public static void Run()
        {
            Console.WriteLine("Start Pipeline.Synchronous use case");
            SimpleUseCase();
            TrivialPipelineUseCase();
            CompoundPipelineUseCase();
            PipelineWithOptionalStepUseCase();
            Console.WriteLine("Finish Pipeline.Synchronous use case");
        }

        private static void SimpleUseCase()
        {
            Console.WriteLine("Simple use case");
            int input = 5;
            string result = input
                              .Step(new IntToStrStep())
                              .Step(new PrintStrStep());
        }

        private static void TrivialPipelineUseCase()
        {
            Console.WriteLine("TrivialPipeline use case");
            TrivialPipeline trivialPipeline = new TrivialPipeline();
            trivialPipeline.PipelineSteps?.Invoke(5);
        }

        private static void CompoundPipelineUseCase()
        {
            Console.WriteLine("CompoundPipeline use case");
            CompoundPipeline compoundPipeline = new CompoundPipeline();
            compoundPipeline.PipelineSteps?.Invoke(5);
        }

        private static void PipelineWithOptionalStepUseCase()
        {
            Console.WriteLine("PipelineWithOptionalStep use case");
            PipelineWithOptionalStep pipelineWithOptionalStep = new PipelineWithOptionalStep();
            pipelineWithOptionalStep.PipelineSteps?.Invoke(20);
        }
    }
}
