using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class BoolNullableRules
    {
        public static IMemberSpecification<TModel, bool?> True<TModel>(this IMemberSpecification<TModel, bool?> @this, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.True(message));
        }

        public static IMemberSpecification<TModel, bool?> False<TModel>(this IMemberSpecification<TModel, bool?> @this, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.False(message));
        }
    }
}