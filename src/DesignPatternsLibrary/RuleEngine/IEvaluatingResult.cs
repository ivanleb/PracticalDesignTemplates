using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatternsLibrary.RuleEngine
{
    public interface IEvaluatingResult<T>
    {
        public T Value { get; set; }
    }
}
