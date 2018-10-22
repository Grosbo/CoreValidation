using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ShortNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, short?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short value)
            where TModel : class
        {
            return @this.AsNullable(m => m.EqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, short?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short value)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotEqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, short?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterThan(min));
        }

        public static IMemberSpecificationBuilder<TModel, short?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterOrEqualTo(min));
        }

        public static IMemberSpecificationBuilder<TModel, short?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessThan(max));
        }

        public static IMemberSpecificationBuilder<TModel, short?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessOrEqualTo(max));
        }

        public static IMemberSpecificationBuilder<TModel, short?> Between<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short min, short max)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max));
        }

        public static IMemberSpecificationBuilder<TModel, short?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short min, short max)
            where TModel : class
        {
            return @this.AsNullable(m => m.BetweenOrEqualTo(min, max));
        }
    }
}