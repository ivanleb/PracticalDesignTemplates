using System;
using System.Linq.Expressions;

namespace DesignPatternsLibrary.Specification
{
    public class AndSpecification<T> : SpecificationBase<T>
    {
        private readonly SpecificationBase<T> _spec1;
        private readonly SpecificationBase<T> _spec2;

        public AndSpecification(SpecificationBase<T> spec1, SpecificationBase<T> spec2)
        {
            _spec1 = spec1;
            _spec2 = spec2;

            _expression = Expression.Lambda<Func<T, bool>>(
                Expression.And(
                    _spec1.ToExpression().Body,
                    Expression.Invoke(_spec2.ToExpression(), _spec1.ToExpression().Parameters)
                    ), 
                _spec1.ToExpression().Parameters
                );
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            return _expression;
        }
    }
}
