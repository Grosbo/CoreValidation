using System.Collections.ObjectModel;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class CollectionRules
    {
        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> EmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.EmptyCollection<TModel, Collection<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> NotEmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.NotEmptyCollection<TModel, Collection<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, int size, string message = null)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, Collection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, long size, string message = null)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, Collection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, int max, string message = null)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, Collection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, long max, string message = null)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, Collection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, int min, string message = null)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, Collection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, long min, string message = null)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, Collection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, Collection<TItem>, TItem>(min, max, message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, Collection<TItem>, TItem>(min, max, message);
        }
    }
}