using System;

namespace DesignPatternsLibrary.Lock.AcquireLockStrategy
{
    public class Lock : ILock
    {
        public Lock(Guid id, Guid objectId)
        {
            Id = id;
            ObjectId = objectId;
        }

        public Guid Id { get; }
        public Guid ObjectId { get; }
    }
}
