using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class UShortRules
    {
        public static IMemberSpecificationBuilder<TModel, ushort> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort value)
            where TModel : class
        {
            return @this.Valid(m => m == value, Phrases.Keys.Numbers.EqualTo, new[] {NumberArg.Create(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, ushort> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort value)
            where TModel : class
        {
            return @this.Valid(m => m != value, Phrases.Keys.Numbers.NotEqualTo, new[] {NumberArg.Create(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, ushort> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort min)
            where TModel : class
        {
            return @this.Valid(m => m > min, Phrases.Keys.Numbers.GreaterThan, new[] {NumberArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, ushort> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort min)
            where TModel : class
        {
            return @this.Valid(m => m >= min, Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {NumberArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, ushort> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort max)
            where TModel : class
        {
            return @this.Valid(m => m < max, Phrases.Keys.Numbers.LessThan, new[] {NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, ushort> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort max)
            where TModel : class
        {
            return @this.Valid(m => m <= max, Phrases.Keys.Numbers.LessOrEqualTo, new[] {NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, ushort> Between<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort min, ushort max)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), Phrases.Keys.Numbers.Between, new[] {NumberArg.Create(nameof(min), min), NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, ushort> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ushort> @this, ushort min, ushort max)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {NumberArg.Create(nameof(min), min), NumberArg.Create(nameof(max), max)});
        }
    }
}