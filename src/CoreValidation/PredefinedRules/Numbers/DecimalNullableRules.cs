using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DecimalNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, decimal?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal value)
            where TModel : class
        {
            return @this.AsNullable(m => m.EqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, decimal?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal value)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotEqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, decimal?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterThan(min));
        }

        public static IMemberSpecificationBuilder<TModel, decimal?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterOrEqualTo(min));
        }

        public static IMemberSpecificationBuilder<TModel, decimal?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessThan(max));
        }

        public static IMemberSpecificationBuilder<TModel, decimal?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessOrEqualTo(max));
        }

        public static IMemberSpecificationBuilder<TModel, decimal?> Between<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal min, decimal max)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max));
        }

        public static IMemberSpecificationBuilder<TModel, decimal?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal?> @this, decimal min, decimal max)
            where TModel : class
        {
            return @this.AsNullable(m => m.BetweenOrEqualTo(min, max));
        }
    }
}