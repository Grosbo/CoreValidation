using System;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    internal static class DateTimeOffsetParametrizedNowRules
    {
        internal static IMemberSpecification<TModel, DateTime> ParametrizedAfterNow<TModel>(this IMemberSpecification<TModel, DateTime> @this, DateTime now, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, now, timeComparison) > 0, message ?? Phrases.Keys.Times.AfterNow, new IMessageArg[] {new NumberArg(nameof(now), now), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        internal static IMemberSpecification<TModel, DateTime> ParametrizedBeforeNow<TModel>(this IMemberSpecification<TModel, DateTime> @this, DateTime now, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.Valid(m => TimeComparer.Compare(m, now, timeComparison) < 0, message ?? Phrases.Keys.Times.BeforeNow, new IMessageArg[] {new NumberArg(nameof(now), now), new EnumArg<TimeComparison>(nameof(timeComparison), timeComparison)});
        }

        internal static IMemberSpecification<TModel, DateTime> ParametrizedFromNow<TModel>(this IMemberSpecification<TModel, DateTime> @this, DateTime now, TimeSpan timeSpan, string message = null)
            where TModel : class
        {
            return @this.Valid(m => timeSpan.Ticks < 0
                    ? (now.Ticks + timeSpan.Ticks) <= m.Ticks
                    : (now.Ticks + timeSpan.Ticks) >= m.Ticks, message ?? Phrases.Keys.Times.FromNow, new IMessageArg[] {new NumberArg(nameof(timeSpan), timeSpan), new NumberArg(nameof(now), now)});
        }
    }
}