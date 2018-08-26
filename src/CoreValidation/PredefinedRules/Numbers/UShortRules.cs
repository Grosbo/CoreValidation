using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class UShortRules
    {
        public static IMemberSpecificationBuilder<TModel, ushort> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m == value, message ?? Phrases.Keys.Numbers.EqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, ushort> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m != value, message ?? Phrases.Keys.Numbers.NotEqualTo, new[] {new NumberArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, ushort> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m > min, message ?? Phrases.Keys.Numbers.GreaterThan, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, ushort> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort min, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m >= min, message ?? Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, ushort> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m < max, message ?? Phrases.Keys.Numbers.LessThan, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, ushort> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m <= max, message ?? Phrases.Keys.Numbers.LessOrEqualTo, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, ushort> Between<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort min, ushort max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), message ?? Phrases.Keys.Numbers.Between, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, ushort> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort min, ushort max, string message = null)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), message ?? Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }
    }
}