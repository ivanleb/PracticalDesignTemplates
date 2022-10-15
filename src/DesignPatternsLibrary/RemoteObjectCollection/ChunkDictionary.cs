using System;
using System.Collections.Generic;

namespace DesignPatternsLibrary.RemoteObjectCollection
{
    internal class ChunkDictionary<TKey, TStorage> : IDisposable
        where TKey : IComparable<TKey>
    {
        private readonly SortedDictionary<TKey, IChunkId> _storages = new SortedDictionary<TKey, IChunkId>();
        private readonly IChunkProvider<TStorage> _storageProvider;

        public ChunkDictionary(IChunkProvider<TStorage> storageProvider)
        {
            _storageProvider = storageProvider;
        }

        public int Count => _storages.Count;

        public TStorage this[TKey key]
        {
            get => _storageProvider.FindStorage(_storages[key]);
            set => SaveOrCreate(key, value);
        }

        public void Add(TKey key, TStorage obj)
        {
            _storages[key] = _storageProvider.Create(obj);
        }

        public bool Remove(TKey key)
        {
            if (_storages.ContainsKey(key))
            {
                if (_storageProvider.Remove(_storages[key])) 
                {
                    _storages.Remove(key);
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            _storageProvider.Dispose();
        }

        private void SaveOrCreate(TKey key, TStorage storage) 
        {
            if (_storages.ContainsKey(key))
            {
                _storageProvider.Save(storage, _storages[key]);
                return;
            }

            Add(key, storage);
        }
    }
}
