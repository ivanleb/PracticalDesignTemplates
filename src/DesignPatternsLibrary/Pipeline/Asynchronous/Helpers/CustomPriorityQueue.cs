using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DesignPatternsLibrary.Pipeline.Asynchronous
{
    public class CustomPriorityQueue<T, TPriority> : PriorityQueue<T, TPriority>, IProducerConsumerCollection<T>
    {
        private readonly Object _syncRoot = new Object();
        public bool IsSynchronized => false;

        public object SyncRoot => _syncRoot;

        public void CopyTo(T[] array, int index)
        {
            var arr = ToArray();
            Array.Copy(arr, array, arr.Length - index);
        }

        public void CopyTo(Array array, int index)
        {
            var arr = ToArray();
            Array.Copy(arr, array, arr.Length - index);
        }

        public IEnumerator<T> GetEnumerator() => ToList().GetEnumerator();

        public T[] ToArray() => ToList().ToArray();

        public bool TryAdd(T item)
        {
            Enqueue(item, default(TPriority));
            return true;
        }

        public bool TryTake([MaybeNullWhen(false)] out T item) => TryDequeue(out item, out TPriority priority);

        IEnumerator IEnumerable.GetEnumerator() => ToArray().GetEnumerator();

        private List<T> ToList()
        {
            List<T> result = new List<T>();
            while (TryDequeue(out T? item, out TPriority? priority))
            {
                result.Add(item);
            };

            return result;
        }
    }
}
