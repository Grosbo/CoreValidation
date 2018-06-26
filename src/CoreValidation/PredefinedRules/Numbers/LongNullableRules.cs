using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class LongNullableRules
    {
        public static IMemberSpecification<TModel, long?> EqualTo<TModel>(this IMemberSpecification<TModel, long?> @this, long value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecification<TModel, long?> NotEqualTo<TModel>(this IMemberSpecification<TModel, long?> @this, long value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecification<TModel, long?> GreaterThan<TModel>(this IMemberSpecification<TModel, long?> @this, long min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecification<TModel, long?> GreaterOrEqualTo<TModel>(this IMemberSpecification<TModel, long?> @this, long min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecification<TModel, long?> LessThan<TModel>(this IMemberSpecification<TModel, long?> @this, long max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecification<TModel, long?> LessOrEqualTo<TModel>(this IMemberSpecification<TModel, long?> @this, long max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecification<TModel, long?> Between<TModel>(this IMemberSpecification<TModel, long?> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecification<TModel, long?> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, long?> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}