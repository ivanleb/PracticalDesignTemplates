namespace DesignPatternsLibrary.RuleEngine
{
    public interface IResultCollector
    {
        IEvaluatingResult GetTotal();
        void Collect(IEvaluatingResult item);
    }
}