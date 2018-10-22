using System;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class TimeSpanRules
    {
        public static IMemberSpecificationBuilder<TModel, TimeSpan> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan value)
            where TModel : class
        {
            return @this.Valid(m => m == value, Phrases.Keys.TimeSpan.EqualTo, new[] {TimeArg.Create(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan value)
            where TModel : class
        {
            return @this.Valid(m => m != value, Phrases.Keys.TimeSpan.NotEqualTo, new[] {TimeArg.Create(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan min)
            where TModel : class
        {
            return @this.Valid(m => m > min, Phrases.Keys.TimeSpan.GreaterThan, new[] {TimeArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan min)
            where TModel : class
        {
            return @this.Valid(m => m >= min, Phrases.Keys.TimeSpan.GreaterOrEqualTo, new[] {TimeArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan max)
            where TModel : class
        {
            return @this.Valid(m => m < max, Phrases.Keys.TimeSpan.LessThan, new[] {TimeArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan max)
            where TModel : class
        {
            return @this.Valid(m => m <= max, Phrases.Keys.TimeSpan.LessOrEqualTo, new[] {TimeArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan> Between<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan min, TimeSpan max)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), Phrases.Keys.TimeSpan.Between, new[] {TimeArg.Create(nameof(min), min), TimeArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, TimeSpan> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, TimeSpan> @this, TimeSpan min, TimeSpan max)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), Phrases.Keys.TimeSpan.BetweenOrEqualTo, new[] {TimeArg.Create(nameof(min), min), TimeArg.Create(nameof(max), max)});
        }
    }
}