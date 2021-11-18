using System;

namespace DesignPatternsLibrary.Pipeline.Synchronous.Steps
{
    internal class PrintStrStep : IPipelineStep<string, string>
    {
        public string Process(string input)
        {
            Console.WriteLine(input); 
            return input;
        }
    }
}
