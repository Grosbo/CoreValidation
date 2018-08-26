using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DecimalNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, decimal?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, decimal?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, decimal?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecificationBuilder<TModel, decimal?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecificationBuilder<TModel, decimal?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecificationBuilder<TModel, decimal?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecificationBuilder<TModel, decimal?> Between<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal min, decimal max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecificationBuilder<TModel, decimal?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal min, decimal max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}