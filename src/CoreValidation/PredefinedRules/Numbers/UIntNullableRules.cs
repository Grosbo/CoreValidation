using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class UIntNullableRules
    {
        public static IMemberSpecification<TModel, uint?> EqualTo<TModel>(this IMemberSpecification<TModel, uint?> @this, uint value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecification<TModel, uint?> NotEqualTo<TModel>(this IMemberSpecification<TModel, uint?> @this, uint value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecification<TModel, uint?> GreaterThan<TModel>(this IMemberSpecification<TModel, uint?> @this, uint min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecification<TModel, uint?> GreaterOrEqualTo<TModel>(this IMemberSpecification<TModel, uint?> @this, uint min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecification<TModel, uint?> LessThan<TModel>(this IMemberSpecification<TModel, uint?> @this, uint max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecification<TModel, uint?> LessOrEqualTo<TModel>(this IMemberSpecification<TModel, uint?> @this, uint max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecification<TModel, uint?> Between<TModel>(this IMemberSpecification<TModel, uint?> @this, uint min, uint max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecification<TModel, uint?> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, uint?> @this, uint min, uint max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}