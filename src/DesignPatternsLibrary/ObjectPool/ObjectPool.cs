using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.ObjectPool
{
    internal class ObjectPool<T> where T : new()
    {
        private readonly ConcurrentBag<T> _objects = new ConcurrentBag<T>();
        private readonly Func<T> _objectGenerator;

        public ObjectPool(Func<T> objectGenerator = null)
        {
            _objectGenerator = objectGenerator ?? (() => new T());
        }

        public T GetObject() 
        {
            if (_objects.TryTake(out T item))
                return item;

            return _objectGenerator();
        }

        public void ReturnObject(T item) 
        {
            _objects.Add(item);
        }
    }

    public class ExpensiveResource 
    {
        public int Value { get; set; }
    }
}
