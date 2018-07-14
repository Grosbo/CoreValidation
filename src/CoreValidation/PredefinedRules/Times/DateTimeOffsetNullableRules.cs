using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DateTimeOffsetNullableRules
    {
        public static IMemberSpecification<TModel, DateTimeOffset?> EqualTo<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset value, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> NotEqualTo<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset value, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> AfterOrEqualTo<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset min, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.AfterOrEqualTo(min, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> After<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset min, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.After(min, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> BeforeOrEqualTo<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BeforeOrEqualTo(max, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> Before<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Before(max, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> Between<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset min, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset min, DateTimeOffset max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> AfterNow<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, TimeNowMode nowMode = TimeNowMode.Now, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.AfterNow(nowMode, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> BeforeNow<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, TimeNowMode nowMode = TimeNowMode.Now, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BeforeNow(nowMode, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> FromNow<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, TimeSpan timeSpan, TimeNowMode nowMode = TimeNowMode.Now, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.FromNow(timeSpan, nowMode, message));
        }
    }
}