using System.Collections.ObjectModel;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ReadOnlyCollectionRules
    {
        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> EmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.EmptyCollection<TModel, ReadOnlyCollection<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> NotEmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.NotEmptyCollection<TModel, ReadOnlyCollection<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, int size, string message = null)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, ReadOnlyCollection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, long size, string message = null)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, ReadOnlyCollection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, int max, string message = null)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, ReadOnlyCollection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, long max, string message = null)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, ReadOnlyCollection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, int min, string message = null)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, ReadOnlyCollection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, long min, string message = null)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, ReadOnlyCollection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, ReadOnlyCollection<TItem>, TItem>(min, max, message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, ReadOnlyCollection<TItem>, TItem>(min, max, message);
        }
    }
}