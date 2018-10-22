using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class DecimalRules
    {
        public static IMemberSpecificationBuilder<TModel, decimal> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal value)
            where TModel : class
        {
            return @this.Valid(m => m == value, Phrases.Keys.Numbers.EqualTo, new[] {NumberArg.Create(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, decimal> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal value)
            where TModel : class
        {
            return @this.Valid(m => m != value, Phrases.Keys.Numbers.NotEqualTo, new[] {NumberArg.Create(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, decimal> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal min)
            where TModel : class
        {
            return @this.Valid(m => m > min, Phrases.Keys.Numbers.GreaterThan, new[] {NumberArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, decimal> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal min)
            where TModel : class
        {
            return @this.Valid(m => m >= min, Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {NumberArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, decimal> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal max)
            where TModel : class
        {
            return @this.Valid(m => m < max, Phrases.Keys.Numbers.LessThan, new[] {NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, decimal> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal max)
            where TModel : class
        {
            return @this.Valid(m => m <= max, Phrases.Keys.Numbers.LessOrEqualTo, new[] {NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, decimal> Between<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal min, decimal max)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), Phrases.Keys.Numbers.Between, new[] {NumberArg.Create(nameof(min), min), NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, decimal> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, decimal> @this, decimal min, decimal max)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {NumberArg.Create(nameof(min), min), NumberArg.Create(nameof(max), max)});
        }
    }
}