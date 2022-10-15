using System;
using System.Collections;
using System.Collections.Generic;

namespace DesignPatternsLibrary.RemoteObjectCollection
{
    public class RemoteCollection<T> : IDisposable, IEnumerable<T>
    {
        private readonly int _bufferSize;
        private ChunkDictionary<int, Chunk<T>> _pool;
        private Chunk<T> _currentChunk;
        private int _currentIndex;

        public RemoteCollection(int bufferSize)
        {
            _bufferSize = bufferSize;
            _currentChunk = new Chunk<T>(_bufferSize);

            IChunkProvider<Chunk<T>> chunkProvider = null;
            _pool = new ChunkDictionary<int, Chunk<T>>(chunkProvider);
        }


        public int Count { get; set; }

        public T this[int i]
        {
            get
            {
                if (IsIndexInCurrentChunk(i))
                {
                    return _currentChunk[i - _currentIndex * _bufferSize];
                }
                else if (IsIndexOutOfCurrentChunk(i)) //  вне текущего bulk-а
                {
                    int index = (int)(Math.Floor((double)i / _bufferSize));
                    ChangeBulk(index);
                    return _currentChunk[i - _currentIndex * _bufferSize];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (IsIndexInCurrentChunk(i))
                {
                    _currentChunk[i - _currentIndex * _bufferSize] = value;
                }
                else if (IsIndexOutOfCurrentChunk(i)) //  вне текущего bulk-а
                {
                    int index = (int)(Math.Floor((double)i / _bufferSize));
                    ChangeBulk(index);
                    _currentChunk[i - _currentIndex * _bufferSize] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        private bool IsIndexOutOfCurrentChunk(int i) => i < _currentIndex * _bufferSize && i >= 0 || i >= (_currentIndex + 1) * _bufferSize && i < Count;
        private bool IsIndexInCurrentChunk(int i) => i >= _currentIndex * _bufferSize && i < (_currentIndex + 1) * _bufferSize;

        public void Add(T entity)
        {
            //добавлять только если мы находимся в конце pool-а и текущий Bulk не заполнен до конца
            int maxIndex = _pool.Count == 0 ? 0 : _pool.Count - 1;
            if (_currentIndex >= maxIndex)
            {
                if (_currentChunk.Count < _currentChunk.Size)
                {
                    _currentChunk.Add(entity);
                }
                else
                {
                    AddBulk(_currentChunk);//Добавить текущий Bulk в Pool
                    //changeBulk(Pool.Count - 1); //переключиться на последний
                    _currentChunk.Add(entity);
                }
                ++Count;
            }
            else
            {
                ChangeBulk(maxIndex);
                Add(entity);
            }
        }

        public bool Remove(T entity)
        {
            //ищем объект во всем Pool-е
            //если он находится в последнем Bulke-е то удаляем просто из него
            //если он находится не в последнем, то переключаемся на нужный Bulk, удаляем из него, 
            //затем перемещаем в него элемент из последующего Bulk-а 
            //и так до тех пор пока незаполненным останется только последний Bulk.
            // если последний bulk получается пустым, то удаляем его
            int maxIndex = _pool.Count;

            int tmpIndex = _currentIndex;
            int entityIndex = IndexOf(entity);
            if (entityIndex != -1)
            {
                int entityIndexInPool = (int)Math.Floor((double)entityIndex / _bufferSize);
                ChangeBulk(entityIndexInPool);
                if (_currentIndex == maxIndex)
                {
                    _currentChunk.Remove(entity);
                    if (_currentChunk.Count == 0)
                    {
                        _pool.Remove(_currentIndex);
                        --_currentIndex;
                    }
                    --Count;
                    ChangeBulk(tmpIndex);
                    return true;
                }
                else
                {
                    _currentChunk.Remove(entity);
                    RebuildPool(_currentIndex);
                    ChangeBulk(tmpIndex);
                    --Count;
                    return true;
                }
            }
            else return false;
        }

        public int IndexOf(T entity)
        {
            //сначала ищщем в текущем Bulk-е
            //Если нет, то перебираем всю коллекцию, кроме того в котором уже искали
            int tmpIndex = _currentIndex;
            int ib = _currentChunk.FindIndex(entity);
            if (ib != -1) return ib + _currentIndex * _bufferSize;
            else
            {
                for (int i = 0; i < _pool.Count; i++)
                {
                    if (i == tmpIndex) continue;
                    ChangeBulk(i);
                    int cib = _currentChunk.FindIndex(entity);
                    if (cib != -1)
                    {
                        int result = cib + _currentIndex * _bufferSize;
                        ChangeBulk(tmpIndex);
                        return result;
                    }
                }
                return -1;
            }
        }

        public void Insert(int index, T value)
        {
            if (index > Count || index < 0) throw new IndexOutOfRangeException();
            //если индекс, по которому нужно вставить находится в текущем Bulk-е, то просто вставляем
            int neededIndex = index / _bufferSize;
            // если текущий bulk дальше от начала коллекции чем требуемый
            if (_currentIndex * _bufferSize > index || (_currentIndex + 1) * _bufferSize >= index)
            {
                ChangeBulk(neededIndex);
            }
            _currentChunk.Insert(index - _currentIndex * _bufferSize, value);
            RebuildPool(neededIndex);
            Count++;
        }

        public void Dispose()
        {
            Clear();
        }

        public void Clear()
        {
            Count = 0;
            _currentIndex = 0;
            _currentChunk.Clear();
            _pool.Dispose();
        }


        public IEnumerator<T> GetEnumerator() => new RemoteCollectionEnumerator<T>(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void ChangeBulk(int index)
        {
            if (_currentIndex != index)
            {
                _pool[_currentIndex] = _currentChunk;
                _currentChunk = _pool[index];
                _currentIndex = index;
            }
        }

        private void RebuildPool(int indexFrom, int indexExcept = -1)
        {
            if (_currentIndex == _pool.Count - 1)
            {
                return;
            }
            for (int i = 0; i < _pool.Count; i++)
            {
                if (i == indexExcept) continue;
                if (_currentIndex + 1 != _pool.Count)
                {
                    //взять следующий Bulk с диска
                    Chunk<T> tmpBulk = _pool[_currentIndex + 1];

                    //циклически перемещать лишний элемент в конец
                    if (_currentChunk.Count > _bufferSize)
                    {
                        //взять из текущего bulk-а последний элемент и добавить в начало следующего 
                        tmpBulk.Insert(0, _currentChunk[_currentChunk.Count - 1]);
                        _currentChunk.RemoveAt(_currentChunk.Count - 1);
                    }
                    //циклически сдвигать пустое место в конец
                    else
                    {
                        //взять из него первый элемент и добавить в конец текущего 
                        _currentChunk.Add(tmpBulk[0]);
                        tmpBulk.RemoveAt(0);
                    }

                    //положить текущий bulk на диск
                    _pool[_currentIndex] = _currentChunk;
                    _currentChunk = tmpBulk;
                    _currentIndex++;
                }
            }
        }

        private void AddBulk(Chunk<T> bulk)
        {
            _pool.Add(_currentIndex, bulk);// new Bulk<T>(_bufferSize));//add bulk
            _currentChunk = new Chunk<T>(_bufferSize);
            ++_currentIndex;
        }
    }
}
