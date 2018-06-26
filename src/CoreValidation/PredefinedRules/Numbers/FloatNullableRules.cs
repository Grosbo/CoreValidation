using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class FloatNullableRules
    {
        public static IMemberSpecification<TModel, float?> CloseTo<TModel>(this IMemberSpecification<TModel, float?> @this, float value, float tolerance = 0.0000001f, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.CloseTo(value, tolerance, message));
        }

        public static IMemberSpecification<TModel, float?> NotCloseTo<TModel>(this IMemberSpecification<TModel, float?> @this, float value, float tolerance = 0.0000001f, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotCloseTo(value, tolerance, message));
        }

        public static IMemberSpecification<TModel, float?> GreaterThan<TModel>(this IMemberSpecification<TModel, float?> @this, float min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecification<TModel, float?> LessThan<TModel>(this IMemberSpecification<TModel, float?> @this, float max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecification<TModel, float?> Between<TModel>(this IMemberSpecification<TModel, float?> @this, float min, float max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }
    }
}