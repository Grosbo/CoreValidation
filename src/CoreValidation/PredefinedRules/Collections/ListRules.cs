using System.Collections.Generic;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ListRules
    {
        public static IMemberSpecificationBuilder<TModel, List<TItem>> EmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this)
            where TModel : class
        {
            return @this.EmptyCollection<TModel, List<TItem>, TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> NotEmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this)
            where TModel : class
        {
            return @this.NotEmptyCollection<TModel, List<TItem>, TItem>();
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, int size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, List<TItem>, TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, long size)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, List<TItem>, TItem>(size);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, int max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, List<TItem>, TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, long max)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, List<TItem>, TItem>(max);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, int min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, List<TItem>, TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, long min)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, List<TItem>, TItem>(min);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, int min, int max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, List<TItem>, TItem>(min, max);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, long min, long max)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, List<TItem>, TItem>(min, max);
        }
    }
}