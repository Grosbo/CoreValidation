using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class CharNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, char?> EqualIgnoreCase<TModel>(this IMemberSpecificationBuilder<TModel, char?> @this, char value)
            where TModel : class
        {
            return @this.AsNullable(m => m.EqualIgnoreCase(value));
        }

        public static IMemberSpecificationBuilder<TModel, char?> NotEqualIgnoreCase<TModel>(this IMemberSpecificationBuilder<TModel, char?> @this, char value)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotEqualIgnoreCase(value));
        }
    }
}