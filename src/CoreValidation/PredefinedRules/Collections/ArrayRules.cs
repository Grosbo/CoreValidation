using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ArrayRules
    {
        public static IMemberSpecificationBuilder<TModel, TItem[]> EmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, string message = null)
            where TModel : class
        {
            return @this.EmptyCollection<TModel, TItem[], TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> NotEmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, string message = null)
            where TModel : class
        {
            return @this.NotEmptyCollection<TModel, TItem[], TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, int size, string message = null)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, TItem[], TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, long size, string message = null)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, TItem[], TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, int max, string message = null)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, TItem[], TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, long max, string message = null)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, TItem[], TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, int min, string message = null)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, TItem[], TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, long min, string message = null)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, TItem[], TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, TItem[], TItem>(min, max, message);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, TItem[], TItem>(min, max, message);
        }
    }
}