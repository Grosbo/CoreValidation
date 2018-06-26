using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class SByteNullableRules
    {
        public static IMemberSpecification<TModel, sbyte?> EqualTo<TModel>(this IMemberSpecification<TModel, sbyte?> @this, sbyte value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecification<TModel, sbyte?> NotEqualTo<TModel>(this IMemberSpecification<TModel, sbyte?> @this, sbyte value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecification<TModel, sbyte?> GreaterThan<TModel>(this IMemberSpecification<TModel, sbyte?> @this, sbyte min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecification<TModel, sbyte?> GreaterOrEqualTo<TModel>(this IMemberSpecification<TModel, sbyte?> @this, sbyte min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecification<TModel, sbyte?> LessThan<TModel>(this IMemberSpecification<TModel, sbyte?> @this, sbyte max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecification<TModel, sbyte?> LessOrEqualTo<TModel>(this IMemberSpecification<TModel, sbyte?> @this, sbyte max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecification<TModel, sbyte?> Between<TModel>(this IMemberSpecification<TModel, sbyte?> @this, sbyte min, sbyte max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecification<TModel, sbyte?> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, sbyte?> @this, sbyte min, sbyte max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}