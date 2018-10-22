using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class TimeSpanNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, TimeSpan?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan?> @this, TimeSpan value)
            where TModel : class
        {
            return @this.AsNullable(m => m.EqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan?> @this, TimeSpan value)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotEqualTo(value));
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan?> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan?> @this, TimeSpan min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterThan(min));
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan?> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan?> @this, TimeSpan min)
            where TModel : class
        {
            return @this.AsNullable(m => m.GreaterOrEqualTo(min));
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan?> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan?> @this, TimeSpan max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessThan(max));
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan?> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan?> @this, TimeSpan max)
            where TModel : class
        {
            return @this.AsNullable(m => m.LessOrEqualTo(max));
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan?> Between<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan?> @this, TimeSpan min, TimeSpan max)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max));
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan?> @this, TimeSpan min, TimeSpan max)
            where TModel : class
        {
            return @this.AsNullable(m => m.BetweenOrEqualTo(min, max));
        }
    }
}