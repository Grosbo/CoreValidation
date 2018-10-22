using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ByteNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, byte?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte value)
            where TModel : class
        {
            return @this.AsNullable(m => m.EqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, byte?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte value)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotEqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, byte?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterThan(min));
        }

        public static IMemberSpecificationBuilder<TModel, byte?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterOrEqualTo(min));
        }

        public static IMemberSpecificationBuilder<TModel, byte?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessThan(max));
        }

        public static IMemberSpecificationBuilder<TModel, byte?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessOrEqualTo(max));
        }

        public static IMemberSpecificationBuilder<TModel, byte?> Between<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte min, byte max)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max));
        }

        public static IMemberSpecificationBuilder<TModel, byte?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte min, byte max)
            where TModel : class
        {
            return @this.AsNullable(m => m.BetweenOrEqualTo(min, max));
        }
    }
}