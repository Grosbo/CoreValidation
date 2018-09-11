using System;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DateTimeRules
    {
        public static IMemberSpecificationBuilder<TModel, DateTime> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime value, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, value, timeComparison) == 0, message ?? Phrases.Keys.Times.EqualTo, new IMessageArg[] {new NumberArg(nameof(value), value), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime value, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, value, timeComparison) != 0, message ?? Phrases.Keys.Times.NotEqualTo, new IMessageArg[] {new NumberArg(nameof(value), value), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> After<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime min, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, min, timeComparison) > 0, message ?? Phrases.Keys.Times.After, new IMessageArg[] {new NumberArg(nameof(min), min), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> AfterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime min, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, min, timeComparison) >= 0, message ?? Phrases.Keys.Times.AfterOrEqualTo, new IMessageArg[] {new NumberArg(nameof(min), min), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> Before<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, max, timeComparison) < 0, message ?? Phrases.Keys.Times.Before, new IMessageArg[] {new NumberArg(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> BeforeOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, max, timeComparison) <= 0, message ?? Phrases.Keys.Times.BeforeOrEqualTo, new IMessageArg[] {new NumberArg(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> Between<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime min, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (TimeComparer.Compare(m, min, timeComparison) > 0) && (TimeComparer.Compare(m, max, timeComparison) < 0), message ?? Phrases.Keys.Times.Between, new IMessageArg[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime min, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (TimeComparer.Compare(m, min, timeComparison) >= 0) && (TimeComparer.Compare(m, max, timeComparison) <= 0), message ?? Phrases.Keys.Times.BetweenOrEqualTo, new IMessageArg[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }
    }
}