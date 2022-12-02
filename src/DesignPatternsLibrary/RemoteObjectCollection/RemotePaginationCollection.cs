using System;
using System.Collections;
using System.Collections.Generic;

namespace DesignPatternsLibrary.RemoteObjectCollection
{
    public class RemotePaginationCollection<T> : IDisposable, IEnumerable<T>
    {
        private readonly int _bufferSize;
        private ChunkDictionary<int, Chunk<T>> _pool;
        private Chunk<T> _currentChunk;
        private int _currentChunkIndex;

        public RemotePaginationCollection(int bufferSize, IChunkProvider<Chunk<T>> chunkProvider)
        {
            _bufferSize = bufferSize;
            _currentChunk = new Chunk<T>(_bufferSize);
            _pool = new ChunkDictionary<int, Chunk<T>>(chunkProvider);
            _currentChunkIndex = _pool.ActiveChunkKey;
        }

        public void Add(T entity) 
        {
            if (!IsCurrentChunkLast)
                ChangeChunk(LastChunkIndex);
            AddToLastChunk(entity);
        }

        public int IndexOf(T entity) 
        {
            int tmpIndex = _currentChunkIndex;
            int ib = _currentChunk.FindIndex(entity);
            if (ib != -1) 
                return ib + _currentChunkIndex * _bufferSize;

            for (int i = 0; i < _pool.Count; i++)
            {
                if (i == tmpIndex) 
                    continue;

                ChangeChunk(i);
                int cib = _currentChunk.FindIndex(entity);
                if (cib != -1)
                {
                    int result = cib + _currentChunkIndex * _bufferSize;
                    ChangeChunk(tmpIndex);
                    return result;
                }
            }
            return -1;
        }

        public bool Remove(T entity)
        {
            //ищем объект во всем Pool-е
            //если он находится в последнем Bulke-е то удаляем просто из него
            //если он находится не в последнем, то переключаемся на нужный Bulk, удаляем из него, 
            //затем перемещаем в него элемент из последующего Bulk-а 
            //и так до тех пор пока незаполненным останется только последний Bulk.
            // если последний bulk получается пустым, то удаляем его
            int maxIndex = LastChunkIndex;

            int tmpIndex = _currentChunkIndex;

            int entityIndex = IndexOf(entity);
            if (entityIndex != -1)
            {
                int entityIndexInPool = (int)Math.Floor((double)entityIndex / _bufferSize);
                ChangeChunk(entityIndexInPool);
                if (_currentChunkIndex == maxIndex)
                {
                    _currentChunk.Remove(entity);
                    if (_currentChunk.Count == 0)
                    {
                        _pool.Remove(_currentChunkIndex);
                        --_currentChunkIndex;
                    }
                    ChangeChunk(tmpIndex);
                    return true;
                }
                else
                {
                    _currentChunk.Remove(entity);
                    ChangeChunk(tmpIndex);
                    return true;
                }
            }
            else return false;
        }

        public IEnumerator<T> GetEnumerator() => new RemoteCollectionEnumerator<T>(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        public void Dispose() => Clear();
        public void Clear()
        {
            _currentChunkIndex = 0;
            _currentChunk.Clear();
            _pool.Dispose();
        }

        private void ChangeChunk(int chunkIndex) 
        {
            if (_currentChunkIndex == chunkIndex)
                return;

            _pool[_currentChunkIndex] = _currentChunk;
            _currentChunk = _pool[chunkIndex];
            _currentChunkIndex = chunkIndex;
        }

        private void AddToLastChunk(T entity) 
        {
            if (_currentChunk.Add(entity))
                return;
            var chunk = new Chunk<T>(_bufferSize);
            chunk.Add(entity);
            _pool.Add(_pool.Count, chunk);
        }

        private bool IsCurrentChunkLast => _pool.Count == 0 || _currentChunkIndex == _pool.Count - 1;
        private int LastChunkIndex => _pool.Count == 0 ? _pool.Count : _pool.Count - 1;
    }
}
