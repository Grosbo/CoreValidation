using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class UIntNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, uint?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint value)
            where TModel : class
        {
            return @this.AsNullable(m => m.EqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, uint?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint value)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotEqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, uint?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterThan(min));
        }

        public static IMemberSpecificationBuilder<TModel, uint?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterOrEqualTo(min));
        }

        public static IMemberSpecificationBuilder<TModel, uint?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessThan(max));
        }

        public static IMemberSpecificationBuilder<TModel, uint?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessOrEqualTo(max));
        }

        public static IMemberSpecificationBuilder<TModel, uint?> Between<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint min, uint max)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max));
        }

        public static IMemberSpecificationBuilder<TModel, uint?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint min, uint max)
            where TModel : class
        {
            return @this.AsNullable(m => m.BetweenOrEqualTo(min, max));
        }
    }
}