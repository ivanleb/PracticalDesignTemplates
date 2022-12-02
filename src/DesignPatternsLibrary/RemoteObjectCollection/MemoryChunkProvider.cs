using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.RemoteObjectCollection
{
    internal class MemoryChunkProvider<T> : IChunkProvider<Chunk<T>>
        where T : ISerializable
    {

        public bool ChangeChunk(Chunk<T> chunk, IChunkId chunkId)
        {
            throw new NotImplementedException();
        }

        public IChunkId Create(Chunk<T> chunk)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Chunk<T> GetChunk(IChunkId chunkId)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IChunkId chunkId)
        {
            throw new NotImplementedException();
        }
    }
}
