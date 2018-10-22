using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DateTimeNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, DateTime?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime value, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.EqualTo(value, timeComparison));
        }

        public static IMemberSpecificationBuilder<TModel, DateTime?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime value, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotEqualTo(value, timeComparison));
        }

        public static IMemberSpecificationBuilder<TModel, DateTime?> AfterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime min, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.AfterOrEqualTo(min, timeComparison));
        }

        public static IMemberSpecificationBuilder<TModel, DateTime?> After<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime min, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.After(min, timeComparison));
        }

        public static IMemberSpecificationBuilder<TModel, DateTime?> BeforeOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.BeforeOrEqualTo(max, timeComparison));
        }

        public static IMemberSpecificationBuilder<TModel, DateTime?> Before<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.Before(max, timeComparison));
        }

        public static IMemberSpecificationBuilder<TModel, DateTime?> Between<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime min, DateTime max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max, timeComparison));
        }

        public static IMemberSpecificationBuilder<TModel, DateTime?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime min, DateTime max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.BetweenOrEqualTo(min, max, timeComparison));
        }
    }
}