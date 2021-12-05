using System;
using System.Linq.Expressions;

namespace DesignPatternsLibrary.Specification
{
    public class LengthMoreThan10TypeMemberSpecification : SpecificationBase<TypeMember>
    {
        public LengthMoreThan10TypeMemberSpecification()
        {
            _expression = member => member.Length > 10;
        }

        public override Expression<Func<TypeMember, bool>> ToExpression()
        {
            return _expression;
        }
    }
}
