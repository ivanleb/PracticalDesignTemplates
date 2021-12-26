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
            ThreadPoolAsyncPipeline rareActionsAsyncPipeline = new ThreadPoolAsyncPipeline();
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
            ChannelsPubSubAsyncPipeline<string> channelsImproovedAsyncPipeline = new ChannelsPubSubAsyncPipeline<string>();

            channelsImproovedAsyncPipeline.RegisterHandler<Context1>(ctx => Console.WriteLine(ctx.Data));
            channelsImproovedAsyncPipeline.RegisterHandler<Context2>(ctx => Console.WriteLine(ctx.Data + " " + Math.PI));

            await channelsImproovedAsyncPipeline.Enqueue(new Context1());
            await channelsImproovedAsyncPipeline.Enqueue(new Context2());
            await channelsImproovedAsyncPipeline.Enqueue(new Context1());
            await channelsImproovedAsyncPipeline.Enqueue(new Context1());
            await channelsImproovedAsyncPipeline.Enqueue(new Context2());
            await channelsImproovedAsyncPipeline.Enqueue(new Context2());
        }

        private static void RxAsyncPipelineUseCase() 
        {
            RxAsyncPipeline rxAsyncPipeline = new RxAsyncPipeline();
            for (int i = 0; i < 20; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(10));
                Action action = () =>
                {
                    Console.WriteLine($"RxAsyncPipeline work: {i}");
                };
                rxAsyncPipeline.Enqueue(action); 
            }
        }

        private static void RxAsyncPipelineImproovedUseCase()
        {
            RxPubSubAsyncPipeline<string> rxAsyncPipeline = new RxPubSubAsyncPipeline<string>();
            rxAsyncPipeline.RegisterHandler<Context1>(ctx => Console.WriteLine(ctx.Data));
            rxAsyncPipeline.RegisterHandler<Context2>(ctx => Console.WriteLine(ctx.Data + " " + Math.E));
            rxAsyncPipeline.Enqueue(new Context1());
            rxAsyncPipeline.Enqueue(new Context2());
            rxAsyncPipeline.Enqueue(new Context1());
            rxAsyncPipeline.Enqueue(new Context1());
            rxAsyncPipeline.Enqueue(new Context2());
            rxAsyncPipeline.Enqueue(new Context2()); 
        }

        private static void DataflowAsyncPipelineUseCase()
        {
            DataflowAsyncPipeline dataflowAsyncPipeline = new DataflowAsyncPipeline();
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
                Action action = () =>
                {
                    Console.WriteLine($"DataflowAsyncPipeline work: {i}");
                };
                dataflowAsyncPipeline.Enqueue(action);
            }
        }

        private static async Task DataflowImproovedAsyncPipelineUseCase()
        {
            DataflowPubSubAsyncPipeline<string> dataflowImproovedAsyncPipeline = new DataflowPubSubAsyncPipeline<string>();

            dataflowImproovedAsyncPipeline.RegisterHandler<Context1>(ctx => Console.WriteLine(ctx.Data));
            dataflowImproovedAsyncPipeline.RegisterHandler<Context2>(ctx => Console.WriteLine(ctx.Data + " " + Math.Tau));

            await dataflowImproovedAsyncPipeline.Enqueue(new Context1());
            await dataflowImproovedAsyncPipeline.Enqueue(new Context2());
            await dataflowImproovedAsyncPipeline.Enqueue(new Context1());
            await dataflowImproovedAsyncPipeline.Enqueue(new Context1());
            await dataflowImproovedAsyncPipeline.Enqueue(new Context2());
            await dataflowImproovedAsyncPipeline.Enqueue(new Context2());
        }
        
        private static void DataflowPriorityAsyncPipelineUseCase()
        {
            DataflowPriorityAsyncPipeline dataflowAsyncPipeline = new DataflowPriorityAsyncPipeline();
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
                Action action = () =>
                {
                    Console.WriteLine($"DataflowAsyncPipeline work: {i}");
                };
                Priority priority = (Priority)Enum.ToObject(typeof(Priority), i % 3);
                dataflowAsyncPipeline.Enqueue(action, priority);
            }
        }

        private static void DataflowCustomPriorityAsyncPipelineUseCase()
        {
            ICustomPriorityQueue<Action> priprityQueue = null;
            DataflowCustomPriorityAsyncPipeline dataflowAsyncPipeline = new DataflowCustomPriorityAsyncPipeline(priprityQueue);
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(3));
                Action action = () =>
                {
                    Console.WriteLine($"DataflowAsyncPipeline work: {i}");
                };
                
                dataflowAsyncPipeline.Enqueue(action, i);
            }
        }
    }
}
