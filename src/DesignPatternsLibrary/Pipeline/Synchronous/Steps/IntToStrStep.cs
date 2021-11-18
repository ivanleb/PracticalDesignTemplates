namespace DesignPatternsLibrary.Pipeline.Synchronous.Steps
{
    public class IntToStrStep : IPipelineStep<int, string>
    {
        public string Process(int input)
        {
            return input.ToString();
        }
    }
}
