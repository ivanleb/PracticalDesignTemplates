using System;

namespace DesignPatternsLibrary.RemoteObjectCollection
{
    public interface IChunkId
    {
        Guid Id { get; }
        object Path { get; }
        int Count { get; }
    }
}
