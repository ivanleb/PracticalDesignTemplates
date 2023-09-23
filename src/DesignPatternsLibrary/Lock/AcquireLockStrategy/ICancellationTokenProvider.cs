using System;
using System.Threading;

namespace DesignPatternsLibrary.Lock.AcquireLockStrategy
{
    public interface ICancellationTokenProvider 
    {
        CancellationToken GetCancellationToken(Guid objectId);
    }
}
