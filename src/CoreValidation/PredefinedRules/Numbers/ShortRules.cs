using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ShortRules
    {
        public static IMemberSpecificationBuilder<TModel, short> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short> @this, short value)
            where TModel : class
        {
            return @this.Valid(m => m == value, Phrases.Keys.Numbers.EqualTo, new[] {Arg.Number(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, short> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short> @this, short value)
            where TModel : class
        {
            return @this.Valid(m => m != value, Phrases.Keys.Numbers.NotEqualTo, new[] {Arg.Number(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, short> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, short> @this, short min)
            where TModel : class
        {
            return @this.Valid(m => m > min, Phrases.Keys.Numbers.GreaterThan, new[] {Arg.Number(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, short> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short> @this, short min)
            where TModel : class
        {
            return @this.Valid(m => m >= min, Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {Arg.Number(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, short> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, short> @this, short max)
            where TModel : class
        {
            return @this.Valid(m => m < max, Phrases.Keys.Numbers.LessThan, new[] {Arg.Number(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, short> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short> @this, short max)
            where TModel : class
        {
            return @this.Valid(m => m <= max, Phrases.Keys.Numbers.LessOrEqualTo, new[] {Arg.Number(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, short> Between<TModel>(this IMemberSpecificationBuilder<TModel, short> @this, short min, short max)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), Phrases.Keys.Numbers.Between, new[] {Arg.Number(nameof(min), min), Arg.Number(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, short> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, short> @this, short min, short max)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {Arg.Number(nameof(min), min), Arg.Number(nameof(max), max)});
        }
    }
}