using System;
using CoreValidation.Exceptions;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal static class MemberValidatorProcessor
    {
        public static IMemberSpecification Process<TModel, TMember>(MemberValidator<TModel, TMember> memberValidator)
            where TModel : class
        {
            if (memberValidator == null)
            {
                throw new ArgumentNullException(nameof(memberValidator));
            }

            var newSpecification = new MemberSpecificationBuilder<TModel, TMember>();

            var processedSpecification = memberValidator(newSpecification);

            if (!ReferenceEquals(newSpecification, processedSpecification))
            {
                throw new InvalidProcessedReferenceException(typeof(MemberSpecificationBuilder<TModel, TMember>));
            }

            return processedSpecification as MemberSpecificationBuilder<TModel, TMember>;
        }
    }
}