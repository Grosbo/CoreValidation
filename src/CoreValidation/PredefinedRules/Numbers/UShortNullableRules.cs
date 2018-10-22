using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class UShortNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, ushort?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort?> @this, ushort value)
            where TModel : class
        {
            return @this.AsNullable(m => m.EqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, ushort?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort?> @this, ushort value)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotEqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, ushort?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, ushort?> @this, ushort min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterThan(min));
        }

        public static IMemberSpecificationBuilder<TModel, ushort?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort?> @this, ushort min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterOrEqualTo(min));
        }

        public static IMemberSpecificationBuilder<TModel, ushort?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, ushort?> @this, ushort max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessThan(max));
        }

        public static IMemberSpecificationBuilder<TModel, ushort?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort?> @this, ushort max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessOrEqualTo(max));
        }

        public static IMemberSpecificationBuilder<TModel, ushort?> Between<TModel>(this IMemberSpecificationBuilder<TModel, ushort?> @this, ushort min, ushort max)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max));
        }

        public static IMemberSpecificationBuilder<TModel, ushort?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort?> @this, ushort min, ushort max)
            where TModel : class
        {
            return @this.AsNullable(m => m.BetweenOrEqualTo(min, max));
        }
    }
}