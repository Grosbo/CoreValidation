using System;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class StringRules
    {
        public static IMemberSpecificationBuilder<TModel, string> EqualTo<TModel>(this IMemberSpecificationBuilder<TModel, string> @this, string value, StringComparison stringComparison = StringComparison.Ordinal)
            where TModel : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return @this.Valid(m => string.Equals(m, value, stringComparison), Phrases.Keys.Texts.EqualTo, new[] {Arg.Text(nameof(value), value), Arg.Enum(nameof(stringComparison), stringComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, string> NotEqualTo<TModel>(this IMemberSpecificationBuilder<TModel, string> @this, string value, StringComparison stringComparison = StringComparison.Ordinal)
            where TModel : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return @this.Valid(m => !string.Equals(m, value, stringComparison), Phrases.Keys.Texts.NotEqualTo, new[] {Arg.Text(nameof(value), value), Arg.Enum(nameof(stringComparison), stringComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, string> Contains<TModel>(this IMemberSpecificationBuilder<TModel, string> @this, string value, StringComparison stringComparison = StringComparison.Ordinal)
            where TModel : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return @this.Valid(m => m.IndexOf(value, stringComparison) >= 0, Phrases.Keys.Texts.Contains, new[] {Arg.Text(nameof(value), value), Arg.Enum(nameof(stringComparison), stringComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, string> NotContains<TModel>(this IMemberSpecificationBuilder<TModel, string> @this, string value, StringComparison stringComparison = StringComparison.Ordinal)
            where TModel : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return @this.Valid(m => m.IndexOf(value, stringComparison) < 0, Phrases.Keys.Texts.NotContains, new[] {Arg.Text(nameof(value), value), Arg.Enum(nameof(stringComparison), stringComparison)});
        }

        public static IMemberSpecificationBuilder<TModel, string> NotEmpty<TModel>(this IMemberSpecificationBuilder<TModel, string> @this)
            where TModel : class
        {
            return @this.Valid(m => !string.IsNullOrEmpty(m), Phrases.Keys.Texts.NotEmpty);
        }

        public static IMemberSpecificationBuilder<TModel, string> NotWhiteSpace<TModel>(this IMemberSpecificationBuilder<TModel, string> @this)
            where TModel : class
        {
            return @this.Valid(m => !string.IsNullOrWhiteSpace(m), Phrases.Keys.Texts.NotWhiteSpace);
        }

        public static IMemberSpecificationBuilder<TModel, string> SingleLine<TModel>(this IMemberSpecificationBuilder<TModel, string> @this)
            where TModel : class
        {
            return @this.Valid(m => !m.Contains(Environment.NewLine), Phrases.Keys.Texts.SingleLine);
        }

        public static IMemberSpecificationBuilder<TModel, string> ExactLength<TModel>(this IMemberSpecificationBuilder<TModel, string> @this, int length)
            where TModel : class
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), length, "Exact length cannot be less than zero");
            }

            return @this.Valid(m => m.Replace(Environment.NewLine, " ").Length == length, Phrases.Keys.Texts.ExactLength, new[] {Arg.Number(nameof(length), length)});
        }

        public static IMemberSpecificationBuilder<TModel, string> MaxLength<TModel>(this IMemberSpecificationBuilder<TModel, string> @this, int max)
            where TModel : class
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Max length cannot be less than zero");
            }

            return @this.Valid(m => m.Replace(Environment.NewLine, " ").Length <= max, Phrases.Keys.Texts.MaxLength, new[] {Arg.Number(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, string> MinLength<TModel>(this IMemberSpecificationBuilder<TModel, string> @this, int min)
            where TModel : class
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, "Max length cannot be less than zero");
            }

            return @this.Valid(m => m.Replace(Environment.NewLine, " ").Length >= min, Phrases.Keys.Texts.MinLength, new[] {Arg.Number(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, string> LengthBetween<TModel>(this IMemberSpecificationBuilder<TModel, string> @this, int min, int max)
            where TModel : class
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, "Min length cannot be less than zero");
            }

            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Max length cannot be less than zero");
            }

            if (max < min)
            {
                throw new ArgumentException($"Max length {{{nameof(max)}}} cannot be less than min length {{{nameof(min)}}}", nameof(max));
            }

            return @this.Valid(m =>
                {
                    var squashedLength = m.Replace(Environment.NewLine, " ").Length;

                    return (squashedLength >= min) && (squashedLength <= max);
                }, Phrases.Keys.Texts.LengthBetween,
                new[] {Arg.Number(nameof(min), min), Arg.Number(nameof(max), max)});
        }
    }
}