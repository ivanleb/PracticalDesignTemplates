﻿

//DesignPatternsLibrary.Pipeline.Synchronous.UseCase.Run();
//DesignPatternsLibrary.Pipeline.MultiThreaded.UseCase.Run();
await DesignPatternsLibrary.Pipeline.Asynchronous.UseCase.Run();
Console.WriteLine("end");
//DesignPatternsLibrary.RuleEngine.UseCase.Run();
//DesignPatternsLibrary.Specification.UseCase.Run();
//DesignPatternsLibrary.Cache.Memoization.UseCase.Run();
//await DesignPatternsLibrary.BackgroundWorkerQueue.UseCase.Run();

//DesignPatternsLibrary.Pipeline.MultiProcess.UseCase.Run();
//DesignPatternsLibrary.MapReduce.UseCase.Run();

List<string> errors = new List<string>();
Console.WriteLine(errors.Capacity);
