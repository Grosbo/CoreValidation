using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    internal static class DateTimeOffsetNullableParametrizedNowRules
    {
        internal static IMemberSpecificationBuilder<TModel, DateTimeOffset?> ParametrizedAfterNow<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset?> @this, DateTimeOffset now, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.ParametrizedAfterNow(now, timeComparison, message));
        }

        internal static IMemberSpecificationBuilder<TModel, DateTimeOffset?> ParametrizedBeforeNow<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset?> @this, DateTimeOffset now, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.ParametrizedBeforeNow(now, timeComparison, message));
        }

        internal static IMemberSpecificationBuilder<TModel, DateTimeOffset?> ParametrizedFromNow<TModel>(this IMemberSpecificationBuilder<TModel, DateTimeOffset?> @this, DateTimeOffset now, TimeSpan timeSpan, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.ParametrizedFromNow(now, timeSpan, message));
        }
    }
}