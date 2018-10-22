using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class SByteNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, sbyte?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte?> @this, sbyte value)
            where TModel : class
        {
            return @this.AsNullable(m => m.EqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, sbyte?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte?> @this, sbyte value)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotEqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, sbyte?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, sbyte?> @this, sbyte min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterThan(min));
        }

        public static IMemberSpecificationBuilder<TModel, sbyte?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte?> @this, sbyte min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterOrEqualTo(min));
        }

        public static IMemberSpecificationBuilder<TModel, sbyte?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, sbyte?> @this, sbyte max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessThan(max));
        }

        public static IMemberSpecificationBuilder<TModel, sbyte?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte?> @this, sbyte max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessOrEqualTo(max));
        }

        public static IMemberSpecificationBuilder<TModel, sbyte?> Between<TModel>(this IMemberSpecificationBuilder<TModel, sbyte?> @this, sbyte min, sbyte max)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max));
        }

        public static IMemberSpecificationBuilder<TModel, sbyte?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte?> @this, sbyte min, sbyte max)
            where TModel : class
        {
            return @this.AsNullable(m => m.BetweenOrEqualTo(min, max));
        }
    }
}