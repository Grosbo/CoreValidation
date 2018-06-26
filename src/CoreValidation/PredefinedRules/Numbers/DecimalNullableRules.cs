using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DecimalNullableRules
    {
        public static IMemberSpecification<TModel, decimal?> EqualTo<TModel>(this IMemberSpecification<TModel, decimal?> @this, decimal value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecification<TModel, decimal?> NotEqualTo<TModel>(this IMemberSpecification<TModel, decimal?> @this, decimal value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecification<TModel, decimal?> GreaterThan<TModel>(this IMemberSpecification<TModel, decimal?> @this, decimal min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecification<TModel, decimal?> GreaterOrEqualTo<TModel>(this IMemberSpecification<TModel, decimal?> @this, decimal min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecification<TModel, decimal?> LessThan<TModel>(this IMemberSpecification<TModel, decimal?> @this, decimal max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecification<TModel, decimal?> LessOrEqualTo<TModel>(this IMemberSpecification<TModel, decimal?> @this, decimal max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecification<TModel, decimal?> Between<TModel>(this IMemberSpecification<TModel, decimal?> @this, decimal min, decimal max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecification<TModel, decimal?> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, decimal?> @this, decimal min, decimal max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}