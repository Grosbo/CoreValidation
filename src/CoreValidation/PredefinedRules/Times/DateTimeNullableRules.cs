using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DateTimeNullableRules
    {
        public static IMemberSpecificationBuilder<TModel, DateTime?> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime value, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.EqualTo(value, timeComparison, message));
        }

        public static IMemberSpecificationBuilder<TModel, DateTime?> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime value, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.NotEqualTo(value, timeComparison, message));
        }

        public static IMemberSpecificationBuilder<TModel, DateTime?> AfterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime min, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.AfterOrEqualTo(min, timeComparison, message));
        }

        public static IMemberSpecificationBuilder<TModel, DateTime?> After<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime min, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.After(min, timeComparison, message));
        }

        public static IMemberSpecificationBuilder<TModel, DateTime?> BeforeOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BeforeOrEqualTo(max, timeComparison, message));
        }

        public static IMemberSpecificationBuilder<TModel, DateTime?> Before<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Before(max, timeComparison, message));
        }

        public static IMemberSpecificationBuilder<TModel, DateTime?> Between<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime min, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.Between(min, max, timeComparison, message));
        }

        public static IMemberSpecificationBuilder<TModel, DateTime?> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, DateTime?> @this, DateTime min, DateTime max, TimeComparison timeComparison = TimeComparison.All, string message = null)
            where TModel : class
        {
            return @this.ValidNullable(m => m.BetweenOrEqualTo(min, max, timeComparison, message));
        }
    }
}