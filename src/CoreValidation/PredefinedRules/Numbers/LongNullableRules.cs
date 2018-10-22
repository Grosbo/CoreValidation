using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class LongNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, long?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long value)
            where TModel : class
        {
            return @this.AsNullable(m => m.EqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, long?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long value)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotEqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, long?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterThan(min));
        }

        public static IMemberSpecificationBuilder<TModel, long?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterOrEqualTo(min));
        }

        public static IMemberSpecificationBuilder<TModel, long?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessThan(max));
        }

        public static IMemberSpecificationBuilder<TModel, long?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessOrEqualTo(max));
        }

        public static IMemberSpecificationBuilder<TModel, long?> Between<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long min, long max)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max));
        }

        public static IMemberSpecificationBuilder<TModel, long?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long?> @this, long min, long max)
            where TModel : class
        {
            return @this.AsNullable(m => m.BetweenOrEqualTo(min, max));
        }
    }
}