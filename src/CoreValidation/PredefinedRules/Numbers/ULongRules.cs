using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ULongRules
    {
        public static IMemberSpecificationBuilder<TModel, ulong> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m == value, message ?? Phrases.Keys.Numbers.EqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, ulong> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m != value, message ?? Phrases.Keys.Numbers.NotEqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, ulong> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m > min, message ?? Phrases.Keys.Numbers.GreaterThan, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, ulong> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m >= min, message ?? Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, ulong> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m < max, message ?? Phrases.Keys.Numbers.LessThan, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, ulong> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m <= max, message ?? Phrases.Keys.Numbers.LessOrEqualTo, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, ulong> Between<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong min, ulong max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), message ?? Phrases.Keys.Numbers.Between, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, ulong> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong min, ulong max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), message ?? Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }
    }
}