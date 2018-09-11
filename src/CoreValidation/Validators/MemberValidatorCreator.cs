using CoreValidation.Exceptions;
using CoreValidation.Specifications;

namespace CoreValidation.Validators
{
    internal static class MemberValidatorCreator
    {
        public static IMemberValidator Create<TModel, TMember>(MemberSpecification<TModel, TMember> memberSpecification)
            where TModel : class
        {
            var builder = new MemberSpecificationBuilder<TModel, TMember>();

            var processedBuilder = memberSpecification(builder);

            if (!ReferenceEquals(builder, processedBuilder))
            {
                throw new InvalidProcessedReferenceException(typeof(MemberSpecificationBuilder<TModel, TMember>));
            }

            return processedBuilder as MemberSpecificationBuilder<TModel, TMember>;
        }
    }
}