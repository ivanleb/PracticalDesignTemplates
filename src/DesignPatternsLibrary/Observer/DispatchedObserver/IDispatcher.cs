using System;

namespace DesignPatternsLibrary.Observer.DispatchedObserver
{
    public interface IDispatcher<T> : IObservable<T>, IObserver<T>
    {
        void Dispatch();
        void StopDispatch();
    }
}
