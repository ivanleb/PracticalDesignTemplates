using System;
using System.Threading.Tasks;

namespace DesignPatternsLibrary.Lock.AcquireLockStrategy
{
    public class UseCase
    {
        public static void Run()
        {
            ICancellationTokenProvider cancellationTokenProvider = new CancellationTokenProvider();
            LockStrategiesFactory lockStrategiesFactory = new LockStrategiesFactory(defaultRetryCount: 10, TimeSpan.FromSeconds(1), cancellationTokenProvider);
            OneTryLock(lockStrategiesFactory);
            CountedRetryingLock(lockStrategiesFactory);
            UnboundedWaitingLock(lockStrategiesFactory);
        }

        private static void OneTryLock(LockStrategiesFactory lockStrategiesFactory)
        {
            IAcquireLockStrategy lockStrategy = lockStrategiesFactory.Create(LockStrategyType.OneTry);
            ILocker locker = new LockerStrategyDecorator(new Locker(), lockStrategy);

            Guid objectId = Guid.NewGuid();
            if (locker.TryLock(objectId, out ILock lock1))
            {
                Console.WriteLine($"First lock for {objectId} has been caught, lock id: {lock1.Id}");
                if (!locker.TryLock(objectId, out ILock lock2))
                {
                    Console.WriteLine($"Second lock for {objectId} has not been caught, lock id: {lock2.Id}");
                }
            }
            locker.Unlock(lock1.Id);
        }

        private async static void CountedRetryingLock(LockStrategiesFactory lockStrategiesFactory)
        {
            IAcquireLockStrategy lockStrategy = lockStrategiesFactory.Create(LockStrategyType.CountedRetrying);
            ILocker locker = new LockerStrategyDecorator(new Locker(), lockStrategy);

            Guid objectId = Guid.NewGuid();
            if (locker.TryLock(objectId, out ILock lock1))
            {
                Console.WriteLine($"First lock for {objectId} has been caught, lock id: {lock1.Id}");
                Task task = Task.Run(async () =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(15));
                    locker.Unlock(lock1.Id);
                });

                if (locker.TryLock(objectId, out ILock lock2))
                {
                    Console.WriteLine($"Second lock for {objectId} has been caught, lock id: {lock2.Id}");
                    locker.Unlock(lock2.Id);
                }

                await task;
            }
            locker.Unlock(lock1.Id);
        }

        private static async void UnboundedWaitingLock(LockStrategiesFactory lockStrategiesFactory)
        {
            IAcquireLockStrategy lockStrategy = lockStrategiesFactory.Create(LockStrategyType.UnboundedWaiting);
            ILocker locker = new LockerStrategyDecorator(new Locker(), lockStrategy);

            Guid objectId = Guid.NewGuid();
            if (locker.TryLock(objectId, out ILock lock1))
            {
                Console.WriteLine($"First lock for {objectId} has been caught, lock id: {lock1.Id}");
                Task task = Task.Run(async () => 
                {
                    await Task.Delay(TimeSpan.FromSeconds(15));
                    locker.Unlock(lock1.Id); 
                });

                if (locker.TryLock(objectId, out ILock lock2))
                {
                    Console.WriteLine($"Second lock for {objectId} has been caught, lock id: {lock2.Id}");
                    locker.Unlock(lock2.Id);
                }

                await task;
            }
            locker.Unlock(lock1.Id);
        }
    }
}
