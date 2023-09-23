using DesignPatternsLibrary.Lock.SimpleLock;
using System;

namespace DesignPatternsLibrary.Lock.AcquireLockStrategy
{
    public class OneTryLockStrategy : IAcquireLockStrategy
    {

        public bool TryLock(ILocker locker, Guid objectId, out ILock @lock)
        {
            return locker.TryLock(objectId, out @lock);
        }
    }
}
