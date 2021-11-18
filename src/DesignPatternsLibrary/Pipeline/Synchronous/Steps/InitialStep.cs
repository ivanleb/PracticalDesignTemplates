using System;

namespace DesignPatternsLibrary.Pipeline.Synchronous.Steps
{
    public class InitialStep : IPipelineStep<int, int>
    {
        public int Process(int input)
        {
            return new Random(DateTime.UtcNow.Millisecond).Next(0, input);
        }
    }
}
