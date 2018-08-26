using System.Collections.Generic;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    // ReSharper disable once InconsistentNaming
    public static class IReadOnlyCollectionRules
    {
        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> Empty<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.Empty<TModel, IReadOnlyCollection<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> NotEmpty<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.NotEmpty<TModel, IReadOnlyCollection<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> ExactSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, int size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, IReadOnlyCollection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> ExactSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, long size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, IReadOnlyCollection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> MaxSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, int max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, IReadOnlyCollection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> MaxSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, long max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, IReadOnlyCollection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> MinSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, int min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, IReadOnlyCollection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> MinSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, long min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, IReadOnlyCollection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> SizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, IReadOnlyCollection<TItem>, TItem>(min, max, message);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> SizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, IReadOnlyCollection<TItem>, TItem>(min, max, message);
        }
    }
}