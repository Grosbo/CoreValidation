using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ShortNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, short?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, short?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, short?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecificationBuilder<TModel, short?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecificationBuilder<TModel, short?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecificationBuilder<TModel, short?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecificationBuilder<TModel, short?> Between<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short min, short max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecificationBuilder<TModel, short?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short?> @this, short min, short max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}