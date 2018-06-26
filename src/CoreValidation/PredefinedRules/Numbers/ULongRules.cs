using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ULongRules
    {
        public static IMemberSpecification<TModel, ulong> EqualTo<TModel>(this IMemberSpecification<TModel, ulong> @this, ulong value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m == value, message ?? Phrases.Keys.Numbers.EqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecification<TModel, ulong> NotEqualTo<TModel>(this IMemberSpecification<TModel, ulong> @this, ulong value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m != value, message ?? Phrases.Keys.Numbers.NotEqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecification<TModel, ulong> GreaterThan<TModel>(this IMemberSpecification<TModel, ulong> @this, ulong min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m > min, message ?? Phrases.Keys.Numbers.GreaterThan, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecification<TModel, ulong> GreaterOrEqualTo<TModel>(this IMemberSpecification<TModel, ulong> @this, ulong min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m >= min, message ?? Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecification<TModel, ulong> LessThan<TModel>(this IMemberSpecification<TModel, ulong> @this, ulong max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m < max, message ?? Phrases.Keys.Numbers.LessThan, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, ulong> LessOrEqualTo<TModel>(this IMemberSpecification<TModel, ulong> @this, ulong max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m <= max, message ?? Phrases.Keys.Numbers.LessOrEqualTo, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, ulong> Between<TModel>(this IMemberSpecification<TModel, ulong> @this, ulong min, ulong max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), message ?? Phrases.Keys.Numbers.Between, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, ulong> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, ulong> @this, ulong min, ulong max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), message ?? Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }
    }
}