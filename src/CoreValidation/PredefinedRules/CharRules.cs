using System.Globalization;

using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class CharRules
    {
        public static IMemberSpecification<TModel, char> EqualIgnoreCase<TModel>(this IMemberSpecification<TModel, char> @this, char value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => string.Compare(m.ToString().ToUpper(), value.ToString().ToUpper(), CultureInfo.InvariantCulture, CompareOptions.Ordinal) == 0, message ?? Phrases.Keys.Char.EqualIgnoreCase, new[] {new TextArg(nameof(value), value)});
        }

        public static IMemberSpecification<TModel, char> NotEqualIgnoreCase<TModel>(this IMemberSpecification<TModel, char> @this, char value, string message = null)
            where TModel : class
        {
            return @this.Valid(m => string.Compare(m.ToString().ToUpper(), value.ToString().ToUpper(), CultureInfo.InvariantCulture, CompareOptions.Ordinal) != 0, message ?? Phrases.Keys.Char.NotEqualIgnoreCase, new[] {new TextArg(nameof(value), value)});
        }
    }
}