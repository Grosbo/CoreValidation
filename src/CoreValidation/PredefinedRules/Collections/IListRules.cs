using System.Collections.Generic;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    // ReSharper disable once InconsistentNaming
    public static class IListRules
    {
        public static IMemberSpecificationBuilder<TModel, IList<TItem>> Empty<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.Empty<TModel, IList<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> NotEmpty<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.NotEmpty<TModel, IList<TItem>, TItem>(message);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> ExactSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, int size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, IList<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> ExactSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, long size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, IList<TItem>, TItem>(size, message);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> MaxSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, int max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, IList<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> MaxSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, long max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, IList<TItem>, TItem>(max, message);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> MinSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, int min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, IList<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> MinSize<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, long min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, IList<TItem>, TItem>(min, message);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> SizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, IList<TItem>, TItem>(min, max, message);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> SizeBetween<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, IList<TItem>, TItem>(min, max, message);
        }
    }
}