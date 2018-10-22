using System;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DateTimeOffsetRules
    {
        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset value, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, value, timeComparison) == 0, Phrases.Keys.Times.EqualTo, new IMessageArg[] {TimeArg.Create(nameof(value), value), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset value, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, value, timeComparison) != 0, Phrases.Keys.Times.NotEqualTo, new IMessageArg[] {TimeArg.Create(nameof(value), value), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> After<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset min, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, min, timeComparison) > 0, Phrases.Keys.Times.After, new IMessageArg[] {TimeArg.Create(nameof(min), min), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> AfterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset min, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, min, timeComparison) >= 0, Phrases.Keys.Times.AfterOrEqualTo, new IMessageArg[] {TimeArg.Create(nameof(min), min), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> Before<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, max, timeComparison) < 0, Phrases.Keys.Times.Before, new IMessageArg[] {TimeArg.Create(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> BeforeOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, max, timeComparison) <= 0, Phrases.Keys.Times.BeforeOrEqualTo, new IMessageArg[] {TimeArg.Create(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> Between<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset min, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => (TimeComparer.Compare(m, min, timeComparison) > 0) && (TimeComparer.Compare(m, max, timeComparison) < 0), Phrases.Keys.Times.Between, new IMessageArg[] {TimeArg.Create(nameof(min), min), TimeArg.Create(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, DateTimeOffset> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset> @this, DateTimeOffset min, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All)
            where TModel : class
        {
            return @this.Valid(m => (TimeComparer.Compare(m, min, timeComparison) >= 0) && (TimeComparer.Compare(m, max, timeComparison) <= 0), Phrases.Keys.Times.BetweenOrEqualTo, new IMessageArg[] {TimeArg.Create(nameof(min), min), TimeArg.Create(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }
    }
}