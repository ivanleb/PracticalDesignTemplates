﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Lock.SimpleLock
{
    public class Lock : ILock
    {
        private ILocker _locker;

        public Lock(ILocker locker, Guid id, Guid objectId)
        {
            _locker = locker;
            Id = id;
            ObjectId = objectId;
        }

        public Guid Id { get; }
        public Guid ObjectId { get; }

        public void Dispose()
        {
            _locker.Unlock(Id);
        }
    }
}
