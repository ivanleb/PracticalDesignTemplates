using System;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Observer.AsyncObserver
{
    public class AsyncObserverEmpty<T> : AsyncObserverBase<T>
    {
        protected override ValueTask OnCompletedAsyncCore() => ValueTask.CompletedTask;
        protected override ValueTask OnErrorAsyncCore(Exception error) => ValueTask.CompletedTask;
        protected override ValueTask OnNextAsyncCore(T value) => ValueTask.CompletedTask;
    }
}
