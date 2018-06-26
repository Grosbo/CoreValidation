using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    internal static class DateTimeNullableParametrizedNowRules
    {
        internal static IMemberSpecification<TModel, DateTime?> ParametrizedAfterNow<TModel>(this IMemberSpecification<TModel, DateTime?> @this, DateTime now, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.ParametrizedAfterNow(now, timeComparison, message));
        }

        internal static IMemberSpecification<TModel, DateTime?> ParametrizedBeforeNow<TModel>(this IMemberSpecification<TModel, DateTime?> @this, DateTime now, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.ParametrizedBeforeNow(now, timeComparison, message));
        }

        internal static IMemberSpecification<TModel, DateTime?> ParametrizedFromNow<TModel>(this IMemberSpecification<TModel, DateTime?> @this, DateTime now, TimeSpan timeSpan, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.ParametrizedFromNow(now, timeSpan, message));
        }
    }
}