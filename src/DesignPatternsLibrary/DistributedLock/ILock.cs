using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.DistributedLock
{
    public interface ILock
    {
        string Name { get; }
        ISynchronizationHandle? TryAcquire(TimeSpan timeout = default, CancellationToken cancellationToken = default);
        ISynchronizationHandle Acquire(TimeSpan? timeout = null, CancellationToken cancellationToken = default);
    }
}
