using System;

namespace DesignPatternsLibrary.RemoteObjectCollection
{
    public interface IChunkProvider<TChunk> : IDisposable
    {
        TChunk FindStorage(IChunkId chunkId);
        bool Save(TChunk? chunk, IChunkId chunkId);
        IChunkId Create(TChunk chunk);
        bool Remove(IChunkId chunkId);
    }
}