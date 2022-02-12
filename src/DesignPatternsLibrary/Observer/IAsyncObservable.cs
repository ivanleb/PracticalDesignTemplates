using System;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Observer
{
    public interface IAsyncObservable<out T>
    {
        ValueTask<IAsyncDisposable> SubscribeAsync(IAsyncObserver<T> observer);
    }
}
