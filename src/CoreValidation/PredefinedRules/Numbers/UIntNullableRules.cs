using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class UIntNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, uint?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, uint?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, uint?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecificationBuilder<TModel, uint?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecificationBuilder<TModel, uint?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecificationBuilder<TModel, uint?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecificationBuilder<TModel, uint?> Between<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint min, uint max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecificationBuilder<TModel, uint?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint?> @this, uint min, uint max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}