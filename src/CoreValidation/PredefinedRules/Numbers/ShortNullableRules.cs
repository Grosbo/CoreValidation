using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ShortNullableRules
    {
        public static IMemberSpecification<TModel, short?> EqualTo<TModel>(this IMemberSpecification<TModel, short?> @this, short value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecification<TModel, short?> NotEqualTo<TModel>(this IMemberSpecification<TModel, short?> @this, short value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecification<TModel, short?> GreaterThan<TModel>(this IMemberSpecification<TModel, short?> @this, short min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecification<TModel, short?> GreaterOrEqualTo<TModel>(this IMemberSpecification<TModel, short?> @this, short min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecification<TModel, short?> LessThan<TModel>(this IMemberSpecification<TModel, short?> @this, short max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecification<TModel, short?> LessOrEqualTo<TModel>(this IMemberSpecification<TModel, short?> @this, short max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecification<TModel, short?> Between<TModel>(this IMemberSpecification<TModel, short?> @this, short min, short max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecification<TModel, short?> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, short?> @this, short min, short max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}