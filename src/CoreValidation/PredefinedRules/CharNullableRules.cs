
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class CharNullableRules
    {
        public static IMemberSpecification<TModel, char?> EqualIgnoreCase<TModel>(this IMemberSpecification<TModel, char?> @this, char value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualIgnoreCase(value, message));
        }

        public static IMemberSpecification<TModel, char?> NotEqualIgnoreCase<TModel>(this IMemberSpecification<TModel, char?> @this, char value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualIgnoreCase(value, message));
        }
    }
}