using System;
using System.Linq.Expressions;

namespace DesignPatternsLibrary.Specification
{
    public class RedColorTypeMemberSpecification : SpecificationBase<TypeMember>
    {
        public RedColorTypeMemberSpecification()
        {
            _expression = member => member.Color == "Red";
        }

        public override Expression<Func<TypeMember, bool>> ToExpression()
        {
            return _expression;
        }
    }
}
