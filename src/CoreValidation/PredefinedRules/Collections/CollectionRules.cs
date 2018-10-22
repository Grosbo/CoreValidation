using System.Collections.ObjectModel;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class CollectionRules
    {
        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> EmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this)
            where TModel : class
        {
            return @this.EmptyCollection<TModel, Collection<TItem>, TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> NotEmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this)
            where TModel : class
        {
            return @this.NotEmptyCollection<TModel, Collection<TItem>, TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, int size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, Collection<TItem>, TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, long size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, Collection<TItem>, TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, int max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, Collection<TItem>, TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, long max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, Collection<TItem>, TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, int min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, Collection<TItem>, TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, long min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, Collection<TItem>, TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, int min, int max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, Collection<TItem>, TItem>(min, max);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, long min, long max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, Collection<TItem>, TItem>(min, max);
        }
    }
}