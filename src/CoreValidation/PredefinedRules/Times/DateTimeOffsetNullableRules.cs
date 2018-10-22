using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DateTimeOffsetNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, DateTimeOffset?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset?> @this, DateTimeOffset value, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.EqualTo(value, timeComparison));
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset?> @this, DateTimeOffset value, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.NotEqualTo(value, timeComparison));
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset?> AfterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset?> @this, DateTimeOffset min, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.AfterOrEqualTo(min, timeComparison));
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset?> After<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset?> @this, DateTimeOffset min, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.After(min, timeComparison));
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset?> BeforeOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset?> @this, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.BeforeOrEqualTo(max, timeComparison));
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset?> Before<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset?> @this, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.Before(max, timeComparison));
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset?> Between<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset?> @this, DateTimeOffset min, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.Between(min, max, timeComparison));
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset?> @this, DateTimeOffset min, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.AsNullable(m => m.BetweenOrEqualTo(min, max, timeComparison));
        }
    }
}