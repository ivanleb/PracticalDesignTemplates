using System;

namespace DesignPatternsLibrary.Lock.SimpleLock
{
    public interface ILock : IDisposable
    {
        Guid Id { get; }
        Guid ObjectId { get; }
    }
}
