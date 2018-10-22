using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class SetOptionalExtension
    {
        public static IMemberSpecificationBuilder<TModel, TMember> SetOptional<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember> memberSpecificationBuilder)
            where TModel : class
            where TMember : class
        {
            return ((MemberSpecificationBuilder<TModel, TMember>)memberSpecificationBuilder).SetOptional();
        }

        public static IMemberSpecificationBuilder<TModel, TMember?> SetOptional<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember?> memberSpecificationBuilder)
            where TModel : class
            where TMember : struct
        {
            return ((MemberSpecificationBuilder<TModel, TMember?>)memberSpecificationBuilder).SetOptional();
        }
    }
}