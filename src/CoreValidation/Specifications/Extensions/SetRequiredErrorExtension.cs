using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class SetRequiredErrorExtension
    {
        public static IMemberSpecificationBuilder<TModel, TMember> SetRequired<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember> builder, string errorMessage)
            where TModel : class
            where TMember : class
        {
            return ((MemberSpecificationBuilder<TModel, TMember>)builder).SetRequired(errorMessage);
        }

        public static IMemberSpecificationBuilder<TModel, TMember?> SetRequired<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember?> builder, string errorMessage)
            where TModel : class
            where TMember : struct
        {
            return ((MemberSpecificationBuilder<TModel, TMember?>)builder).SetRequired(errorMessage);
        }
    }
}