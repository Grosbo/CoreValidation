using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class LongNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, long?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, long?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, long?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecificationBuilder<TModel, long?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecificationBuilder<TModel, long?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecificationBuilder<TModel, long?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecificationBuilder<TModel, long?> Between<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecificationBuilder<TModel, long?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}