using DesignPatternsLibrary.Helpers;
using Hangfire;
using System;
using System.Collections.Concurrent;
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
            RxAsyncPipelineUseCase();
            RxAsyncPipelineImproovedUseCase();
            DataflowAsyncPipelineUseCase();
            DataflowImproovedAsyncPipelineUseCase().Wait();
            DataflowPriorityAsyncPipelineUseCase();
            DataflowCustomPriorityAsyncPipelineUseCase();
            DataflowErrorHandlingAsyncPipelineUseCase();
            HangfireUseCase();
            ParallelAsyncPipelineUseCase();
            Console.WriteLine("Finish Pipeline.Asynchronous use case");
        }

        private static void NotThreadSafeAsyncPipelineUseCase()
        {
            NotThreadSafeAsyncPipeline pipeline = new NotThreadSafeAsyncPipeline();
            for (int i = 0; i < 20; i++)
            {
                Action action = () => Console.WriteLine($"NotThreadSafeAsyncPipeline work: {i}");
                pipeline.Enqueue(action);
            }
        }

        private static void SimpleAsyncPipelineUseCase()
        {
            SimpleAsyncPipeline pipeline = new SimpleAsyncPipeline();
            for (int i = 0; i < 20; i++)
            {
                Action action = () => Console.WriteLine($"SimpleAsyncPipeline work: {i}");
                pipeline.Enqueue(action);
            }
        }

        private static void BlockingCollectionAsyncPipelineUseCase()
        {
            BlockingCollectionAsyncPipeline pipeline = new BlockingCollectionAsyncPipeline();
            for (int i = 0; i < 20; i++)
            {
                Action action = () => Console.WriteLine($"BlockingCollectionAsyncPipeline work: {i}");
                pipeline.Enqueue(action);
            }
        }

        private async static void RareActionsAsyncPipelineUseCase()
        {
            ThreadPoolAsyncPipeline pipeline = new ThreadPoolAsyncPipeline();
            for (int i = 0; i < 20; i++)
            {
                await TimeSpan.FromSeconds(3);

                Action action = () =>
                {
                    Console.WriteLine($"RareActionsAsyncPipeline work: {i}");
                };
                pipeline.Enqueue(action);
            }
        }

        private async static void ChannelAsyncPipelineUseCase()
        {
            ChannelAsyncPipeline pipeline = new ChannelAsyncPipeline();
            for (int i = 0; i < 20; i++)
            {
                await TimeSpan.FromSeconds(3);

                Action action = () =>
                {
                    Console.WriteLine($"ChannelAsyncPipeline work: {i}");
                };
                pipeline.Enqueue(action);
            }
            pipeline.Stop();
        }

        private static async void ChannelsMultiThreadsAsyncPipelineUseCase()
        {
            ChannelsMultiThreadsAsyncPipeline pipeline = new ChannelsMultiThreadsAsyncPipeline(threadCount: 10);
            for (int i = 0; i < 20; i++)
            {
                await TimeSpan.FromSeconds(3);

                Action action = () =>
                {
                    Console.WriteLine($"ChannelsMultiThreadsAsyncPipeline work: {i}");
                };
                pipeline.Enqueue(action);
            }
            pipeline.Stop();
        }

        private static async Task ChannelsImproovedAsyncPipelineUseCase()
        {
            ChannelsPubSubAsyncPipeline<string> pipeline = new ChannelsPubSubAsyncPipeline<string>();

            pipeline.RegisterHandler<Context1>(ctx => Console.WriteLine(ctx.Data));
            pipeline.RegisterHandler<Context2>(ctx => Console.WriteLine(ctx.Data + " " + Math.PI));

            await pipeline.Enqueue(new Context1());
            await pipeline.Enqueue(new Context2());
            await pipeline.Enqueue(new Context1());
            await pipeline.Enqueue(new Context1());
            await pipeline.Enqueue(new Context2());
            await pipeline.Enqueue(new Context2());
            pipeline.Stop();
        }

        private async static void RxAsyncPipelineUseCase()
        {
            RxAsyncPipeline pipeline = new RxAsyncPipeline();
            for (int i = 0; i < 20; i++)
            {
                await TimeSpan.FromSeconds(3);

                Action action = () =>
                {
                    Console.WriteLine($"RxAsyncPipeline work: {i}");
                };
                pipeline.Enqueue(action);
            }
        }

        private static void RxAsyncPipelineImproovedUseCase()
        {
            RxPubSubAsyncPipeline<string> pipeline = new RxPubSubAsyncPipeline<string>();
            pipeline.RegisterHandler<Context1>(ctx => Console.WriteLine(ctx.Data));
            pipeline.RegisterHandler<Context2>(ctx => Console.WriteLine(ctx.Data + " " + Math.E));
            pipeline.Enqueue(new Context1());
            pipeline.Enqueue(new Context2());
            pipeline.Enqueue(new Context1());
            pipeline.Enqueue(new Context1());
            pipeline.Enqueue(new Context2());
            pipeline.Enqueue(new Context2());
        }

        private static void DataflowAsyncPipelineUseCase()
        {
            DataflowAsyncPipeline pipeline = new DataflowAsyncPipeline();
            for (int i = 0; i < 10; i++)
            {
                Action action = () =>
                {
                    Console.WriteLine($"DataflowAsyncPipeline work: {i}");
                };
                pipeline.Enqueue(action);
            }
            pipeline.Stop();
        }

        private static async Task DataflowImproovedAsyncPipelineUseCase()
        {
            DataflowPubSubAsyncPipeline<string> pipeline = new DataflowPubSubAsyncPipeline<string>();

            pipeline.RegisterHandler<Context1>(ctx => Console.WriteLine(ctx.Data));
            pipeline.RegisterHandler<Context2>(ctx => Console.WriteLine(ctx.Data + " " + Math.Tau));

            await pipeline.Enqueue(new Context1());
            await pipeline.Enqueue(new Context2());
            await pipeline.Enqueue(new Context1());
            await pipeline.Enqueue(new Context1());
            await pipeline.Enqueue(new Context2());
            await pipeline.Enqueue(new Context2());

            pipeline.Stop();
        }

        private async static void DataflowPriorityAsyncPipelineUseCase()
        {
            DataflowPriorityAsyncPipeline pipeline = new DataflowPriorityAsyncPipeline();
            for (int i = 0; i < 10; i++)
            {
                await TimeSpan.FromSeconds(3);

                int j = i;
                Action action = () =>
                {
                    Console.WriteLine($"DataflowPriorityAsyncPipeline work: {j}");
                };
                Priority priority = (Priority)Enum.ToObject(typeof(Priority), j % 3);
                pipeline.Enqueue(action, priority);
            }
            pipeline.Stop();
        }

        private async static void DataflowCustomPriorityAsyncPipelineUseCase()
        {
            DataflowCustomPriorityAsyncPipeline pipeline = new DataflowCustomPriorityAsyncPipeline();
            for (int i = 0; i < 10; i++)
            {
                await TimeSpan.FromSeconds(3);

                Action action = () =>
                {
                    Console.WriteLine($"DataflowCustomPriorityAsyncPipeline work: {i}");
                };

                pipeline.Enqueue(action, i);
            }
            pipeline.Stop();
        }


        private static void DataflowErrorHandlingAsyncPipelineUseCase()
        {
            DataflowErrorHandlingAsyncPipeline pipeline = new DataflowErrorHandlingAsyncPipeline(new FailedJobQueue<string>());

            pipeline.Enqueue(new Context3());
            pipeline.Enqueue(new Context3());
            pipeline.Enqueue(new Context3());
            pipeline.Enqueue(new Context1());
            pipeline.Enqueue(new Context2());
            pipeline.Enqueue(new Context3());
            pipeline.Stop();
        }

        private static void HangfireUseCase()
        {
            try
            {
                string? jobId = BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget!"));

                string? delayedJobId = BackgroundJob.Schedule(() => Console.WriteLine("Delayed!"), TimeSpan.FromSeconds(20));

                RecurringJob.AddOrUpdate("myrecurringjob", () => Console.WriteLine("Recurring!"), Cron.Minutely);

                BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine("Continuation!"));

            }
            catch (Exception e)
            {
                Console.WriteLine("HangfireUseCase Error: " + e.Message);
            }
        }

        private static void ParallelAsyncPipelineUseCase() 
        {
            ParallelAsyncPipeline<string> parallelAsyncPipeline = new ParallelAsyncPipeline<string>();
            parallelAsyncPipeline.Enqueue("first");
            parallelAsyncPipeline.Enqueue("second");
            parallelAsyncPipeline.Enqueue("third");
            parallelAsyncPipeline.Enqueue("fourth");
            parallelAsyncPipeline.Enqueue("fifth");
            parallelAsyncPipeline.OnStart(2, item => 
                    {
                        Console.WriteLine(item);
                        Thread.Sleep(TimeSpan.FromSeconds(3));
                        if(item == "third")
                            parallelAsyncPipeline.Enqueue("twenty third");
                    });
        }
    }
}
