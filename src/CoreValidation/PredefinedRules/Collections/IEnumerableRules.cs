using System.Collections.Generic;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableRules
    {
        public static IMemberSpecification<TModel, IEnumerable<TItem>> Empty<TModel, TItem>(this IMemberSpecification<TModel, IEnumerable<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.Empty<TModel, IEnumerable<TItem>, TItem>(message);
        }

        public static IMemberSpecification<TModel, IEnumerable<TItem>> NotEmpty<TModel, TItem>(this IMemberSpecification<TModel, IEnumerable<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.NotEmpty<TModel, IEnumerable<TItem>, TItem>(message);
        }

        public static IMemberSpecification<TModel, IEnumerable<TItem>> ExactSize<TModel, TItem>(this IMemberSpecification<TModel, IEnumerable<TItem>> @this, int size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, IEnumerable<TItem>, TItem>(size, message);
        }

        public static IMemberSpecification<TModel, IEnumerable<TItem>> ExactSize<TModel, TItem>(this IMemberSpecification<TModel, IEnumerable<TItem>> @this, long size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, IEnumerable<TItem>, TItem>(size, message);
        }

        public static IMemberSpecification<TModel, IEnumerable<TItem>> MaxSize<TModel, TItem>(this IMemberSpecification<TModel, IEnumerable<TItem>> @this, int max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, IEnumerable<TItem>, TItem>(max, message);
        }

        public static IMemberSpecification<TModel, IEnumerable<TItem>> MaxSize<TModel, TItem>(this IMemberSpecification<TModel, IEnumerable<TItem>> @this, long max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, IEnumerable<TItem>, TItem>(max, message);
        }

        public static IMemberSpecification<TModel, IEnumerable<TItem>> MinSize<TModel, TItem>(this IMemberSpecification<TModel, IEnumerable<TItem>> @this, int min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, IEnumerable<TItem>, TItem>(min, message);
        }

        public static IMemberSpecification<TModel, IEnumerable<TItem>> MinSize<TModel, TItem>(this IMemberSpecification<TModel, IEnumerable<TItem>> @this, long min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, IEnumerable<TItem>, TItem>(min, message);
        }

        public static IMemberSpecification<TModel, IEnumerable<TItem>> SizeBetween<TModel, TItem>(this IMemberSpecification<TModel, IEnumerable<TItem>> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, IEnumerable<TItem>, TItem>(min, max, message);
        }

        public static IMemberSpecification<TModel, IEnumerable<TItem>> SizeBetween<TModel, TItem>(this IMemberSpecification<TModel, IEnumerable<TItem>> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, IEnumerable<TItem>, TItem>(min, max, message);
        }
    }
}