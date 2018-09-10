using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class CharNumbersRules
    {
        public static IMemberSpecificationBuilder<TModel, char> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m == value, message ?? Phrases.Keys.Numbers.EqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, char> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m != value, message ?? Phrases.Keys.Numbers.NotEqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, char> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m > min, message ?? Phrases.Keys.Numbers.GreaterThan, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, char> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m >= min, message ?? Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, char> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m < max, message ?? Phrases.Keys.Numbers.LessThan, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, char> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m <= max, message ?? Phrases.Keys.Numbers.LessOrEqualTo, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, char> Between<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char min, char max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), message ?? Phrases.Keys.Numbers.Between, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, char> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char min, char max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), message ?? Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }
    }
}