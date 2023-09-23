using System;

namespace DesignPatternsLibrary.Lock.AcquireLockStrategy
{
    public interface ILocker
    {
        bool TryLock(Guid objectId, out ILock @lock);
        void Unlock(Guid lockId);
    }
}
