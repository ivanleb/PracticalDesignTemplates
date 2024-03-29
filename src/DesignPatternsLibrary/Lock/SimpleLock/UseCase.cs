﻿using System;

namespace DesignPatternsLibrary.Lock.SimpleLock
{
    public static class UseCase
    {
        public static void Run()
        {
            ILocker locker = new Locker();
            Guid objectId = Guid.NewGuid();
            if (locker.TryLock(objectId, out ILock lock1)) 
            {
                Console.WriteLine($"Lock for {objectId} has been caught, lock id: {lock1.Id}");
                if(!locker.TryLock(objectId, out ILock lock2))
                {
                    Console.WriteLine($"Second lock for {objectId} has not been caught, lock id: {lock2.Id}");
                }
            }
            lock1.Dispose();
        }
    }
}
