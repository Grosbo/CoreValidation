using System.Collections.Generic;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    // ReSharper disable once InconsistentNaming
    public static class ICollectionRules
    {
        public static IMemberSpecification<TModel, ICollection<TItem>> Empty<TModel, TItem>(this IMemberSpecification<TModel, ICollection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.Empty<TModel, ICollection<TItem>, TItem>(message);
        }

        public static IMemberSpecification<TModel, ICollection<TItem>> NotEmpty<TModel, TItem>(this IMemberSpecification<TModel, ICollection<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.NotEmpty<TModel, ICollection<TItem>, TItem>(message);
        }

        public static IMemberSpecification<TModel, ICollection<TItem>> ExactSize<TModel, TItem>(this IMemberSpecification<TModel, ICollection<TItem>> @this, int size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, ICollection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecification<TModel, ICollection<TItem>> ExactSize<TModel, TItem>(this IMemberSpecification<TModel, ICollection<TItem>> @this, long size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, ICollection<TItem>, TItem>(size, message);
        }

        public static IMemberSpecification<TModel, ICollection<TItem>> MaxSize<TModel, TItem>(this IMemberSpecification<TModel, ICollection<TItem>> @this, int max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, ICollection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecification<TModel, ICollection<TItem>> MaxSize<TModel, TItem>(this IMemberSpecification<TModel, ICollection<TItem>> @this, long max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, ICollection<TItem>, TItem>(max, message);
        }

        public static IMemberSpecification<TModel, ICollection<TItem>> MinSize<TModel, TItem>(this IMemberSpecification<TModel, ICollection<TItem>> @this, int min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, ICollection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecification<TModel, ICollection<TItem>> MinSize<TModel, TItem>(this IMemberSpecification<TModel, ICollection<TItem>> @this, long min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, ICollection<TItem>, TItem>(min, message);
        }

        public static IMemberSpecification<TModel, ICollection<TItem>> SizeBetween<TModel, TItem>(this IMemberSpecification<TModel, ICollection<TItem>> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, ICollection<TItem>, TItem>(min, max, message);
        }

        public static IMemberSpecification<TModel, ICollection<TItem>> SizeBetween<TModel, TItem>(this IMemberSpecification<TModel, ICollection<TItem>> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, ICollection<TItem>, TItem>(min, max, message);
        }
    }
}