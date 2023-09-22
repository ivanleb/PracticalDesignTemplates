using System;

namespace DesignPatternsLibrary.Lock.TypedLock
{
    public interface ILocker
    {
        bool TryLock(Guid objectId, LockType lockType, out ILock? @lock);
        void Unlock(Guid lockId);
    }
}
