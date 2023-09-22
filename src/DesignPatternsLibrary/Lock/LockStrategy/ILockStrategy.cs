using DesignPatternsLibrary.Lock.SimpleLock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Lock.LockStrategy
{
    public interface ILockStrategy
    {
        bool TryLock(Guid objectId, out ILock @lock);
        void Unlock(Guid lockId);
    }

    public class OneTryLockStrategy : ILockStrategy
    {
        private readonly ILocker _locker = new Locker();

        public bool TryLock(Guid objectId, out ILock @lock)
        {
            return _locker.TryLock(objectId, out @lock);
        }

        public void Unlock(Guid lockId)
        {
            _locker.Unlock(lockId);
        }
    }

    public class RetryingLockStrategy : ILockStrategy
    {
        private readonly ILocker _locker = new Locker();
        private readonly int _retryCount;
        private readonly TimeSpan _retryInterval;

        public RetryingLockStrategy(int retryCount, TimeSpan retryInterval)
        {
            _retryCount = retryCount;
            _retryInterval = retryInterval;
        }

        public bool TryLock(Guid objectId, out ILock @lock)
        {
            for (int i = 0; i < _retryCount; i++)
            {
                if (_locker.TryLock(objectId, out @lock))
                    return true;
                Task.Delay(_retryInterval);
            }
            @lock = null;
            return false;
        }

        public void Unlock(Guid lockId)
        {
            _locker.Unlock(lockId);
        }
    }

    public class WaitingLockStrategy : ILockStrategy
    {
        private readonly ILocker _locker = new Locker();
        private readonly TimeSpan _retryInterval;
        private readonly CancellationToken _cancellationToken;

        public WaitingLockStrategy(TimeSpan retryInterval, CancellationToken cancellationToken)
        {
            _retryInterval = retryInterval;
            _cancellationToken = cancellationToken;
        }

        public bool TryLock(Guid objectId, out ILock @lock)
        {
            for (;;)
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    @lock = null;
                    return false;
                }

                if (_locker.TryLock(objectId, out @lock))
                    return true;

                Task.Delay(_retryInterval);
            }
        }

        public void Unlock(Guid lockId)
        {
            throw new NotImplementedException();
        }
    }
}
