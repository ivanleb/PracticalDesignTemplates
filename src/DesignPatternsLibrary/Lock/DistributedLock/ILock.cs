using System;
using System.Threading;

namespace DesignPatternsLibrary.DistributedLock
{
    public interface ILock
    {
        string Name { get; }
        ISynchronizationHandle? TryAcquire(TimeSpan timeout = default, CancellationToken cancellationToken = default);
        ISynchronizationHandle Acquire(TimeSpan? timeout = null, CancellationToken cancellationToken = default);
    }
}
