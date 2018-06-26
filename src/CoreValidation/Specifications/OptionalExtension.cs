using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class OptionalRuleExtension
    {
        public static IMemberSpecification<TModel, TMember> Optional<TModel, TMember>(this IMemberSpecification<TModel, TMember> memberSpecification)
            where TModel : class
            where TMember : class
        {
            return ((MemberSpecification<TModel, TMember>) memberSpecification).Optional();
        }

        public static IMemberSpecification<TModel, TMember?> Optional<TModel, TMember>(this IMemberSpecification<TModel, TMember?> memberSpecification)
            where TModel : class
            where TMember : struct
        {
            return ((MemberSpecification<TModel, TMember?>) memberSpecification).Optional();
        }
    }
}