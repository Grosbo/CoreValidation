using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class SByteRules
    {
        public static IMemberSpecification<TModel, sbyte> EqualTo<TModel>(this IMemberSpecification<TModel, sbyte> @this, sbyte value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m == value, message ?? Phrases.Keys.Numbers.EqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecification<TModel, sbyte> NotEqualTo<TModel>(this IMemberSpecification<TModel, sbyte> @this, sbyte value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m != value, message ?? Phrases.Keys.Numbers.NotEqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecification<TModel, sbyte> GreaterThan<TModel>(this IMemberSpecification<TModel, sbyte> @this, sbyte min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m > min, message ?? Phrases.Keys.Numbers.GreaterThan, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecification<TModel, sbyte> GreaterOrEqualTo<TModel>(this IMemberSpecification<TModel, sbyte> @this, sbyte min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m >= min, message ?? Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecification<TModel, sbyte> LessThan<TModel>(this IMemberSpecification<TModel, sbyte> @this, sbyte max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m < max, message ?? Phrases.Keys.Numbers.LessThan, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, sbyte> LessOrEqualTo<TModel>(this IMemberSpecification<TModel, sbyte> @this, sbyte max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m <= max, message ?? Phrases.Keys.Numbers.LessOrEqualTo, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, sbyte> Between<TModel>(this IMemberSpecification<TModel, sbyte> @this, sbyte min, sbyte max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), message ?? Phrases.Keys.Numbers.Between, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, sbyte> BetweenOrEqualTo<TModel>(this IMemberSpecification<TModel, sbyte> @this, sbyte min, sbyte max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), message ?? Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }
    }
}