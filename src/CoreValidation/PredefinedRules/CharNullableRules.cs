
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class CharNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, char?> EqualIgnoreCase<TModel>(this IMemberSpecificationBuilder<TModel, char?> @this, char value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualIgnoreCase(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, char?> NotEqualIgnoreCase<TModel>(this IMemberSpecificationBuilder<TModel, char?> @this, char value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualIgnoreCase(value, message));
        }
    }
}