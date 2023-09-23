using DesignPatternsLibrary.Lock.SimpleLock;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Lock.AcquireLockStrategy
{
    public class WaitingLockStrategy : IAcquireLockStrategy
    {
        private readonly TimeSpan _retryInterval;
        private readonly ICancellationTokenProvider _cancellationTokenProvider;

        public WaitingLockStrategy(TimeSpan retryInterval, ICancellationTokenProvider cancellationTokenProvider)
        {
            _retryInterval = retryInterval;
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public bool TryLock(ILocker locker, Guid objectId, out ILock @lock)
        {
            CancellationToken cancellationToken = _cancellationTokenProvider.GetCancellationToken(objectId);
            for (;;)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    @lock = null;
                    return false;
                }

                if (locker.TryLock(objectId, out @lock))
                    return true;

                Task.Delay(_retryInterval);
            }
        }
    }
}
