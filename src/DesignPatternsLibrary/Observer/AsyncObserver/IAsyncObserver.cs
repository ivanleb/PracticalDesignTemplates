using System;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Observer.AsyncObserver
{
    public interface IAsyncObserver<in T>
    {
        ValueTask OnNextAsync(T value);
        ValueTask OnErrorAsync(Exception error);
        ValueTask OnCompletedAsync();
    }
}
