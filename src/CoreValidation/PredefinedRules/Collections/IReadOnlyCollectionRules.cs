using System.Collections.Generic;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    // ReSharper disable once InconsistentNaming
    public static class IReadOnlyCollectionRules
    {
        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> EmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this)
            where TModel : class
        {
            return @this.EmptyCollection<TModel, IReadOnlyCollection<TItem>, TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> NotEmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this)
            where TModel : class
        {
            return @this.NotEmptyCollection<TModel, IReadOnlyCollection<TItem>, TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, int size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, IReadOnlyCollection<TItem>, TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, long size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, IReadOnlyCollection<TItem>, TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, int max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, IReadOnlyCollection<TItem>, TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, long max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, IReadOnlyCollection<TItem>, TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, int min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, IReadOnlyCollection<TItem>, TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, long min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, IReadOnlyCollection<TItem>, TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, int min, int max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, IReadOnlyCollection<TItem>, TItem>(min, max);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, long min, long max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, IReadOnlyCollection<TItem>, TItem>(min, max);
        }
    }
}