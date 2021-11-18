namespace DesignPatternsLibrary.Pipeline.Synchronous.Steps
{
    internal class MultiplyIntStep : IPipelineStep<int, int>
    {
        private readonly int _value;

        public MultiplyIntStep(int value)
        {
            _value = value;
        }

        public int Process(int input)
        {
            return _value * input;
        }
    }
}
