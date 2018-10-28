using System;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;


// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class AsNullableExtension
    {
        /// <summary>
        /// Sets the validation logic for the nullable member's underlying value type.
        /// If the nullable member as no value, the validation logic is not triggered and no error is added.
        /// </summary>
        /// <typeparam name="TModel">Type of the parent model.</typeparam>
        /// <typeparam name="TMember">The underlying value type.</typeparam>
        /// <param name="memberSpecification">The specification for the underlying value type.</param>
        /// <exception cref="Exceptions.InvalidProcessedReferenceException">Thrown when <paramref name="memberSpecification"/> returns different reference than the one on the input.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="memberSpecification"/> is null.</exception>
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