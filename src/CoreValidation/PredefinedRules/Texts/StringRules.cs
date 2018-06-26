using System;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class StringRules
    {
        public static IMemberSpecification<TModel, string> EqualTo<TModel>(this IMemberSpecification<TModel, string> @this, string value, StringComparison stringComparison = StringComparison.Ordinal, string message = null)
            where TModel : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return @this.Valid(m => string.Equals(m, value, stringComparison), message ?? Phrases.Keys.Texts.EqualTo, new IMessageArg[] {new TextArg(nameof(value), value), new EnumArg<StringComparison>(nameof(stringComparison), stringComparison)});
        }

        public static IMemberSpecification<TModel, string> NotEqualTo<TModel>(this IMemberSpecification<TModel, string> @this, string value, StringComparison stringComparison = StringComparison.Ordinal, string message = null)
            where TModel : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return @this.Valid(m => !string.Equals(m, value, stringComparison), message ?? Phrases.Keys.Texts.NotEqualTo, new IMessageArg[] {new TextArg(nameof(value), value), new EnumArg<StringComparison>(nameof(stringComparison), stringComparison)});
        }

        public static IMemberSpecification<TModel, string> Contains<TModel>(this IMemberSpecification<TModel, string> @this, string value, StringComparison stringComparison = StringComparison.Ordinal, string message = null)
            where TModel : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return @this.Valid(m => m.IndexOf(value, stringComparison) >= 0, message ?? Phrases.Keys.Texts.Contains, new IMessageArg[] {new TextArg(nameof(value), value), new EnumArg<StringComparison>(nameof(stringComparison), stringComparison)});
        }

        public static IMemberSpecification<TModel, string> NotContains<TModel>(this IMemberSpecification<TModel, string> @this, string value, StringComparison stringComparison = StringComparison.Ordinal, string message = null)
            where TModel : class
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return @this.Valid(m => m.IndexOf(value, stringComparison) < 0, message ?? Phrases.Keys.Texts.NotContains, new IMessageArg[] {new TextArg(nameof(value), value), new EnumArg<StringComparison>(nameof(stringComparison), stringComparison)});
        }

        public static IMemberSpecification<TModel, string> NotEmpty<TModel>(this IMemberSpecification<TModel, string> @this, string message = null)
            where TModel : class
        {
            return @this.Valid(m => !string.IsNullOrEmpty(m), message ?? Phrases.Keys.Texts.NotEmpty);
        }

        public static IMemberSpecification<TModel, string> NotWhiteSpace<TModel>(this IMemberSpecification<TModel, string> @this, string message = null)
            where TModel : class
        {
            return @this.Valid(m => !string.IsNullOrWhiteSpace(m), message ?? Phrases.Keys.Texts.NotWhiteSpace);
        }

        public static IMemberSpecification<TModel, string> SingleLine<TModel>(this IMemberSpecification<TModel, string> @this, string message = null)
            where TModel : class
        {
            return @this.Valid(m => !m.Contains(Environment.NewLine), message ?? Phrases.Keys.Texts.SingleLine);
        }

        public static IMemberSpecification<TModel, string> ExactLength<TModel>(this IMemberSpecification<TModel, string> @this, int length, string message = null)
            where TModel : class
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), length, "Exact length cannot be less than zero");
            }

            return @this.Valid(m => m.Replace(Environment.NewLine, " ").Length == length, message ?? Phrases.Keys.Texts.ExactLength, new[] {new NumberArg(nameof(length), length)});
        }

        public static IMemberSpecification<TModel, string> MaxLength<TModel>(this IMemberSpecification<TModel, string> @this, int max, string message = null)
            where TModel : class
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Max length cannot be less than zero");
            }

            return @this.Valid(m => m.Replace(Environment.NewLine, " ").Length <= max, message ?? Phrases.Keys.Texts.MaxLength, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, string> MinLength<TModel>(this IMemberSpecification<TModel, string> @this, int min, string message = null)
            where TModel : class
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, "Max length cannot be less than zero");
            }

            return @this.Valid(m => m.Replace(Environment.NewLine, " ").Length >= min, message ?? Phrases.Keys.Texts.MinLength, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecification<TModel, string> LengthBetween<TModel>(this IMemberSpecification<TModel, string> @this, int min, int max, string message = null)
            where TModel : class
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, message ?? "Min length cannot be less than zero");
            }

            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, message ?? "Max length cannot be less than zero");
            }

            if (max < min)
            {
                throw new ArgumentException($"Max length {{{nameof(max)}}} cannot be less than min length {{{nameof(min)}}}", nameof(max));
            }

            return @this.Valid(m =>
                {
                    var squashedLength = m.Replace(Environment.NewLine, " ").Length;

                    return (squashedLength >= min) && (squashedLength <= max);
                }, message ?? Phrases.Keys.Texts.LengthBetween,
                new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, string> IsGuid<TModel>(this IMemberSpecification<TModel, string> @this, string message = null)
            where TModel : class
        {
            return @this.Valid(m => Guid.TryParse(m, out _), Phrases.Keys.Texts.IsGuid);
        }
    }
}