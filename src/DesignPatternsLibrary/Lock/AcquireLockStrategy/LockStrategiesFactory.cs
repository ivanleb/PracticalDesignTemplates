using System;

namespace DesignPatternsLibrary.Lock.AcquireLockStrategy
{
    public class LockStrategiesFactory
    {
        private readonly int _defaultRetryCount;
        private readonly TimeSpan _defaultRetryInterval;
        private readonly ICancellationTokenProvider _cancellationTokenProvider;

        public LockStrategiesFactory(int defaultRetryCount, TimeSpan defaultRetryInterval, ICancellationTokenProvider cancellationTokenProvider)
        {
            _defaultRetryCount = defaultRetryCount;
            _defaultRetryInterval = defaultRetryInterval;
            _cancellationTokenProvider = cancellationTokenProvider;
        }

        public IAcquireLockStrategy Create(LockStrategyType lockStrategyType) 
        {
            switch (lockStrategyType)
            {
                case LockStrategyType.OneTry:
                    return new OneTryLockStrategy();
                case LockStrategyType.CountedRetrying:
                    return new RetryingLockStrategy(_defaultRetryCount, _defaultRetryInterval);
                case LockStrategyType.UnboundedWaiting:
                    return new WaitingLockStrategy(_defaultRetryInterval, _cancellationTokenProvider);
                default:
                    throw new ArgumentException($"Can't find locking strategy for this type {lockStrategyType}");
            }
        }
    }
}
