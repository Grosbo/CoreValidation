using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    internal static class DateTimeOffsetNullableParametrizedNowRules
    {
        internal static IMemberSpecification<TModel, DateTimeOffset?> ParametrizedAfterNow<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset now, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.ParametrizedAfterNow(now, timeComparison, message));
        }

        internal static IMemberSpecification<TModel, DateTimeOffset?> ParametrizedBeforeNow<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset now, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.ParametrizedBeforeNow(now, timeComparison, message));
        }

        internal static IMemberSpecification<TModel, DateTimeOffset?> ParametrizedFromNow<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset now, TimeSpan timeSpan, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.ParametrizedFromNow(now, timeSpan, message));
        }
    }
}