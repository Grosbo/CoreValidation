using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DoubleNullableRules
    {
        public static IMemberSpecification<TModel, double?> CloseTo<TModel>(this IMemberSpecification<TModel, double?> @this, double value, double tolerance = 0.0000001f, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.CloseTo(value, tolerance, message));
        }

        public static IMemberSpecification<TModel, double?> NotCloseTo<TModel>(this IMemberSpecification<TModel, double?> @this, double value, double tolerance = 0.0000001f, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotCloseTo(value, tolerance, message));
        }

        public static IMemberSpecification<TModel, double?> GreaterThan<TModel>(this IMemberSpecification<TModel, double?> @this, double min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecification<TModel, double?> LessThan<TModel>(this IMemberSpecification<TModel, double?> @this, double max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecification<TModel, double?> Between<TModel>(this IMemberSpecification<TModel, double?> @this, double min, double max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }
    }
}