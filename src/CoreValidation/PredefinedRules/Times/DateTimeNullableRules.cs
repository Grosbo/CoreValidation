using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DateTimeNullableRules
    {
        public static IMemberSpecification<TModel, DateTime?> EqualTo<TModel>(this IMemberSpecification<TModel, DateTime?> @this, DateTime value, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTime?> NotEqualTo<TModel>(this IMemberSpecification<TModel, DateTime?> @this, DateTime value, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTime?> AfterOrEqualTo<TModel>(this IMemberSpecification<TModel, DateTime?> @this, DateTime min, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.AfterOrEqualTo(min, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTime?> After<TModel>(this IMemberSpecification<TModel, DateTime?> @this, DateTime min, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.After(min, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTime?> BeforeOrEqualTo<TModel>(this IMemberSpecification<TModel, DateTime?> @this, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BeforeOrEqualTo(max, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTime?> Before<TModel>(this IMemberSpecification<TModel, DateTime?> @this, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Before(max, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTime?> Between<TModel>(this IMemberSpecification<TModel, DateTime?> @this, DateTime min, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTime?> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, DateTime?> @this, DateTime min, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTime?> AfterNow<TModel>(this IMemberSpecification<TModel, DateTime?> @this, TimeNowMode nowMode = TimeNowMode.Now, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.AfterNow(nowMode, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTime?> BeforeNow<TModel>(this IMemberSpecification<TModel, DateTime?> @this, TimeNowMode nowMode = TimeNowMode.Now, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BeforeNow(nowMode, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTime?> FromNow<TModel>(this IMemberSpecification<TModel, DateTime?> @this, TimeSpan timeSpan, TimeNowMode nowMode = TimeNowMode.Now, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.FromNow(timeSpan, nowMode));
        }
    }
}