using System;

namespace DesignPatternsLibrary.NTimesValue
{
    internal class NTimesValue<T>
    {
        private readonly T _value;
        private readonly int _maxAccessCount;
        private int _accessCount;

        public NTimesValue(T value, int maxAccessCount)
        {
            _value = value;
            _maxAccessCount = maxAccessCount;
        }

        public T Value
        {
            get
            {
                if (HasValue)
                {
                    _accessCount++;
                    return _value;
                }
                else
                {
                    throw new InvalidOperationException($"The value has been accessed the maximum number of times - {_maxAccessCount}");
                }
            }
        }

        public bool HasValue => _accessCount < _maxAccessCount; 

        public bool TryGetValue(out T value)
        {
            if (HasValue)
            {
                value = Value;
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }
    }
}
