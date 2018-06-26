using System;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class TimeSpanRules
    {
        public static IMemberSpecification<TModel, TimeSpan> EqualTo<TModel>(this IMemberSpecification<TModel, TimeSpan> @this, TimeSpan value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m == value, message ?? Phrases.Keys.TimeSpan.EqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecification<TModel, TimeSpan> NotEqualTo<TModel>(this IMemberSpecification<TModel, TimeSpan> @this, TimeSpan value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m != value, message ?? Phrases.Keys.TimeSpan.NotEqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecification<TModel, TimeSpan> GreaterThan<TModel>(this IMemberSpecification<TModel, TimeSpan> @this, TimeSpan min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m > min, message ?? Phrases.Keys.TimeSpan.GreaterThan, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecification<TModel, TimeSpan> GreaterOrEqualTo<TModel>(this IMemberSpecification<TModel, TimeSpan> @this, TimeSpan min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m >= min, message ?? Phrases.Keys.TimeSpan.GreaterOrEqualTo, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecification<TModel, TimeSpan> LessThan<TModel>(this IMemberSpecification<TModel, TimeSpan> @this, TimeSpan max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m < max, message ?? Phrases.Keys.TimeSpan.LessThan, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, TimeSpan> LessOrEqualTo<TModel>(this IMemberSpecification<TModel, TimeSpan> @this, TimeSpan max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m <= max, message ?? Phrases.Keys.TimeSpan.LessOrEqualTo, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, TimeSpan> Between<TModel>(this IMemberSpecification<TModel, TimeSpan> @this, TimeSpan min, TimeSpan max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), message ?? Phrases.Keys.TimeSpan.Between, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, TimeSpan> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, TimeSpan> @this, TimeSpan min, TimeSpan max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), message ?? Phrases.Keys.TimeSpan.BetweenOrEqualTo, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }
    }
}