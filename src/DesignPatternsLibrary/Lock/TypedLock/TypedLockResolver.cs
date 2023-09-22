using System.Collections.Generic;

namespace DesignPatternsLibrary.Lock.TypedLock
{
    public class TypedLockResolver : ILockResolver
    {
        private readonly Dictionary<LockType, HashSet<LockType>> _compatibleLockTypes = new Dictionary<LockType, HashSet<LockType>>();
        
        public TypedLockResolver() 
        {
            FillCompatibleLockTypes(_compatibleLockTypes);
        }

        private static void FillCompatibleLockTypes(Dictionary<LockType, HashSet<LockType>> compatibleLockTypes) 
        {
            compatibleLockTypes[LockType.Read] = new HashSet<LockType> { LockType.Read };
            compatibleLockTypes[LockType.Write] = new HashSet<LockType> ();
        }

        public bool CanAcquireNewLock(ILock newLock, IReadOnlyList<ILock> oldLocks)
        {
            foreach (var oldLock in oldLocks)
            {
                if (newLock.ObjectId == oldLock.ObjectId && !_compatibleLockTypes[oldLock.Type].Contains(newLock.Type))
                    return false;
            } 
            return true;
        }
    }
}
