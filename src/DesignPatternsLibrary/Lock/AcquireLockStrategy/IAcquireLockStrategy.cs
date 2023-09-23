using System;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Lock.AcquireLockStrategy
{
    public interface IAcquireLockStrategy
    {
        bool TryLock(ILocker locker, Guid objectId, out ILock? @lock);
    }
}
