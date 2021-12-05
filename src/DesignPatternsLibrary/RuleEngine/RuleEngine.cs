using System.Collections.Generic;

namespace DesignPatternsLibrary.RuleEngine
{
    public class RuleEngine
    {
        private readonly List<IRule<string>> _rules;
        private readonly IResultCollector<string> _resultCollector;
        public RuleEngine(List<IRule<string>> rules, IResultCollector<string> resultCollector)
        {
            _rules = rules;
            _resultCollector = resultCollector;
        }

        public IEvaluatingResult<string> Apply(Context<string> context) 
        {
            foreach (IRule<string> rule in _rules)
            {
                if(rule.IsMatch(context))
                    _resultCollector.Collect(rule.Evaluate(context));
            }

            return _resultCollector.GetTotal();
        }
    }
}
