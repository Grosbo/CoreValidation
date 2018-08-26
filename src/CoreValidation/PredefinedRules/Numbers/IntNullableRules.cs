using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class IntNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, int?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, int?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, int?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecificationBuilder<TModel, int?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecificationBuilder<TModel, int?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecificationBuilder<TModel, int?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecificationBuilder<TModel, int?> Between<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecificationBuilder<TModel, int?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}