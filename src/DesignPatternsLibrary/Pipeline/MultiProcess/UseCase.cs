using DesignPatternsLibrary.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.MultiProcess
{
    public static class UseCase
    {
        public static void Run() 
        {
            Console.WriteLine("Start Pipeline.MultiProcess use case");
            SimpleMultiprocessPipelineUseCase().Wait();
            Console.WriteLine("Finish Pipeline.MultiProcess use case");
        }

        private static async Task SimpleMultiprocessPipelineUseCase() 
        {
            SimpleMultiprocessPipeline pipeline = new SimpleMultiprocessPipeline();
            pipeline.AddStep("ipconfig", "");
            pipeline.AddStep("ping", "127.0.0.1");
            await pipeline.Execute();

            foreach (string result in pipeline.JobTasks.Select(t => t.Results).SelectMany(r => r.StandardOutput))
            {
                Console.WriteLine(result);
            }
        }
    }
}
