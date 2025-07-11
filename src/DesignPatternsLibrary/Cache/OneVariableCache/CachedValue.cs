using System;

namespace DesignPatternsLibrary.Cache.OneVariableCache
{
    internal class CachedValue<T> where T : class
    {
        public CachedValue(Func<T> factory, TimeSpan expirationPeriod)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _expirationPeriod = expirationPeriod;
        }
        private Func<T> _factory;
        private T _value;
        private TimeSpan _expirationPeriod;
        private DateTime? _lastSetTime;

        public T GetValue()
        {
            if (_value == default(T) || !_lastSetTime.HasValue || IsExpired())
            {
                _value = AcquireValue();
            }

            return _value;
        }

        private bool IsExpired()
        {
            return _lastSetTime.HasValue && _lastSetTime.Value < (DateTime.UtcNow - _expirationPeriod);
        }

        private T AcquireValue()
        {
            T value = _factory();
            _lastSetTime = DateTime.UtcNow;
            return value;
        }
    }
}
