using System;

namespace DesignPatternsLibrary.Lock.TypedLock
{
    public interface ILock
    {
        Guid Id { get; }
        Guid ObjectId { get; }
        LockType Type { get; }
    }
}
