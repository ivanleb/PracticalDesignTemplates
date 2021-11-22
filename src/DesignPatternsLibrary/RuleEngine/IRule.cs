using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatternsLibrary.RuleEngine
{
    public interface IRule<T>
    {
        bool IsMatch(Context<T> context);
        IEvaluatingResult<T> Evaluate(Context<T> context);
    }
}
