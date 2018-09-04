using System.Collections.Generic;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    // ReSharper disable once InconsistentNaming
    public static class ICollectionRules
    {
        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> EmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.EmptyCollection<TModel, ICollection<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> NotEmptyCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.NotEmptyCollection<TModel, ICollection<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, int size, string message = null)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, ICollection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> ExactCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, long size, string message = null)
            where TModel : class
        {
            return @this.ExactCollectionSize<TModel, ICollection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, int max, string message = null)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, ICollection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> MaxCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, long max, string message = null)
            where TModel : class
        {
            return @this.MaxCollectionSize<TModel, ICollection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, int min, string message = null)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, ICollection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> MinCollectionSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, long min, string message = null)
            where TModel : class
        {
            return @this.MinCollectionSize<TModel, ICollection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, ICollection<TItem>, TItem>(min, max, message);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> CollectionSizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.CollectionSizeBetween<TModel, ICollection<TItem>, TItem>(min, max, message);
        }
    }
}