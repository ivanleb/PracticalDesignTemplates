using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternsLibrary.RemoteObjectCollection
{
    public struct Chunk<T>
    {
        private readonly List<T> _data = new List<T>();

        public Chunk(int size)
        {
            Size = size;
        }

        public Chunk(IReadOnlyList<T> data)
        { 
            _data = data.ToList();
            Size = data.Count;
        }

        public int Size { get; }
        public int Count => _data.Count();

        public T this[int index]
        {
            get => IsIndexInRange(index) ? _data[index] : throw new IndexOutOfRangeException($"Index {index}, data range 0..{_data.Count}");

            set
            {
                if (IsIndexInRange(index))
                    _data[index] = value;
                else
                    throw new IndexOutOfRangeException($"Index {index}, data range 0..{_data.Count}");
            }
        }


        public bool Add(T entity)
        {
            if (_data.Count >= Size) 
                return false;

            _data.Add(entity);
            return true;
        }

        public bool Remove(T entity) => _data.Remove(entity);
        public void RemoveAt(int i) => _data.RemoveAt(i);
        public void Insert(int index, T value) => _data.Insert(index, value);
        public int FindIndex(T entity) => _data.FindIndex(x => x is not null && x.Equals(entity));
        public void Clear() => _data.Clear();
        public T[] GetValues() => _data.ToArray();
        private readonly bool IsIndexInRange(int index) => index >= 0 && index < _data.Count;
    }

    public static class ChunkExtentions 
    {
        public static void AddRange<T>(this Chunk<T> chunk, IReadOnlyCollection<T> entities)
        {
            foreach (var entity in entities)
            {
                chunk.Add(entity);
            }
        }
    }
}
