using CoreValidation.Specifications;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class BoolRules
    {
        public static IMemberSpecification<TModel, bool> True<TModel>(this IMemberSpecification<TModel, bool> @this, string message = null)
            where TModel : class
        {
            return @this.Valid(m => m, message ?? Phrases.Keys.Bool.True);
        }

        public static IMemberSpecification<TModel, bool> False<TModel>(this IMemberSpecification<TModel, bool> @this, string message = null)
            where TModel : class
        {
            return @this.Valid(m => !m, message ?? Phrases.Keys.Bool.False);
        }
    }
}