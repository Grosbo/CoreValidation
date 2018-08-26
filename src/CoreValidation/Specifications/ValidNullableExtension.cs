using System;
using CoreValidation.Specifications;
using CoreValidation.Validators;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidNullableExtension
    {
        public static IMemberSpecificationBuilder<TModel, TMember?> ValidNullable<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember?> @this, MemberValidator<TModel, TMember> memberValidator)
            where TModel : class
            where TMember : struct
        {
            if (memberValidator == null)
            {
                throw new ArgumentNullException(nameof(memberValidator));
            }

            var memberSpecification = (MemberSpecificationBuilder<TModel, TMember?>) @this;

            memberSpecification.AddRule(new ValidNullableRule<TModel, TMember>(memberValidator));

            return @this;
        }
    }
}