using System.Collections.Generic;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    // ReSharper disable once InconsistentNaming
    public static class ICollectionRules
    {
        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> EmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this)
            where TModel : class
        {
            return @this.EmptyCollection<TModel, ICollection<TItem>, TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> NotEmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this)
            where TModel : class
        {
            return @this.NotEmptyCollection<TModel, ICollection<TItem>, TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, int size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, ICollection<TItem>, TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, long size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, ICollection<TItem>, TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, int max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, ICollection<TItem>, TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, long max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, ICollection<TItem>, TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, int min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, ICollection<TItem>, TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, long min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, ICollection<TItem>, TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, int min, int max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, ICollection<TItem>, TItem>(min, max);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, long min, long max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, ICollection<TItem>, TItem>(min, max);
        }
    }
}