using System;

namespace DesignPatternsLibrary.RemoteObjectCollection
{
    public interface IChunkProvider<TChunk> : IDisposable
    {
        TChunk GetChunk(IChunkId chunkId);
        bool ChangeChunk(TChunk? chunk, IChunkId chunkId);
        IChunkId Create(TChunk chunk);
        bool Remove(IChunkId chunkId);
    }
}