using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DecimalRules
    {
        public static IMemberSpecificationBuilder<TModel, decimal> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m == value, message ?? Phrases.Keys.Numbers.EqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, decimal> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m != value, message ?? Phrases.Keys.Numbers.NotEqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, decimal> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m > min, message ?? Phrases.Keys.Numbers.GreaterThan, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, decimal> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m >= min, message ?? Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, decimal> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m < max, message ?? Phrases.Keys.Numbers.LessThan, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, decimal> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m <= max, message ?? Phrases.Keys.Numbers.LessOrEqualTo, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, decimal> Between<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal min, decimal max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), message ?? Phrases.Keys.Numbers.Between, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, decimal> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal min, decimal max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), message ?? Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }
    }
}