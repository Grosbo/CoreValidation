using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DoubleNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, double?> CloseTo<TModel>(this IMemberSpecificationBuilder<TModel, double?> @this, double value, double tolerance = 0.0000001f)
            where TModel : class
        {
            return @this.AsNullable(m => m.CloseTo(value, tolerance));
        }

        public static IMemberSpecificationBuilder<TModel, double?> NotCloseTo<TModel>(this IMemberSpecificationBuilder<TModel, double?> @this, double value, double tolerance = 0.0000001f)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotCloseTo(value, tolerance));
        }

        public static IMemberSpecificationBuilder<TModel, double?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, double?> @this, double min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterThan(min));
        }

        public static IMemberSpecificationBuilder<TModel, double?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, double?> @this, double max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessThan(max));
        }

        public static IMemberSpecificationBuilder<TModel, double?> Between<TModel>(this IMemberSpecificationBuilder<TModel, double?> @this, double min, double max)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max));
        }
    }
}