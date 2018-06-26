using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class TimeSpanNullableRules
    {
        public static IMemberSpecification<TModel, TimeSpan?> EqualTo<TModel>(this IMemberSpecification<TModel, TimeSpan?> @this, TimeSpan value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, message));
        }

        public static IMemberSpecification<TModel, TimeSpan?> NotEqualTo<TModel>(this IMemberSpecification<TModel, TimeSpan?> @this, TimeSpan value, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, message));
        }

        public static IMemberSpecification<TModel, TimeSpan?> GreaterThan<TModel>(this IMemberSpecification<TModel, TimeSpan?> @this, TimeSpan min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterThan(min, message));
        }

        public static IMemberSpecification<TModel, TimeSpan?> GreaterOrEqualTo<TModel>(this IMemberSpecification<TModel, TimeSpan?> @this, TimeSpan min, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.GreaterOrEqualTo(min, message));
        }

        public static IMemberSpecification<TModel, TimeSpan?> LessThan<TModel>(this IMemberSpecification<TModel, TimeSpan?> @this, TimeSpan max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessThan(max, message));
        }

        public static IMemberSpecification<TModel, TimeSpan?> LessOrEqualTo<TModel>(this IMemberSpecification<TModel, TimeSpan?> @this, TimeSpan max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.LessOrEqualTo(max, message));
        }

        public static IMemberSpecification<TModel, TimeSpan?> Between<TModel>(this IMemberSpecification<TModel, TimeSpan?> @this, TimeSpan min, TimeSpan max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, message));
        }

        public static IMemberSpecification<TModel, TimeSpan?> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, TimeSpan?> @this, TimeSpan min, TimeSpan max, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, message));
        }
    }
}