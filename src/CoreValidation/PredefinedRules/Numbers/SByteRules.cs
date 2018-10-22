using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class SByteRules
    {
        public static IMemberSpecificationBuilder<TModel, sbyte> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte value)
            where TModel : class
        {
            return @this.Valid(m => m == value, Phrases.Keys.Numbers.EqualTo, new[] {NumberArg.Create(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, sbyte> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte value)
            where TModel : class
        {
            return @this.Valid(m => m != value, Phrases.Keys.Numbers.NotEqualTo, new[] {NumberArg.Create(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, sbyte> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte min)
            where TModel : class
        {
            return @this.Valid(m => m > min, Phrases.Keys.Numbers.GreaterThan, new[] {NumberArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, sbyte> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte min)
            where TModel : class
        {
            return @this.Valid(m => m >= min, Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {NumberArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, sbyte> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte max)
            where TModel : class
        {
            return @this.Valid(m => m < max, Phrases.Keys.Numbers.LessThan, new[] {NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, sbyte> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte max)
            where TModel : class
        {
            return @this.Valid(m => m <= max, Phrases.Keys.Numbers.LessOrEqualTo, new[] {NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, sbyte> Between<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte min, sbyte max)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), Phrases.Keys.Numbers.Between, new[] {NumberArg.Create(nameof(min), min), NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, sbyte> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, sbyte> @this, sbyte min, sbyte max)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {NumberArg.Create(nameof(min), min), NumberArg.Create(nameof(max), max)});
        }
    }
}