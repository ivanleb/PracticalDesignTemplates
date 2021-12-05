using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatternsLibrary.RuleEngine.example
{
    internal class CountResultCollector : IResultCollector
    {
        private string _result = "0"; 
        public void Collect(IEvaluatingResult item)
        {
            _result = (int.Parse(_result) + int.Parse(item.Value)).ToString();
        }

        public IEvaluatingResult GetTotal()
        {
            return new LetterCountResult { Value = _result };
        }
    }
}
