using System;
using System.Linq.Expressions;

namespace DesignPatternsLibrary.Specification
{
    public abstract class SpecificationBase<T> : ISpecification<T>
    {
        protected Expression<Func<T, bool>> _expression = null;
        public abstract Expression<Func<T, bool>> ToExpression();

        public bool IsSatisfiedBy(T item)
        {
            Func<T, bool> pridicate = ToExpression().Compile();
            return pridicate(item);
        }
    }
}
