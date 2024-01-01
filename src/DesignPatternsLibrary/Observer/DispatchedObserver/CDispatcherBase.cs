using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace DesignPatternsLibrary.Observer.DispatchedObserver
{
    public abstract class CDispatcherBase<T> : IDispatcher<T>
    {
        protected TimeSpan _timeInterval;
        protected readonly ObservableSubject<T> _observable;
        protected ConcurrentQueue<T> _messageQueue;
        protected bool cancel = false;

        protected CDispatcherBase(ObservableSubject<T> observableSubject, TimeSpan interval)
        {
            _observable = observableSubject ?? throw new ArgumentNullException(nameof(observableSubject));
            _messageQueue = new ConcurrentQueue<T>();
            _timeInterval = interval;
        }

        public abstract void Dispatch();

        public void Dispose()
        {
            _observable.Dispose();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _observable.Subscribe(observer);
        }

        public void Push(T state)
        {
            _messageQueue.Enqueue(state);
        }

        public void StopDispatch()
        {
            cancel = true;
        }

        protected void DoDispatch()
        {
            while (true)
            {
                if (cancel) 
                {
                    OnCompleted();
                    break;
                }

                while (_messageQueue.TryDequeue(out T message))
                {
                    OnNext(message);
                }
                Thread.Sleep(_timeInterval);
            }
        }

        public void OnCompleted()
        {
            _observable.OnCompleted();
        }

        public void OnError(Exception error)
        {
            _observable.OnError(error);
        }

        public void OnNext(T value)
        {
            _observable.OnNext(value);
        }
    }
}
