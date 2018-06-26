using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class IntNullableRules
    {
        public static IMemberSpecification<TModel, int?> EqualTo<TModel>(this IMemberSpecification<TModel, int?> @this, int value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecification<TModel, int?> NotEqualTo<TModel>(this IMemberSpecification<TModel, int?> @this, int value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecification<TModel, int?> GreaterThan<TModel>(this IMemberSpecification<TModel, int?> @this, int min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecification<TModel, int?> GreaterOrEqualTo<TModel>(this IMemberSpecification<TModel, int?> @this, int min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecification<TModel, int?> LessThan<TModel>(this IMemberSpecification<TModel, int?> @this, int max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecification<TModel, int?> LessOrEqualTo<TModel>(this IMemberSpecification<TModel, int?> @this, int max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecification<TModel, int?> Between<TModel>(this IMemberSpecification<TModel, int?> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecification<TModel, int?> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, int?> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}