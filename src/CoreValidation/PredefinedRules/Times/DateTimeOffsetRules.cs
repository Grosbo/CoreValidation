using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DateTimeOffsetRules
    {
        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset value, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, value, timeComparison) == 0, Phrases.Keys.Times.EqualTo, new[] {Arg.Time(nameof(value), value), Arg.Enum(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset value, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, value, timeComparison) != 0, Phrases.Keys.Times.NotEqualTo, new[] {Arg.Time(nameof(value), value), Arg.Enum(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> After<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset min, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, min, timeComparison) > 0, Phrases.Keys.Times.After, new[] {Arg.Time(nameof(min), min), Arg.Enum(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> AfterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset min, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, min, timeComparison) >= 0, Phrases.Keys.Times.AfterOrEqualTo, new[] {Arg.Time(nameof(min), min), Arg.Enum(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> Before<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, max, timeComparison) < 0, Phrases.Keys.Times.Before, new[] {Arg.Time(nameof(max), max), Arg.Enum(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> BeforeOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, max, timeComparison) <= 0, Phrases.Keys.Times.BeforeOrEqualTo, new[] {Arg.Time(nameof(max), max), Arg.Enum(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> Between<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset min, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => (TimeComparer.Compare(m, min, timeComparison) > 0) && (TimeComparer.Compare(m, max, timeComparison) < 0), Phrases.Keys.Times.Between, new[] {Arg.Time(nameof(min), min), Arg.Time(nameof(max), max), Arg.Enum(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset min, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => (TimeComparer.Compare(m, min, timeComparison) >= 0) && (TimeComparer.Compare(m, max, timeComparison) <= 0), Phrases.Keys.Times.BetweenOrEqualTo, new[] {Arg.Time(nameof(min), min), Arg.Time(nameof(max), max), Arg.Enum(nameof(timeComparison), timeComparison)});
        }
    }
}