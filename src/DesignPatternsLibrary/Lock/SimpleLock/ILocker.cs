using System;

namespace DesignPatternsLibrary.Lock.SimpleLock
{
    public interface ILocker
    {
        bool TryLock(Guid objectId, out ILock @lock);
        void Unlock(Guid lockId);
    }
}