using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ULongNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, ulong?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong value)
            where TModel : class
        {
            return @this.AsNullable(m => m.EqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, ulong?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong value)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotEqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, ulong?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterThan(min));
        }

        public static IMemberSpecificationBuilder<TModel, ulong?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterOrEqualTo(min));
        }

        public static IMemberSpecificationBuilder<TModel, ulong?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessThan(max));
        }

        public static IMemberSpecificationBuilder<TModel, ulong?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessOrEqualTo(max));
        }

        public static IMemberSpecificationBuilder<TModel, ulong?> Between<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong min, ulong max)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max));
        }

        public static IMemberSpecificationBuilder<TModel, ulong?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong min, ulong max)
            where TModel : class
        {
            return @this.AsNullable(m => m.BetweenOrEqualTo(min, max));
        }
    }
}