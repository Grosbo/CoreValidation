using System.Collections.Generic;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ListRules
    {
        public static IMemberSpecificationBuilder<TModel, List<TItem>> EmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.EmptyCollection<TModel, List<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> NotEmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.NotEmptyCollection<TModel, List<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, int size, string message = null)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, List<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, long size, string message = null)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, List<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, int max, string message = null)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, List<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, long max, string message = null)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, List<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, int min, string message = null)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, List<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, long min, string message = null)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, List<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, List<TItem>, TItem>(min, max, message);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, List<TItem>, TItem>(min, max, message);
        }
    }
}