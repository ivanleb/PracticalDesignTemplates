using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternsLibrary.Lock.SimpleLock
{
    public class Locker : ILocker
    {
        private readonly HashSet<ILock> _locks = new HashSet<ILock>();
        public bool TryLock(Guid objectId, out ILock @lock)
        {
            ILock? existingLock = _locks.FirstOrDefault(l => l.ObjectId == objectId);
            if (existingLock != null) 
            {
                @lock = existingLock;
                return false;
            }

            @lock = new Lock(this, Guid.NewGuid(), objectId);            
            return _locks.Add(@lock);
        }

        public void Unlock(Guid lockId)
        {
            _locks.RemoveWhere(l => l.Id == lockId);
        }
    }
}
