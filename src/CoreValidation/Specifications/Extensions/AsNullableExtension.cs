using System;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;


// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class AsNullableExtension
    {
        public static IMemberSpecificationBuilder<TModel, TMember?> AsNullable<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember?> @this, MemberSpecification<TModel, TMember> memberSpecification)
            where TModel : class
            where TMember : struct
        {
            if (memberSpecification == null)
            {
                throw new ArgumentNullException(nameof(memberSpecification));
            }

            var memberSpecificationBuilder = (MemberSpecificationBuilder<TModel, TMember?>)@this;

            memberSpecificationBuilder.AddCommand(new AsNullableRule<TModel, TMember>(memberSpecification));

            return @this;
        }
    }
}