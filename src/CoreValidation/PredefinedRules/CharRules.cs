using System.Globalization;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class CharRules
    {
        public static IMemberSpecificationBuilder<TModel, char> EqualIgnoreCase<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char value)
            where TModel : class
        {
            return @this.Valid(m => string.Compare(m.ToString().ToUpper(), value.ToString().ToUpper(), CultureInfo.InvariantCulture, CompareOptions.Ordinal) == 0, Phrases.Keys.Char.EqualIgnoreCase, new[] {new TextArg(nameof(value), value)});
        }

        public static IMemberSpecificationBuilder<TModel, char> NotEqualIgnoreCase<TModel>(this IMemberSpecificationBuilder<TModel, char> @this, char value)
            where TModel : class
        {
            return @this.Valid(m => string.Compare(m.ToString().ToUpper(), value.ToString().ToUpper(), CultureInfo.InvariantCulture, CompareOptions.Ordinal) != 0, Phrases.Keys.Char.NotEqualIgnoreCase, new[] {new TextArg(nameof(value), value)});
        }
    }
}