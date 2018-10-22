using System.Collections.Generic;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    // ReSharper disable once InconsistentNaming
    public static class IListRules
    {
        public static IMemberSpecificationBuilder<TModel, IList<TItem>> EmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this)
            where TModel : class
        {
            return @this.EmptyCollection<TModel, IList<TItem>, TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> NotEmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this)
            where TModel : class
        {
            return @this.NotEmptyCollection<TModel, IList<TItem>, TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, int size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, IList<TItem>, TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, long size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, IList<TItem>, TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, int max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, IList<TItem>, TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, long max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, IList<TItem>, TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, int min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, IList<TItem>, TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, long min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, IList<TItem>, TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, int min, int max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, IList<TItem>, TItem>(min, max);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, long min, long max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, IList<TItem>, TItem>(min, max);
        }
    }
}