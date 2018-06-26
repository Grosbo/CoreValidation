using System;
using CoreValidation.Specifications;
using CoreValidation.Validators;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidNullableExtension
    {
        public static IMemberSpecification<TModel, TMember?> ValidNullable<TModel, TMember>(this IMemberSpecification<TModel, TMember?> @this, MemberValidator<TModel, TMember> memberValidator)
            where TModel : class
            where TMember : struct
        {
            if (memberValidator == null)
            {
                throw new ArgumentNullException(nameof(memberValidator));
            }

            var memberSpecification = (MemberSpecification<TModel, TMember?>) @this;

            memberSpecification.AddRule(new ValidNullableRule<TModel, TMember>(memberValidator));

            return @this;
        }
    }
}