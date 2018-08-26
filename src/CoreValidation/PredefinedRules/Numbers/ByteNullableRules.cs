using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ByteNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, byte?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, byte?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, byte?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecificationBuilder<TModel, byte?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecificationBuilder<TModel, byte?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecificationBuilder<TModel, byte?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecificationBuilder<TModel, byte?> Between<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte min, byte max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecificationBuilder<TModel, byte?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte?> @this, byte min, byte max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}