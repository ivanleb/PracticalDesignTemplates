using System;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Observer
{
    public interface IAsyncObserver<in T>
    {
        ValueTask OnNextAsync(T value);
        ValueTask OnErrorAsync(Exception error);
        ValueTask OnCompletedAsync();
    }
}
