using System.Collections.Generic;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    // ReSharper disable once InconsistentNaming
    public static class IReadOnlyCollectionRules
    {
        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> EmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.EmptyCollection<TModel, IReadOnlyCollection<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> NotEmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.NotEmptyCollection<TModel, IReadOnlyCollection<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, int size, string message = null)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, IReadOnlyCollection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, long size, string message = null)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, IReadOnlyCollection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, int max, string message = null)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, IReadOnlyCollection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, long max, string message = null)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, IReadOnlyCollection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, int min, string message = null)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, IReadOnlyCollection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, long min, string message = null)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, IReadOnlyCollection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, IReadOnlyCollection<TItem>, TItem>(min, max, message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, IReadOnlyCollection<TItem>, TItem>(min, max, message);
        }
    }
}