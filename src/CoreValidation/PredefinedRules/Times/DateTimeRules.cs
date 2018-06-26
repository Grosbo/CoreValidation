using System;

using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DateTimeRules
    {
        public static IMemberSpecification<TModel, DateTime> EqualTo<TModel>(this IMemberSpecification<TModel, DateTime> @this, DateTime value, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, value, timeComparison) == 0, message ?? Phrases.Keys.Times.EqualTo, new IMessageArg[] {new NumberArg(nameof(value), value), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecification<TModel, DateTime> NotEqualTo<TModel>(this IMemberSpecification<TModel, DateTime> @this, DateTime value, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, value, timeComparison) != 0, message ?? Phrases.Keys.Times.NotEqualTo, new IMessageArg[] {new NumberArg(nameof(value), value), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecification<TModel, DateTime> After<TModel>(this IMemberSpecification<TModel, DateTime> @this, DateTime min, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, min, timeComparison) > 0, message ?? Phrases.Keys.Times.After, new IMessageArg[] {new NumberArg(nameof(min), min), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecification<TModel, DateTime> AfterOrEqualTo<TModel>(this IMemberSpecification<TModel, DateTime> @this, DateTime min, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, min, timeComparison) >= 0, message ?? Phrases.Keys.Times.AfterOrEqualTo, new IMessageArg[] {new NumberArg(nameof(min), min), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecification<TModel, DateTime> Before<TModel>(this IMemberSpecification<TModel, DateTime> @this, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, max, timeComparison) < 0, message ?? Phrases.Keys.Times.Before, new IMessageArg[] {new NumberArg(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecification<TModel, DateTime> BeforeOrEqualTo<TModel>(this IMemberSpecification<TModel, DateTime> @this, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, max, timeComparison) <= 0, message ??Phrases.Keys.Times.BeforeOrEqualTo, new IMessageArg[] {new NumberArg(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecification<TModel, DateTime> Between<TModel>(this IMemberSpecification<TModel, DateTime> @this, DateTime min, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, min, timeComparison) > 0 && TimeComparer.Compare(m, max, timeComparison) < 0, message ?? Phrases.Keys.Times.Between, new IMessageArg[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecification<TModel, DateTime> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, DateTime> @this, DateTime min, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, min, timeComparison) >= 0 && TimeComparer.Compare(m, max, timeComparison) <= 0, message ?? Phrases.Keys.Times.BetweenOrEqualTo, new IMessageArg[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        public static IMemberSpecification<TModel, DateTime> AfterNow<TModel>(this IMemberSpecification<TModel, DateTime> @this, TimeNowMode nowMode = TimeNowMode.Now, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ParametrizedAfterNow(GetNow(nowMode), timeComparison, message);
        }

        public static IMemberSpecification<TModel, DateTime> BeforeNow<TModel>(this IMemberSpecification<TModel, DateTime> @this, TimeNowMode nowMode = TimeNowMode.Now, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ParametrizedBeforeNow(GetNow(nowMode), timeComparison, message);
        }

        public static IMemberSpecification<TModel, DateTime> FromNow<TModel>(this IMemberSpecification<TModel, DateTime> @this, TimeSpan timeSpan, TimeNowMode nowMode = TimeNowMode.Now, string message = null)
            where TModel : class
        {
            return @this.ParametrizedFromNow(GetNow(nowMode), timeSpan, message);
        }

        private static DateTime GetNow(TimeNowMode nowMode)
        {
            return nowMode == TimeNowMode.Now ? DateTime.Now : DateTime.UtcNow;
        }
    }
}