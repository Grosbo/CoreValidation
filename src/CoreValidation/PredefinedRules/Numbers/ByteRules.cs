using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ByteRules
    {
        public static IMemberSpecificationBuilder<TModel, byte> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte> @this, byte value)
            where TModel : class
        {
            return @this.Valid(m => m == value, Phrases.Keys.Numbers.EqualTo, new[] {NumberArg.Create(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, byte> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte> @this, byte value)
            where TModel : class
        {
            return @this.Valid(m => m != value, Phrases.Keys.Numbers.NotEqualTo, new[] {NumberArg.Create(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, byte> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, byte> @this, byte min)
            where TModel : class
        {
            return @this.Valid(m => m > min, Phrases.Keys.Numbers.GreaterThan, new[] {NumberArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, byte> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte> @this, byte min)
            where TModel : class
        {
            return @this.Valid(m => m >= min, Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {NumberArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, byte> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, byte> @this, byte max)
            where TModel : class
        {
            return @this.Valid(m => m < max, Phrases.Keys.Numbers.LessThan, new[] {NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, byte> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte> @this, byte max)
            where TModel : class
        {
            return @this.Valid(m => m <= max, Phrases.Keys.Numbers.LessOrEqualTo, new[] {NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, byte> Between<TModel>(this IMemberSpecificationBuilder<TModel, byte> @this, byte min, byte max)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), Phrases.Keys.Numbers.Between, new[] {NumberArg.Create(nameof(min), min), NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, byte> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, byte> @this, byte min, byte max)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {NumberArg.Create(nameof(min), min), NumberArg.Create(nameof(max), max)});
        }
    }
}