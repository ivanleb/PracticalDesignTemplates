using System;

namespace DesignPatternsLibrary.Lock.AcquireLockStrategy
{
    public class LockerStrategyDecorator : ILocker
    {
        private readonly IAcquireLockStrategy _acquireLockStrategy;
        private readonly ILocker _locker;

        public LockerStrategyDecorator(ILocker locker, IAcquireLockStrategy acquireLockStrategy)
        {
            _locker = locker;
            _acquireLockStrategy = acquireLockStrategy;
        }

        public bool TryLock(Guid objectId, out ILock @lock)
        {
            return _acquireLockStrategy.TryLock(_locker, objectId, out @lock);
        }

        public void Unlock(Guid lockId)
        {
            _locker.Unlock(lockId);
        }
    }
}
