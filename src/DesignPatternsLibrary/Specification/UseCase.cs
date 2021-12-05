using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatternsLibrary.Specification
{
    public static class UseCase
    {
        public static void Run()
        {
            RedColorTypeMemberSpecification redColorTypeMemberSpecification = new RedColorTypeMemberSpecification();
            LengthMoreThan10TypeMemberSpecification lengthMoreThan10TypeMemberSpecification = new LengthMoreThan10TypeMemberSpecification();
            AndSpecification<TypeMember> redAndLongSpecification = new AndSpecification<TypeMember>(redColorTypeMemberSpecification, lengthMoreThan10TypeMemberSpecification);

            List<TypeMember> typeMembers = new List<TypeMember>
            {
                new TypeMember {Color = "Blue", Length = 15, IsMarked = false},
                new TypeMember {Color = "Red", Length = 5, IsMarked = false},
                new TypeMember {Color = "Red", Length = 15, IsMarked = true},
            };

            foreach (var filteredMember in typeMembers.Where(redAndLongSpecification.IsSatisfiedBy))
            {
                Console.WriteLine($"Color: {filteredMember.Color}, Length: {filteredMember.Length}, IsMarked: {filteredMember.IsMarked}");
            }
        }
    }
}
