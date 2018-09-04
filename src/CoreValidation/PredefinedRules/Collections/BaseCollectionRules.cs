using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class BaseCollectionRules
    {
        public static IMemberSpecificationBuilder<TModel, TMember> EmptyCollection<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            return @this.Valid(m => !m.Any(), message ?? Phrases.Keys.Collections.EmptyCollection);
        }

        public static IMemberSpecificationBuilder<TModel, TMember> NotEmptyCollection<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            return @this.Valid(m => m.Any(), message ?? Phrases.Keys.Collections.NotEmptyCollection);
        }

        public static IMemberSpecificationBuilder<TModel, TMember> ExactCollectionSize<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, int size, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, "Exact size cannot be less than zero");
            }

            return @this.Valid(m => m.Count() == size, message ?? Phrases.Keys.Collections.ExactCollectionSize, new[] {new NumberArg(nameof(size), size)});
        }

        public static IMemberSpecificationBuilder<TModel, TMember> ExactCollectionSize<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, long size, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, "Exact size cannot be less than zero");
            }

            return @this.Valid(m => m.LongCount() == size, message ?? Phrases.Keys.Collections.ExactCollectionSize, new[] {new NumberArg(nameof(size), size)});
        }

        public static IMemberSpecificationBuilder<TModel, TMember> MaxCollectionSize<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, int max, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Max size cannot be less than zero");
            }

            return @this.Valid(m => m.Count() <= max, message ?? Phrases.Keys.Collections.MaxCollectionSize, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, TMember> MaxCollectionSize<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, long max, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Max size cannot be less than zero");
            }

            return @this.Valid(m => m.LongCount() <= max, message ?? Phrases.Keys.Collections.MaxCollectionSize, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, TMember> MinCollectionSize<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, int min, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, "Max size cannot be less than zero");
            }

            return @this.Valid(m => m.Count() >= min, message ?? Phrases.Keys.Collections.MinCollectionSize, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, TMember> MinCollectionSize<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, long min, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, "Min size cannot be less than zero");
            }

            return @this.Valid(m => m.LongCount() >= min, message ?? Phrases.Keys.Collections.MinCollectionSize, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, TMember> CollectionSizeBetween<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, int min, int max, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, "Min size cannot be less than zero");
            }

            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Max size cannot be less than zero");
            }

            if (max < min)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, $"Max size cannot be less than min value {min}");
            }

            return @this.Valid(m =>
                {
                    var count = m.Count();

                    return (count >= min) && (count <= max);
                },
                message ?? Phrases.Keys.Collections.CollectionSizeBetween,
                new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, TMember> CollectionSizeBetween<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, long min, long max, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, "Min size cannot be less than zero");
            }

            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Max size cannot be less than zero");
            }

            if (max < min)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, $"Max size cannot be less than min value {min}");
            }

            return @this.Valid(m =>
                {
                    var count = m.LongCount();

                    return (count >= min) && (count <= max);
                }, message ?? Phrases.Keys.Collections.CollectionSizeBetween,
                new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }
    }
}