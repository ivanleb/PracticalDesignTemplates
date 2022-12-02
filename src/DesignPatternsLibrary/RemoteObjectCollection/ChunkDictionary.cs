using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternsLibrary.RemoteObjectCollection
{
    internal class ChunkDictionary<TKey, TChunk> : IDisposable
        where TKey : IComparable<TKey>
    {
        private readonly SortedDictionary<TKey, IChunkId> _chunks = new SortedDictionary<TKey, IChunkId>();
        private readonly IChunkProvider<TChunk> _chunkProvider;

        public ChunkDictionary(IChunkProvider<TChunk> chunkProvider)
        {
            _chunkProvider = chunkProvider;
        }

        public int Count => _chunks.Count;
        public TKey ActiveChunkKey { get; private set; } = default(TKey);

        public TChunk this[TKey key]
        {
            get => GetChunk(key);
            set => SaveOrCreate(key, value);
        }

        public void Add(TKey key, TChunk obj)
        {
            _chunks[key] = _chunkProvider.Create(obj);
        }

        public bool Remove(TKey key)
        {
            if (_chunks.ContainsKey(key) && _chunkProvider.Remove(_chunks[key]))
            {
                _chunks.Remove(key);

                if (ActiveChunkKey.Equals(key))
                    ActiveChunkKey = default(TKey);

                return true;
            }
            return false;
        }

        public TChunk? Last()
        {
            TKey? lastKey = _chunks.Keys.Max();
            if (lastKey == null)
                return default(TChunk?);

            return GetChunk(lastKey);
        }

        public void Dispose()
        {
            _chunkProvider.Dispose();
        }

        private TChunk GetChunk(TKey key)
        {
            IChunkId chunkId = _chunks[key];
            ActiveChunkKey = key;
            return _chunkProvider.GetChunk(chunkId);
        }

        private void SaveOrCreate(TKey key, TChunk storage)
        {
            if (_chunks.ContainsKey(key))
            {
                _chunkProvider.ChangeChunk(storage, _chunks[key]);
                return;
            }

            Add(key, storage);
        }
    }
}
