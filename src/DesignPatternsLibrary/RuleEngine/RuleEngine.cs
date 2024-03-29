﻿using System.Collections.Generic;

namespace DesignPatternsLibrary.RuleEngine
{
    public class RuleEngine
    {
        private readonly List<IRule> _rules;
        private readonly IResultCollector _resultCollector;
        public RuleEngine(List<IRule> rules, IResultCollector resultCollector)
        {
            _rules = rules;
            _resultCollector = resultCollector;
        }

        public IEvaluatingResult Apply(Context context) 
        {
            foreach (IRule rule in _rules)
            {
                _resultCollector.Collect(rule.Evaluate(context));
            }

            return _resultCollector.GetTotal();
        }
    }
}
