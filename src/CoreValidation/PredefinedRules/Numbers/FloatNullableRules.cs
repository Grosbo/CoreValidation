using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class FloatNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, float?> CloseTo<TModel>(this IMemberSpecificationBuilder<TModel, float?> @this, float value, float tolerance = 0.0000001f)
            where TModel : class
        {
            return @this.AsNullable(m => m.CloseTo(value, tolerance));
        }

        public static IMemberSpecificationBuilder<TModel, float?> NotCloseTo<TModel>(this IMemberSpecificationBuilder<TModel, float?> @this, float value, float tolerance = 0.0000001f)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotCloseTo(value, tolerance));
        }

        public static IMemberSpecificationBuilder<TModel, float?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, float?> @this, float min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterThan(min));
        }

        public static IMemberSpecificationBuilder<TModel, float?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, float?> @this, float max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessThan(max));
        }

        public static IMemberSpecificationBuilder<TModel, float?> Between<TModel>(this IMemberSpecificationBuilder<TModel, float?> @this, float min, float max)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max));
        }
    }
}