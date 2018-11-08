using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class UIntRules
    {
        public static IMemberSpecificationBuilder<TModel, uint> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint> @this, uint value)
            where TModel : class
        {
            return @this.Valid(m => m == value, Phrases.Keys.Numbers.EqualTo, new[] {Arg.Number(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, uint> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint> @this, uint value)
            where TModel : class
        {
            return @this.Valid(m => m != value, Phrases.Keys.Numbers.NotEqualTo, new[] {Arg.Number(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, uint> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, uint> @this, uint min)
            where TModel : class
        {
            return @this.Valid(m => m > min, Phrases.Keys.Numbers.GreaterThan, new[] {Arg.Number(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, uint> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint> @this, uint min)
            where TModel : class
        {
            return @this.Valid(m => m >= min, Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {Arg.Number(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, uint> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, uint> @this, uint max)
            where TModel : class
        {
            return @this.Valid(m => m < max, Phrases.Keys.Numbers.LessThan, new[] {Arg.Number(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, uint> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint> @this, uint max)
            where TModel : class
        {
            return @this.Valid(m => m <= max, Phrases.Keys.Numbers.LessOrEqualTo, new[] {Arg.Number(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, uint> Between<TModel>(this IMemberSpecificationBuilder<TModel, uint> @this, uint min, uint max)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), Phrases.Keys.Numbers.Between, new[] {Arg.Number(nameof(min), min), Arg.Number(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, uint> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, uint> @this, uint min, uint max)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {Arg.Number(nameof(min), min), Arg.Number(nameof(max), max)});
        }
    }
}