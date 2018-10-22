using System.Collections.ObjectModel;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ReadOnlyCollectionRules
    {
        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> EmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this)
            where TModel : class
        {
            return @this.EmptyCollection<TModel, ReadOnlyCollection<TItem>, TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> NotEmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this)
            where TModel : class
        {
            return @this.NotEmptyCollection<TModel, ReadOnlyCollection<TItem>, TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, int size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, ReadOnlyCollection<TItem>, TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, long size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, ReadOnlyCollection<TItem>, TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, int max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, ReadOnlyCollection<TItem>, TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, long max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, ReadOnlyCollection<TItem>, TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, int min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, ReadOnlyCollection<TItem>, TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, long min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, ReadOnlyCollection<TItem>, TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, int min, int max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, ReadOnlyCollection<TItem>, TItem>(min, max);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, long min, long max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, ReadOnlyCollection<TItem>, TItem>(min, max);
        }
    }
}