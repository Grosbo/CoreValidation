using System;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class TimeSpanRules
    {
        public static IMemberSpecificationBuilder<TModel, TimeSpan> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m == value, message ?? Phrases.Keys.TimeSpan.EqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m != value, message ?? Phrases.Keys.TimeSpan.NotEqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m > min, message ?? Phrases.Keys.TimeSpan.GreaterThan, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m >= min, message ?? Phrases.Keys.TimeSpan.GreaterOrEqualTo, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m < max, message ?? Phrases.Keys.TimeSpan.LessThan, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m <= max, message ?? Phrases.Keys.TimeSpan.LessOrEqualTo, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan> Between<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan min, TimeSpan max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), message ?? Phrases.Keys.TimeSpan.Between, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan min, TimeSpan max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), message ?? Phrases.Keys.TimeSpan.BetweenOrEqualTo, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }
    }
}