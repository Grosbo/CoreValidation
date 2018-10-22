using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class CharNumbersRules
    {
        public static IMemberSpecificationBuilder<TModel, char> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char value)
            where TModel : class
        {
            return @this.Valid(m => m == value, Phrases.Keys.Numbers.EqualTo, new[] {NumberArg.Create(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, char> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char value)
            where TModel : class
        {
            return @this.Valid(m => m != value, Phrases.Keys.Numbers.NotEqualTo, new[] {NumberArg.Create(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, char> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char min)
            where TModel : class
        {
            return @this.Valid(m => m > min, Phrases.Keys.Numbers.GreaterThan, new[] {NumberArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, char> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char min)
            where TModel : class
        {
            return @this.Valid(m => m >= min, Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {NumberArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, char> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char max)
            where TModel : class
        {
            return @this.Valid(m => m < max, Phrases.Keys.Numbers.LessThan, new[] {NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, char> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char max)
            where TModel : class
        {
            return @this.Valid(m => m <= max, Phrases.Keys.Numbers.LessOrEqualTo, new[] {NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, char> Between<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char min, char max)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), Phrases.Keys.Numbers.Between, new[] {NumberArg.Create(nameof(min), min), NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, char> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char min, char max)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {NumberArg.Create(nameof(min), min), NumberArg.Create(nameof(max), max)});
        }
    }
}