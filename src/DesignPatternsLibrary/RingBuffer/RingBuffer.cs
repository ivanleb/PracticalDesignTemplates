using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.RingBuffer
{
    public class RingBuffer<T>
    {
        private int _tail;              
        private int _head;              
        private int _size;              
        private int _capacity;          
        private T[] _queue;         

        public RingBuffer(int capacity)
        {
            _tail = -1;
            _head = 0;
            _size = 0;
            _capacity = capacity;
            _queue = new T[_capacity];
        }

        public bool Enqueue(T item) 
        {
            if (_size == _capacity)
                return false;

            _tail = (_tail + 1) % _capacity;
            _queue[_tail] = item;
            _size++;
            return true;
        }

        public bool Dequeue(out T item)
        {
            if (_size == 0)
            {
                item = default(T);
                return false;
            }

            item = _queue[_head];
            _head = (_head + 1) % _capacity;
            _size--;
            return true;
        }
    }
}
