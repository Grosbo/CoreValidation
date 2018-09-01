using System;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Rules;


// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidNullableExtension
    {
        public static IMemberSpecificationBuilder<TModel, TMember?> ValidNullable<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember?> @this, MemberSpecification<TModel, TMember> memberSpecification)
            where TModel : class
            where TMember : struct
        {
            if (memberSpecification == null)
            {
                throw new ArgumentNullException(nameof(memberSpecification));
            }

            var memberSpecificationBuilder = (MemberSpecificationBuilder<TModel, TMember?>)@this;

            memberSpecificationBuilder.AddRule(new ValidNullableRule<TModel, TMember>(memberSpecification));

            return @this;
        }
    }
}