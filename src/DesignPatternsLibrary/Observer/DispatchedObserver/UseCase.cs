using System;
using System.Threading;

namespace DesignPatternsLibrary.Observer.DispatchedObserver
{
    public static class UseCase
    {
        public static void Run()
        {
            DispatcherAggregator<string> dispatchAggregator = new DispatcherAggregator<string>();
            ProcessingMessageDispatcher<string> processingMessageDispatcher1 = ProcessingMessageDispatcher<string>.Create(TimeSpan.FromSeconds(1));
            ProcessingMessageDispatcher<string> processingMessageDispatcher2 = ProcessingMessageDispatcher<string>.Create(TimeSpan.FromSeconds(2));
            dispatchAggregator.AddDispatcher(processingMessageDispatcher1);
            dispatchAggregator.AddDispatcher(processingMessageDispatcher2);
            Thread thread = new Thread(new ThreadStart(() => {
                MessageObserver<string> messageObserver = new MessageObserver<string>();
                dispatchAggregator.Subscribe(messageObserver);
                dispatchAggregator.OnNext($"message 1 {DateTime.Now}");
                Thread.Sleep(TimeSpan.FromSeconds(2));

                processingMessageDispatcher1.Dispatch();
                dispatchAggregator.OnNext($"message 2 {DateTime.Now}");
                Thread.Sleep(TimeSpan.FromSeconds(2));

                processingMessageDispatcher2.Dispatch();
                dispatchAggregator.OnNext($"message 3 {DateTime.Now}");
                Thread.Sleep(TimeSpan.FromSeconds(2));

                dispatchAggregator.StopDispatch();
                Thread.Sleep(TimeSpan.FromSeconds(2));

                dispatchAggregator.OnNext($"message 4 {DateTime.Now}");
            }));
            thread.Start();
            thread.Join();
        }
    }
}
