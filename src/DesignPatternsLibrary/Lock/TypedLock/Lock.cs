using System;

namespace DesignPatternsLibrary.Lock.TypedLock
{
    internal class Lock : ILock
    {
        public Lock(Guid id, LockType type, Guid objectId)
        {
            Id = id;
            Type = type;
            ObjectId = objectId;
        }

        public Guid Id { get; }
        public LockType Type { get; }
        public Guid ObjectId { get; }
    }
}
