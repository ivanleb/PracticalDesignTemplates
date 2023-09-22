using System.Collections.Generic;

namespace DesignPatternsLibrary.Lock.TypedLock
{
    public interface ILockResolver
    {
        bool CanAcquireNewLock(ILock newLock, IReadOnlyList<ILock> oldLocks);
    }
}
