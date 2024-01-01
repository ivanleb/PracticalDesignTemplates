using DesignPatternsLibrary.Helpers;
using System;
using System.Linq;

namespace DesignPatternsLibrary.Observer.DispatchedObserver
{
    public class ObservableSubject<T> : IObservable<T>, IObserver<T>
    {
        private readonly ConcurrentHashSet<IObserver<T>> _observers;

        public ObservableSubject()
        {
            _observers = new ConcurrentHashSet<IObserver<T>>();
        }

        public bool IsEmpty => _observers.Count == 0;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (_observers.Add(observer))
                return new Unsubscriber<T>(_observers.ToList(), observer);
            throw new Exception("Unable to subscribe.");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;
            foreach (var obsr in _observers)
                ((IObserver<T>)obsr).OnCompleted();
            _observers.Clear();
        }

        public void Dispose() => Dispose(true);
        
        public void OnCompleted()
        {
            foreach (var obsr in _observers)
                ((IObserver<T>)obsr).OnCompleted();
        }

        public void OnError(Exception error)
        {
            foreach (var obsr in _observers)
                ((IObserver<T>)obsr).OnError(error);
        }

        public void OnNext(T value)
        {
            foreach (var obsr in _observers)
                ((IObserver<T>)obsr).OnNext(value);
        }
    }
}
