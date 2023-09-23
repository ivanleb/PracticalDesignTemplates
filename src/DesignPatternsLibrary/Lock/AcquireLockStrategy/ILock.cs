using System;

namespace DesignPatternsLibrary.Lock.AcquireLockStrategy
{
    public interface ILock
    {
        Guid Id { get; }
        Guid ObjectId { get; }
    }
}
