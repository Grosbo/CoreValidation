using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class CharNumbersNullableRules
    {
        public static IMemberSpecification<TModel, char?> EqualTo<TModel>(this IMemberSpecification<TModel, char?> @this, char value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecification<TModel, char?> NotEqualTo<TModel>(this IMemberSpecification<TModel, char?> @this, char value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecification<TModel, char?> GreaterThan<TModel>(this IMemberSpecification<TModel, char?> @this, char min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecification<TModel, char?> GreaterOrEqualTo<TModel>(this IMemberSpecification<TModel, char?> @this, char min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecification<TModel, char?> LessThan<TModel>(this IMemberSpecification<TModel, char?> @this, char max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecification<TModel, char?> LessOrEqualTo<TModel>(this IMemberSpecification<TModel, char?> @this, char max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecification<TModel, char?> Between<TModel>(this IMemberSpecification<TModel, char?> @this, char min, char max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecification<TModel, char?> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, char?> @this, char min, char max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}