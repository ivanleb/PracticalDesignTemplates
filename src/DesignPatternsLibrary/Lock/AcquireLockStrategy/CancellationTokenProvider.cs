using System;
using System.Threading;

namespace DesignPatternsLibrary.Lock.AcquireLockStrategy
{
    public class CancellationTokenProvider : ICancellationTokenProvider
    {
        public CancellationToken GetCancellationToken(Guid objectId)
        {
            return new CancellationTokenSource(TimeSpan.FromSeconds(20)).Token;
        }
    }
}
