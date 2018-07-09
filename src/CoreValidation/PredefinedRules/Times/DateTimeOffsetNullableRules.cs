﻿using System;
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

        public static IMemberSpecification<TModel, DateTimeOffset?> AfterOrEqualTo<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset value, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.AfterOrEqualTo(value, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> After<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset value, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.After(value, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> BeforeOrEqualTo<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset value, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BeforeOrEqualTo(value, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> Before<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset value, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Before(value, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> Between<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset minimum, DateTimeOffset maximum, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(minimum, maximum, timeComparison, message));
        }

        public static IMemberSpecification<TModel, DateTimeOffset?> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, DateTimeOffset?> @this, DateTimeOffset minimum, DateTimeOffset maximum, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(minimum, maximum, timeComparison, message));
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