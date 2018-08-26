using System.Collections.ObjectModel;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class CollectionRules
    {
        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> Empty<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.Empty<TModel, Collection<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> NotEmpty<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.NotEmpty<TModel, Collection<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> ExactSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, int size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, Collection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> ExactSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, long size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, Collection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> MaxSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, int max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, Collection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> MaxSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, long max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, Collection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> MinSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, int min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, Collection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> MinSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, long min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, Collection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> SizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, Collection<TItem>, TItem>(min, max, message);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> SizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, Collection<TItem>, TItem>(min, max, message);
        }
    }
}