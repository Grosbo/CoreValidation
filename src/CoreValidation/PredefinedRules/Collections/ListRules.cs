using System.Collections.Generic;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ListRules
    {
        public static IMemberSpecification<TModel, List<TItem>> Empty<TModel, TItem>(this IMemberSpecification<TModel, List<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.Empty<TModel, List<TItem>, TItem>(message);
        }

        public static IMemberSpecification<TModel, List<TItem>> NotEmpty<TModel, TItem>(this IMemberSpecification<TModel, List<TItem>> @this, string message = null)
            where TModel : class
        {
            return @this.NotEmpty<TModel, List<TItem>, TItem>(message);
        }

        public static IMemberSpecification<TModel, List<TItem>> ExactSize<TModel, TItem>(this IMemberSpecification<TModel, List<TItem>> @this, int size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, List<TItem>, TItem>(size, message);
        }

        public static IMemberSpecification<TModel, List<TItem>> ExactSize<TModel, TItem>(this IMemberSpecification<TModel, List<TItem>> @this, long size, string message = null)
            where TModel : class
        {
            return @this.ExactSize<TModel, List<TItem>, TItem>(size, message);
        }

        public static IMemberSpecification<TModel, List<TItem>> MaxSize<TModel, TItem>(this IMemberSpecification<TModel, List<TItem>> @this, int max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, List<TItem>, TItem>(max, message);
        }

        public static IMemberSpecification<TModel, List<TItem>> MaxSize<TModel, TItem>(this IMemberSpecification<TModel, List<TItem>> @this, long max, string message = null)
            where TModel : class
        {
            return @this.MaxSize<TModel, List<TItem>, TItem>(max, message);
        }

        public static IMemberSpecification<TModel, List<TItem>> MinSize<TModel, TItem>(this IMemberSpecification<TModel, List<TItem>> @this, int min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, List<TItem>, TItem>(min, message);
        }

        public static IMemberSpecification<TModel, List<TItem>> MinSize<TModel, TItem>(this IMemberSpecification<TModel, List<TItem>> @this, long min, string message = null)
            where TModel : class
        {
            return @this.MinSize<TModel, List<TItem>, TItem>(min, message);
        }

        public static IMemberSpecification<TModel, List<TItem>> SizeBetween<TModel, TItem>(this IMemberSpecification<TModel, List<TItem>> @this, int min, int max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, List<TItem>, TItem>(min, max, message);
        }

        public static IMemberSpecification<TModel, List<TItem>> SizeBetween<TModel, TItem>(this IMemberSpecification<TModel, List<TItem>> @this, long min, long max, string message = null)
            where TModel : class
        {
            return @this.SizeBetween<TModel, List<TItem>, TItem>(min, max, message);
        }
    }
}