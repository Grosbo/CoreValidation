using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class SetOptionalExtension
    {
        /// <summary>
        /// Sets the member as optional. In case the member value is null, no error is added.
        /// </summary>
        public static IMemberSpecificationBuilder<TModel, TMember> SetOptional<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember> memberSpecificationBuilder)
            where TModel : class
            where TMember : class
        {
            return ((MemberSpecificationBuilder<TModel, TMember>)memberSpecificationBuilder).SetOptional();
        }

        /// <summary>
        /// Sets the member as optional. In case the member value is null, no error is added.
        /// </summary>
        public static IMemberSpecificationBuilder<TModel, TMember?> SetOptional<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember?> memberSpecificationBuilder)
            where TModel : class
            where TMember : struct
        {
            return ((MemberSpecificationBuilder<TModel, TMember?>)memberSpecificationBuilder).SetOptional();
        }
    }
}