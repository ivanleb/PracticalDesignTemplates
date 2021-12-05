namespace DesignPatternsLibrary.RuleEngine
{
    public interface IRule
    {
        IEvaluatingResult Evaluate(Context context);
    }
}
