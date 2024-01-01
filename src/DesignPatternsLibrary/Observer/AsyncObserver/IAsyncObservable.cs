using System;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Observer.AsyncObserver
{
    public interface IAsyncObservable<out T>
    {
        ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer);
    }
}
