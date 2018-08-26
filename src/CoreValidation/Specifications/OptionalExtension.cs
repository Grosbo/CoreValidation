using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class OptionalRuleExtension
    {
        public static IMemberSpecificationBuilder<TModel, TMember> Optional<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember> memberSpecificationBuilder)
            where TModel : class
            where TMember : class
        {
            return ((MemberSpecificationBuilder<TModel, TMember>) memberSpecificationBuilder).Optional();
        }

        public static IMemberSpecificationBuilder<TModel, TMember?> Optional<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember?> memberSpecificationBuilder)
            where TModel : class
            where TMember : struct
        {
            return ((MemberSpecificationBuilder<TModel, TMember?>) memberSpecificationBuilder).Optional();
        }
    }
}