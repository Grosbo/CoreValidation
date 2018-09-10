using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class SByteRules
    {
        public static IMemberSpecificationBuilder<TModel, sbyte> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m == value, message ?? Phrases.Keys.Numbers.EqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, sbyte> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m != value, message ?? Phrases.Keys.Numbers.NotEqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, sbyte> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m > min, message ?? Phrases.Keys.Numbers.GreaterThan, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, sbyte> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m >= min, message ?? Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, sbyte> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m < max, message ?? Phrases.Keys.Numbers.LessThan, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, sbyte> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m <= max, message ?? Phrases.Keys.Numbers.LessOrEqualTo, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, sbyte> Between<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte min, sbyte max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), message ?? Phrases.Keys.Numbers.Between, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, sbyte> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte min, sbyte max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), message ?? Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }
    }
}