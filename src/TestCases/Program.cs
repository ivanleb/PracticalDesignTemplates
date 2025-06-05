DesignPatternsLibrary.Pipeline.Synchronous.UseCase.Run();
DesignPatternsLibrary.Pipeline.MultiThreaded.UseCase.Run();
DesignPatternsLibrary.Pipeline.Asynchronous.UseCase.Run();
DesignPatternsLibrary.RuleEngine.UseCase.Run();
DesignPatternsLibrary.Specification.UseCase.Run();
DesignPatternsLibrary.Cache.Memoization.UseCase.Run();
DesignPatternsLibrary.Cache.PersistentInProcessCache.UseCase.Run();
await DesignPatternsLibrary.BackgroundWorkerQueue.UseCase.Run();

DesignPatternsLibrary.Pipeline.MultiProcess.UseCase.Run();
DesignPatternsLibrary.MapReduce.UseCase.Run();
DesignPatternsLibrary.Lock.SimpleLock.UseCase.Run();
DesignPatternsLibrary.Lock.TypedLock.UseCase.Run();
DesignPatternsLibrary.Lock.AcquireLockStrategy.UseCase.Run();
DesignPatternsLibrary.Disposable.UseCase.Run();
DesignPatternsLibrary.RingBuffer.UseCase.Run();
DesignPatternsLibrary.EventLoop.UseCase.Run();
DesignPatternsLibrary.RAII.UseCase.Run();

DesignPatternsLibrary.Observer.DispatchedObserver.UseCase.Run();
DesignPatternsLibrary.ObjectPool.UseCase.Run();
DesignPatternsLibrary.Retrying.UseCase.Run();
