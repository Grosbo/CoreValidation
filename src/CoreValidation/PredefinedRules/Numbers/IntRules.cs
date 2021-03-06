using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class IntRules
    {
        public static IMemberSpecificationBuilder<TModel, int> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int> @this, int value)
            where TModel : class
        {
            return @this.Valid(m => m == value, Phrases.Keys.Numbers.EqualTo, new[] {Arg.Number(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, int> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int> @this, int value)
            where TModel : class
        {
            return @this.Valid(m => m != value, Phrases.Keys.Numbers.NotEqualTo, new[] {Arg.Number(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, int> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, int> @this, int min)
            where TModel : class
        {
            return @this.Valid(m => m > min, Phrases.Keys.Numbers.GreaterThan, new[] {Arg.Number(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, int> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int> @this, int min)
            where TModel : class
        {
            return @this.Valid(m => m >= min, Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {Arg.Number(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, int> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, int> @this, int max)
            where TModel : class
        {
            return @this.Valid(m => m < max, Phrases.Keys.Numbers.LessThan, new[] {Arg.Number(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, int> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int> @this, int max)
            where TModel : class
        {
            return @this.Valid(m => m <= max, Phrases.Keys.Numbers.LessOrEqualTo, new[] {Arg.Number(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, int> Between<TModel>(this IMemberSpecificationBuilder<TModel, int> @this, int min, int max)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), Phrases.Keys.Numbers.Between, new[] {Arg.Number(nameof(min), min), Arg.Number(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, int> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, int> @this, int min, int max)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {Arg.Number(nameof(min), min), Arg.Number(nameof(max), max)});
        }
    }
}