using System.Collections.Generic;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableRules
    {
        public static IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> EmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> @this)
            where TModel : class
        {
            return @this.EmptyCollection<TModel, IEnumerable<TItem>, TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> NotEmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> @this)
            where TModel : class
        {
            return @this.NotEmptyCollection<TModel, IEnumerable<TItem>, TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> @this, int size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, IEnumerable<TItem>, TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> @this, long size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, IEnumerable<TItem>, TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> @this, int max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, IEnumerable<TItem>, TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> @this, long max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, IEnumerable<TItem>, TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> @this, int min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, IEnumerable<TItem>, TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> @this, long min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, IEnumerable<TItem>, TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> @this, int min, int max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, IEnumerable<TItem>, TItem>(min, max);
        }

        public static IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> @this, long min, long max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, IEnumerable<TItem>, TItem>(min, max);
        }
    }
}