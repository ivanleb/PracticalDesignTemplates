using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternsLibrary.Lock.TypedLock
{
    public class Locker : ILocker
    {
        private readonly HashSet<ILock> _locks = new HashSet<ILock>();
        private readonly ILockResolver _lockResolver;

        public Locker(ILockResolver lockResolver)
        {
            _lockResolver = lockResolver;
        }

        public bool TryLock(Guid objectId, LockType lockType, out ILock? @lock)
        {
            Lock newLock = new Lock(Guid.NewGuid(), lockType, objectId);

            IReadOnlyList<ILock> existingLocks = _locks.Where(l => l.ObjectId == objectId).ToArray();
            if (existingLocks.Any())
            {
                if (_lockResolver.CanAcquireNewLock(newLock, existingLocks))
                {
                    @lock = newLock;
                    return true;
                }

                @lock = null;
                return false;
            }

            @lock = newLock;
            return _locks.Add(@lock);
        }

        public void Unlock(Guid lockId)
        {
            _locks.RemoveWhere(l => l.Id == lockId);
        }
    }
}
