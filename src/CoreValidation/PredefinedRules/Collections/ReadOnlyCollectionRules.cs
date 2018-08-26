using System.Collections.ObjectModel;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ReadOnlyCollectionRules
    {
        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> Empty<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.Empty<TModel, ReadOnlyCollection<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> NotEmpty<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.NotEmpty<TModel, ReadOnlyCollection<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> ExactSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, int size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, ReadOnlyCollection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> ExactSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, long size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, ReadOnlyCollection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> MaxSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, int max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, ReadOnlyCollection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> MaxSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, long max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, ReadOnlyCollection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> MinSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, int min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, ReadOnlyCollection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> MinSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, long min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, ReadOnlyCollection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> SizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, ReadOnlyCollection<TItem>, TItem>(min, max, message);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> SizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, ReadOnlyCollection<TItem>, TItem>(min, max, message);
        }
    }
}