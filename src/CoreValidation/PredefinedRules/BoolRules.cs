using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class BoolRules
    {
        public static IMemberSpecificationBuilder<TModel, bool> True<TModel>(this IMemberSpecificationBuilder<TModel, bool> @this)
            where TModel : class
        {
            return @this.Valid(m => m, Phrases.Keys.Bool.True);
        }

        public static IMemberSpecificationBuilder<TModel, bool> False<TModel>(this IMemberSpecificationBuilder<TModel, bool> @this)
            where TModel : class
        {
            return @this.Valid(m => !m, Phrases.Keys.Bool.False);
        }
    }
}