using System;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DateTimeRules
    {
        public static IMemberSpecificationBuilder<TModel, DateTime> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime value, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, value, timeComparison) == 0, Phrases.Keys.Times.EqualTo, new IMessageArg[] {TimeArg.Create(nameof(value), value), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime value, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, value, timeComparison) != 0, Phrases.Keys.Times.NotEqualTo, new IMessageArg[] {TimeArg.Create(nameof(value), value), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> After<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime min, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, min, timeComparison) > 0, Phrases.Keys.Times.After, new IMessageArg[] {TimeArg.Create(nameof(min), min), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> AfterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime min, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, min, timeComparison) >= 0, Phrases.Keys.Times.AfterOrEqualTo, new IMessageArg[] {TimeArg.Create(nameof(min), min), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> Before<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, max, timeComparison) < 0, Phrases.Keys.Times.Before, new IMessageArg[] {TimeArg.Create(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> BeforeOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, max, timeComparison) <= 0, Phrases.Keys.Times.BeforeOrEqualTo, new IMessageArg[] {TimeArg.Create(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> Between<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime min, DateTime max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => (TimeComparer.Compare(m, min, timeComparison) > 0) && (TimeComparer.Compare(m, max, timeComparison) < 0), Phrases.Keys.Times.Between, new IMessageArg[] {TimeArg.Create(nameof(min), min), TimeArg.Create(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTime> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime> @this, DateTime min, DateTime max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => (TimeComparer.Compare(m, min, timeComparison) >= 0) && (TimeComparer.Compare(m, max, timeComparison) <= 0), Phrases.Keys.Times.BetweenOrEqualTo, new IMessageArg[] {TimeArg.Create(nameof(min), min), TimeArg.Create(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }
    }
}