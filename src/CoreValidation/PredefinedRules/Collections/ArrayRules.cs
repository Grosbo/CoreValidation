using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ArrayRules
    {
        public static IMemberSpecificationBuilder<TModel, TItem[]> EmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this)
            where TModel : class
        {
            return @this.EmptyCollection<TModel, TItem[], TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> NotEmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this)
            where TModel : class
        {
            return @this.NotEmptyCollection<TModel, TItem[], TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, int size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, TItem[], TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, long size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, TItem[], TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, int max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, TItem[], TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, long max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, TItem[], TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, int min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, TItem[], TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, long min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, TItem[], TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, int min, int max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, TItem[], TItem>(min, max);
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, long min, long max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, TItem[], TItem>(min, max);
        }
    }
}