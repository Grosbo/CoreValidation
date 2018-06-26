using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class UShortNullableRules
    {
        public static IMemberSpecification<TModel, ushort?> EqualTo<TModel>(this IMemberSpecification<TModel, ushort?> @this, ushort value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecification<TModel, ushort?> NotEqualTo<TModel>(this IMemberSpecification<TModel, ushort?> @this, ushort value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecification<TModel, ushort?> GreaterThan<TModel>(this IMemberSpecification<TModel, ushort?> @this, ushort min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecification<TModel, ushort?> GreaterOrEqualTo<TModel>(this IMemberSpecification<TModel, ushort?> @this, ushort min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecification<TModel, ushort?> LessThan<TModel>(this IMemberSpecification<TModel, ushort?> @this, ushort max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecification<TModel, ushort?> LessOrEqualTo<TModel>(this IMemberSpecification<TModel, ushort?> @this, ushort max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecification<TModel, ushort?> Between<TModel>(this IMemberSpecification<TModel, ushort?> @this, ushort min, ushort max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecification<TModel, ushort?> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, ushort?> @this, ushort min, ushort max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}