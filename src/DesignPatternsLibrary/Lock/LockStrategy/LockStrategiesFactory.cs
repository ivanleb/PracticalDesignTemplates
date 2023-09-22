using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Lock.LockStrategy
{
    public class LockStrategiesFactory
    {
        private readonly int _defaultRetryCount;
        private readonly TimeSpan _defaultRetryInterval;

        public ILockStrategy Create(ELockAcquiringType lockAcquiringType, CancellationToken cancelationToken) 
        {
            switch (lockAcquiringType)
            {
                case ELockAcquiringType.OneTry:
                    return new OneTryLockStrategy();
                case ELockAcquiringType.CountedRetrying:
                    return new RetryingLockStrategy(_defaultRetryCount, _defaultRetryInterval);
                case ELockAcquiringType.UnboundedWaiting:
                    return new WaitingLockStrategy(_defaultRetryInterval, cancelationToken);
                default:
                    throw new ArgumentException($"Can't find locking strategy for this type {lockAcquiringType}");
            }
        }
    }

    public enum ELockAcquiringType 
    { 
        OneTry,
        CountedRetrying,
        UnboundedWaiting
    }
}
