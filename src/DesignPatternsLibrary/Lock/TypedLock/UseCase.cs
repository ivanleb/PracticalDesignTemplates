using System;

namespace DesignPatternsLibrary.Lock.TypedLock
{
    public static class UseCase
    {
        public static void Run()
        {
            ILockResolver lockResolver = new TypedLockResolver();
            ILocker locker = new Locker(lockResolver);
            Guid objectId = Guid.NewGuid();
            if (locker.TryLock(objectId, LockType.Read, out ILock? lock1))
            {
                Console.WriteLine($"First read lock for {objectId} has been caught, lock id: {lock1?.Id}");
                if (locker.TryLock(objectId, LockType.Read, out ILock? lock2))
                {
                    Console.WriteLine($"Second read lock for {objectId} has been caught, lock id: {lock2?.Id}");
                }

                if (!locker.TryLock(objectId, LockType.Write, out ILock? lock3))
                {
                    Console.WriteLine($"Third write lock for {objectId} has not been caught");
                }


                locker.Unlock(lock2?.Id ?? Guid.NewGuid());
            }
            locker.Unlock(lock1?.Id ?? Guid.NewGuid());
        }
    }
}

