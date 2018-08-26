using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ArrayRules
    {
        public static IMemberSpecificationBuilder<TModel, TItem[]> Empty<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, string message = null)
            where TModel : class
        {
            return @this.Empty<TModel, TItem[], TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> NotEmpty<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, string message = null)
            where TModel : class
        {
            return @this.NotEmpty<TModel, TItem[], TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> ExactSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, int size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, TItem[], TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> ExactSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, long size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, TItem[], TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> MaxSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, int max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, TItem[], TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> MaxSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, long max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, TItem[], TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> MinSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, int min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, TItem[], TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> MinSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, long min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, TItem[], TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> SizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, TItem[], TItem>(min, max, message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> SizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, TItem[], TItem>(min, max, message);
        }
    }
}