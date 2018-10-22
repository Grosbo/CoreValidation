using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class BoolNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, bool?> True<TModel>(this IMemberSpecificationBuilder<TModel, bool?> @this)
            where TModel : class
        {
            return @this.AsNullable(m => m.True());
        }

        public static IMemberSpecificationBuilder<TModel, bool?> False<TModel>(this IMemberSpecificationBuilder<TModel, bool?> @this)
            where TModel : class
        {
            return @this.AsNullable(m => m.False());
        }
    }
}