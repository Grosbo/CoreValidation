using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class CharNumbersNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, char?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char?> @this, char value)
            where TModel : class
        {
            return @this.AsNullable(m => m.EqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, char?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char?> @this, char value)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotEqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, char?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, char?> @this, char min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterThan(min));
        }

        public static IMemberSpecificationBuilder<TModel, char?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char?> @this, char min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterOrEqualTo(min));
        }

        public static IMemberSpecificationBuilder<TModel, char?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, char?> @this, char max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessThan(max));
        }

        public static IMemberSpecificationBuilder<TModel, char?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char?> @this, char max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessOrEqualTo(max));
        }

        public static IMemberSpecificationBuilder<TModel, char?> Between<TModel>(this IMemberSpecificationBuilder<TModel, char?> @this, char min, char max)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max));
        }

        public static IMemberSpecificationBuilder<TModel, char?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char?> @this, char min, char max)
            where TModel : class
        {
            return @this.AsNullable(m => m.BetweenOrEqualTo(min, max));
        }
    }
}