using System;
using System.Linq;

namespace DesignPatternsLibrary.RuleEngine.example
{
    public class SCountRule : IRule
    {
        public IEvaluatingResult Evaluate(Context context)
        {
            return new LetterCountResult() { Value = Convert.ToString(context.Value.Count(x => x == 's')) };
        }
    }
}
