namespace DesignPatternsLibrary.Pipeline.Synchronous.Steps
{
    internal class DivideIntByStep : IPipelineStep<int, int>
    {
        private readonly int _divider;

        public DivideIntByStep(int divider)
        {
            _divider = divider;
        }

        public int Process(int input)
        {
            return input / _divider;
        }
    }
}
