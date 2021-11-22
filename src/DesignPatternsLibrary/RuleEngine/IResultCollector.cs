namespace DesignPatternsLibrary.RuleEngine
{
    public interface IResultCollector<T>
    {
        IEvaluatingResult<T> GetTotal();
        void Collect(IEvaluatingResult<T> item);
    }
}