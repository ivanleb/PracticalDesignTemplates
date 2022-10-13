using System;
using System.Threading;

namespace DesignPatternsLibrary.DistributedLock
{
    public interface ISynchronizationHandle : IDisposable
    {
        CancellationToken HandleLostToken { get; }
    }
}