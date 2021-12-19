using System;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public static class UseCase
    {
        public static void Run()
        {
            Console.WriteLine("Start Pipeline.Asynchronous use case");
            NotThreadSafeAsyncPipelineUseCase();
            SimpleAsyncPipelineUseCase();
            BlockingCollectionAsyncPipelineUseCase();
            RareActionsAsyncPipelineUseCase();
            ChannelAsyncPipelineUseCase();
            ChannelsMultiThreadsAsyncPipelineUseCase();
            ChannelsImproovedAsyncPipelineUseCase().Wait();
            Console.WriteLine("Finish Pipeline.Asynchronous use case");
        }

        private static void NotThreadSafeAsyncPipelineUseCase()
        {
            NotThreadSafeAsyncPipeline notThreadSafeAsyncPipeline = new NotThreadSafeAsyncPipeline();
            for (int i = 0; i < 20; i++)
            {
                Action action = () => Console.WriteLine($"NotThreadSafeAsyncPipeline work: {i}");
                notThreadSafeAsyncPipeline.Enqueue(action);
            }
        }

        private static void SimpleAsyncPipelineUseCase()
        {
            SimpleAsyncPipeline simpleAsyncPipeline = new SimpleAsyncPipeline();
            for (int i = 0; i < 20; i++)
            {
                Action action = () => Console.WriteLine($"SimpleAsyncPipeline work: {i}");
                simpleAsyncPipeline.Enqueue(action);
            }
        }

        private static void BlockingCollectionAsyncPipelineUseCase()
        {
            BlockingCollectionAsyncPipeline blockingCollectionAsyncPipeline = new BlockingCollectionAsyncPipeline();
            for (int i = 0; i < 20; i++)
            {
                Action action = () => Console.WriteLine($"BlockingCollectionAsyncPipeline work: {i}");
                blockingCollectionAsyncPipeline.Enqueue(action);
            }
        }

        private static void RareActionsAsyncPipelineUseCase()
        {
            RareActionsAsyncPipeline rareActionsAsyncPipeline = new RareActionsAsyncPipeline();
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(10));
                Action action = () =>
                {
                    Console.WriteLine($"RareActionsAsyncPipeline work: {i}");
                };
                rareActionsAsyncPipeline.Enqueue(action);
            }
        }

        private static void ChannelAsyncPipelineUseCase()
        {
            ChannelAsyncPipeline channelAsyncPipeline = new ChannelAsyncPipeline();
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(10));
                Action action = () =>
                {
                    Console.WriteLine($"ChannelAsyncPipeline work: {i}");
                };
                channelAsyncPipeline.Enqueue(action);
            }
        }
        
        private static void ChannelsMultiThreadsAsyncPipelineUseCase()
        {
            ChannelsMultiThreadsAsyncPipeline chanelAsyncPipeline = new ChannelsMultiThreadsAsyncPipeline(threadCount: 10);
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(10));
                Action action = () =>
                {
                    Console.WriteLine($"ChannelsMultiThreadsAsyncPipeline work: {i}");
                };
                chanelAsyncPipeline.Enqueue(action);
            }
        }

        private static async Task ChannelsImproovedAsyncPipelineUseCase() 
        {

            ChannelsImproovedAsyncPipeline<string> channelsImproovedAsyncPipeline = new ChannelsImproovedAsyncPipeline<string>();

            channelsImproovedAsyncPipeline.RegisterHandler<Job1>(job => Console.WriteLine(job.JobData));
            channelsImproovedAsyncPipeline.RegisterHandler<Job2>(job => Console.WriteLine(job.JobData + " " + Math.PI));

            await channelsImproovedAsyncPipeline.Enqueue(new Job1());
            await channelsImproovedAsyncPipeline.Enqueue(new Job2());
            await channelsImproovedAsyncPipeline.Enqueue(new Job1());
            await channelsImproovedAsyncPipeline.Enqueue(new Job1());
            await channelsImproovedAsyncPipeline.Enqueue(new Job2());
            await channelsImproovedAsyncPipeline.Enqueue(new Job2());
        }
    }

    internal class Job1 : IJob<string>
    {
        public string JobData => "Job1";
    }
    internal class Job2 : IJob<string>
    {
        public string JobData => "Job2";
    }
}
