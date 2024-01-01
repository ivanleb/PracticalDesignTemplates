using DesignPatternsLibrary.Disposable;
using System;
using System.Collections.Generic;

namespace DesignPatternsLibrary.Observer.DispatchedObserver
{
    public class DispatcherAggregator<T> : IDispatcher<T>
    {
        private readonly IList<IDispatcher<T>> _dispatchers = new List<IDispatcher<T>>();
        private readonly IList<WeakReference<IObserver<T>>> _observers = new List<WeakReference<IObserver<T>>>();

        public void AddDispatcher(IDispatcher<T> dispatcher)
        {
            _dispatchers.Add(dispatcher);
            foreach (var observer in _observers)
            {
                if (observer.TryGetTarget(out IObserver<T> obsrv))
                    dispatcher.Subscribe(obsrv);                
            }
        }

        public void RemoveDispatcher(IDispatcher<T> dispatcher)
        {
            _dispatchers.Remove(dispatcher);
        }

        public void StopDispatch()
        {
            foreach (var dispatcher in _dispatchers)
                dispatcher.StopDispatch();
        }

        public void Dispatch()
        {
            foreach (var dispatcher in _dispatchers)
                dispatcher.Dispatch();
        }

        public IDisposable? Subscribe(IObserver<T> observer)
        {
            AutoDisposeStack disposableStack = new AutoDisposeStack(allowNullObjects: true, throwItemDisposeException: false);
            foreach (var dispatcher in _dispatchers)
                disposableStack.Add(dispatcher.Subscribe(observer));
            _observers.Add(new WeakReference<IObserver<T>>(observer));
            return disposableStack;
        }

        public void OnCompleted()
        {
            foreach (var dispatcher in _dispatchers)
                dispatcher.OnCompleted();
        }

        public void OnError(Exception error)
        {
            foreach (var dispatcher in _dispatchers)
                dispatcher.OnError(error);
        }

        public void OnNext(T value)
        {
            foreach (var dispatcher in _dispatchers)
                dispatcher.OnNext(value);
        }
    }
}
