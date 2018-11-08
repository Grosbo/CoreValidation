using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DateTimeRules
    {
        public static IMemberSpecificationBuilder<TModel, DateTime> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime value, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, value, timeComparison) == 0, Phrases.Keys.Times.EqualTo, new[] {Arg.Time(nameof(value), value), Arg.Enum(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime value, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, value, timeComparison) != 0, Phrases.Keys.Times.NotEqualTo, new[] {Arg.Time(nameof(value), value), Arg.Enum(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> After<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime min, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, min, timeComparison) > 0, Phrases.Keys.Times.After, new[] {Arg.Time(nameof(min), min), Arg.Enum(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> AfterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime min, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, min, timeComparison) >= 0, Phrases.Keys.Times.AfterOrEqualTo, new[] {Arg.Time(nameof(min), min), Arg.Enum(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> Before<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, max, timeComparison) < 0, Phrases.Keys.Times.Before, new[] {Arg.Time(nameof(max), max), Arg.Enum(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> BeforeOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, max, timeComparison) <= 0, Phrases.Keys.Times.BeforeOrEqualTo, new[] {Arg.Time(nameof(max), max), Arg.Enum(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> Between<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime min, DateTime max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => (TimeComparer.Compare(m, min, timeComparison) > 0) && (TimeComparer.Compare(m, max, timeComparison) < 0), Phrases.Keys.Times.Between, new[] {Arg.Time(nameof(min), min), Arg.Time(nameof(max), max), Arg.Enum(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime min, DateTime max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => (TimeComparer.Compare(m, min, timeComparison) >= 0) && (TimeComparer.Compare(m, max, timeComparison) <= 0), Phrases.Keys.Times.BetweenOrEqualTo, new[] {Arg.Time(nameof(min), min), Arg.Time(nameof(max), max), Arg.Enum(nameof(timeComparison), timeComparison)});
        }
    }
}