using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace DesignPatternsLibrary.Pipeline.MultiThreaded
{
    public static class UseCase
    {
        public static void Run()
        {
            Console.WriteLine("Start Pipeline.MultiThreaded use case");
            NonGenericUseCase();
            GenericPipelineUseCase();
            NonGenericPipelineWithAsyncUseCase();
            SimpleDataflowPipelineUseCase();
            DataflowPipelineWithStepOptionsUseCase();
            DataflowPipelineWithAwaitUseCase();
            Task.Delay(4000).Wait();
            Console.WriteLine("Finish Pipeline.MultiThreaded use case");
        }

        private static void NonGenericUseCase()
        {
            var builder = new NonGenericPipelineBuilder();

            builder.AddStep(input => FindMostCommon(input as string));
            builder.AddStep(input => (input as string).Length);
            builder.AddStep(input => ((int)input) % 2 == 1);

            var pipeline = builder.GetPipeline();

            pipeline.Finished += res => Console.WriteLine(res);
            pipeline.Execute("The pipeline pattern is the best pattern");
        }

        private static void GenericPipelineUseCase()
        {
            var pipeline = new GenericPipeline<string, bool>((inputFirst, builder) =>
                               inputFirst
                                  .Step(builder, input => FindMostCommon(input))
                                  .Step(builder, input => input.Length)
                                  .Step(builder, input => input % 2 == 1));

            pipeline.Finished += res => Console.WriteLine(res);
            pipeline.Execute("The pipeline pattern is the best pattern");
        }

        private static async void NonGenericPipelineWithAsyncUseCase()
        {
            var builder = new NonGenericPipelineWithAwait<bool>();

            builder.AddStep(input => FindMostCommon(input as string), 2, 2);
            builder.AddStep(input => (input as string).Length, 2, 2);
            builder.AddStep(input => ((int)input) % 2 == 1, 2, 2);

            IAwaitablePipeline<bool> pipeline = builder.GetPipeline();

            Task<bool> resultTask = pipeline.Execute("The pipeline pattern is the best pattern");
            bool result = await resultTask;
            Console.WriteLine(result);
        }

        private static void SimpleDataflowPipelineUseCase()
        {
            var pipeline = SimpleDataflowPipeline.CreatePipeline(resultCallback: res => Console.WriteLine(res));
            pipeline.Post("The pipeline pattern is the best pattern");
        }

        private static async void DataflowPipelineWithStepOptionsUseCase()
        {
            var pipeline = SimpleDataflowPipeline.CreatePipelineWithStepOptions(resultCallback: res => Console.WriteLine(res));
            await pipeline.SendAsync("The pipeline pattern is the best pattern");
        }

        private static async void DataflowPipelineWithAwaitUseCase()
        {
            DataflowPipeline<string, bool> pipeline = new DataflowPipeline<string, bool>(
                (inputFirst, builder) =>
                                    inputFirst
                                        .AddStep(builder, input => FindMostCommon(input), maxDegreeOfParallelism: 2, maxCapacity: 2)
                                        .AddStep(builder, input => input.Length, maxDegreeOfParallelism: 2, maxCapacity: 2)
                                        .AddStep(builder, input => input % 2 == 1, maxDegreeOfParallelism: 2, maxCapacity: 2));

            bool result = await pipeline.Execute("The pipeline pattern is the best pattern");
            Console.WriteLine(result);
        }

        static string FindMostCommon(string input)
        {
            string[] splitted = input.Split(' ');
            Dictionary<string, int> counters = new Dictionary<string, int>();
            for (int i = 0; i < splitted.Length; i++)
            {
                if (counters.ContainsKey(splitted[i]))
                    counters[splitted[i]]++;
                else
                    counters[splitted[i]] = 1; ;
            }
            string mostCommon = counters.MaxBy(p => p.Value).Key;
            return mostCommon;
        }
    }
}
