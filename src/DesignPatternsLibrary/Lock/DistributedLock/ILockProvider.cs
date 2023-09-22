namespace DesignPatternsLibrary.DistributedLock
{
    internal interface ILockProvider
    {
        ILock CreateLock(string name);
    }
}
