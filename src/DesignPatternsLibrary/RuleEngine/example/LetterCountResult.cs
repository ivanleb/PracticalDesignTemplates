using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatternsLibrary.RuleEngine.example
{
    internal class LetterCountResult : IEvaluatingResult
    {
        public string Value { get; set; }
    }
}
