using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class IntNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, int?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int value)
            where TModel : class
        {
            return @this.AsNullable(m => m.EqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, int?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int value)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotEqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, int?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterThan(min));
        }

        public static IMemberSpecificationBuilder<TModel, int?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterOrEqualTo(min));
        }

        public static IMemberSpecificationBuilder<TModel, int?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessThan(max));
        }

        public static IMemberSpecificationBuilder<TModel, int?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessOrEqualTo(max));
        }

        public static IMemberSpecificationBuilder<TModel, int?> Between<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int min, int max)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max));
        }

        public static IMemberSpecificationBuilder<TModel, int?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int?> @this, int min, int max)
            where TModel : class
        {
            return @this.AsNullable(m => m.BetweenOrEqualTo(min, max));
        }
    }
}