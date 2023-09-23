using DesignPatternsLibrary.Lock.SimpleLock;
using System;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Lock.AcquireLockStrategy
{
    public class RetryingLockStrategy : IAcquireLockStrategy
    {
        private readonly int _retryCount;
        private readonly TimeSpan _retryInterval;

        public RetryingLockStrategy(int retryCount, TimeSpan retryInterval)
        {
            _retryCount = retryCount;
            _retryInterval = retryInterval;
        }

        public bool TryLock(ILocker locker, Guid objectId, out ILock? @lock)
        {
            for (int i = 0; i < _retryCount; i++)
            {
                if (locker.TryLock(objectId, out @lock))
                    return true;

                Task.Delay(_retryInterval);
            }
            @lock = null;
            return false;
        }
    }
}
