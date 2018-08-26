using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ULongNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, ulong?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, ulong?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecificationBuilder<TModel, ulong?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecificationBuilder<TModel, ulong?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecificationBuilder<TModel, ulong?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecificationBuilder<TModel, ulong?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecificationBuilder<TModel, ulong?> Between<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong min, ulong max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecificationBuilder<TModel, ulong?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong?> @this, ulong min, ulong max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}