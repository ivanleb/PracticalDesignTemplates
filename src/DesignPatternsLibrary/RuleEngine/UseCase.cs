using DesignPatternsLibrary.RuleEngine.example;
using System;
using System.Collections.Generic;

namespace DesignPatternsLibrary.RuleEngine
{
    public static class UseCase
    {
        public static void Run() 
        {
            IRule rule = new SCountRule();
            RuleEngine engine = new RuleEngine(new List<IRule> { rule }, new CountResultCollector());
            var result = engine.Apply(new Context { Value = "ssssssssssssssssss" });
            Console.WriteLine($"RuleEngine: {result.Value}");
        }
    }
}
