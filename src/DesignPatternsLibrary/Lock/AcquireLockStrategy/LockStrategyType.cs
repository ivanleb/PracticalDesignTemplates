namespace DesignPatternsLibrary.Lock.AcquireLockStrategy
{
    public enum LockStrategyType 
    { 
        OneTry,
        CountedRetrying,
        UnboundedWaiting
    }
}
