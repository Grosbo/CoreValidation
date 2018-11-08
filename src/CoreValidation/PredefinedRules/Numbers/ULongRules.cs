using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ULongRules
    {
        public static IMemberSpecificationBuilder<TModel, ulong> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong value)
            where TModel : class
        {
            return @this.Valid(m => m == value, Phrases.Keys.Numbers.EqualTo, new[] {Arg.Number(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, ulong> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong value)
            where TModel : class
        {
            return @this.Valid(m => m != value, Phrases.Keys.Numbers.NotEqualTo, new[] {Arg.Number(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, ulong> GreaterThan<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong min)
            where TModel : class
        {
            return @this.Valid(m => m > min, Phrases.Keys.Numbers.GreaterThan, new[] {Arg.Number(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, ulong> GreaterOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong min)
            where TModel : class
        {
            return @this.Valid(m => m >= min, Phrases.Keys.Numbers.GreaterOrEqualTo, new[] {Arg.Number(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, ulong> LessThan<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong max)
            where TModel : class
        {
            return @this.Valid(m => m < max, Phrases.Keys.Numbers.LessThan, new[] {Arg.Number(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, ulong> LessOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong max)
            where TModel : class
        {
            return @this.Valid(m => m <= max, Phrases.Keys.Numbers.LessOrEqualTo, new[] {Arg.Number(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, ulong> Between<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong min, ulong max)
            where TModel : class
        {
            return @this.Valid(m => (m > min) && (m < max), Phrases.Keys.Numbers.Between, new[] {Arg.Number(nameof(min), min), Arg.Number(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, ulong> BetweenOrEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, ulong> @this, ulong min, ulong max)
            where TModel : class
        {
            return @this.Valid(m => (m >= min) && (m <= max), Phrases.Keys.Numbers.BetweenOrEqualTo, new[] {Arg.Number(nameof(min), min), Arg.Number(nameof(max), max)});
        }
    }
}