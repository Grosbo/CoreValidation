using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class LongRules
    {
        public static IMemberSpecificationBuilder<TModel, long> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long> @this, long value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m == value, message ?? Phrases.Keys.Numbers.EqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, long> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long> @this, long value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m != value, message ?? Phrases.Keys.Numbers.NotEqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, long> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, long> @this, long min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m > min, message ?? Phrases.Keys.Numbers.GreaterThan, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, long> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long> @this, long min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m >= min, message ?? Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, long> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, long> @this, long max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m < max, message ?? Phrases.Keys.Numbers.LessThan, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, long> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long> @this, long max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m <= max, message ?? Phrases.Keys.Numbers.LessOrEqualTo, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, long> Between<TModel>(this IMemberSpecificationBuilder<TModel, long> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), message ?? Phrases.Keys.Numbers.Between, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, long> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, long> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), message ?? Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }
    }
}