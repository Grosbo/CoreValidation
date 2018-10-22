using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class BaseCollectionRules
    {
        public static IMemberSpecificationBuilder<TModel, TMember> EmptyCollection<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            return @this.Valid(m => !m.Any(), Phrases.Keys.Collections.EmptyCollection);
        }

        public static IMemberSpecificationBuilder<TModel, TMember> NotEmptyCollection<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            return @this.Valid(m => m.Any(), Phrases.Keys.Collections.NotEmptyCollection);
        }

        public static IMemberSpecificationBuilder<TModel, TMember> ExactCollectionSize<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, int size)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, "Exact size cannot be less than zero");
            }

            return @this.Valid(m => m.Count() == size, Phrases.Keys.Collections.ExactCollectionSize, new[] {NumberArg.Create(nameof(size), size)});
        }

        public static IMemberSpecificationBuilder<TModel, TMember> ExactCollectionSize<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, long size)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, "Exact size cannot be less than zero");
            }

            return @this.Valid(m => m.LongCount() == size, Phrases.Keys.Collections.ExactCollectionSize, new[] {NumberArg.Create(nameof(size), size)});
        }

        public static IMemberSpecificationBuilder<TModel, TMember> MaxCollectionSize<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, int max)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Max size cannot be less than zero");
            }

            return @this.Valid(m => m.Count() <= max, Phrases.Keys.Collections.MaxCollectionSize, new[] {NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, TMember> MaxCollectionSize<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, long max)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Max size cannot be less than zero");
            }

            return @this.Valid(m => m.LongCount() <= max, Phrases.Keys.Collections.MaxCollectionSize, new[] {NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, TMember> MinCollectionSize<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, int min)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, "Max size cannot be less than zero");
            }

            return @this.Valid(m => m.Count() >= min, Phrases.Keys.Collections.MinCollectionSize, new[] {NumberArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, TMember> MinCollectionSize<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, long min)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, "Min size cannot be less than zero");
            }

            return @this.Valid(m => m.LongCount() >= min, Phrases.Keys.Collections.MinCollectionSize, new[] {NumberArg.Create(nameof(min), min)});
        }

        public static IMemberSpecificationBuilder<TModel, TMember> CollectionSizeBetween<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, int min, int max)
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
                Phrases.Keys.Collections.CollectionSizeBetween,
                new[] {NumberArg.Create(nameof(min), min), NumberArg.Create(nameof(max), max)});
        }

        public static IMemberSpecificationBuilder<TModel, TMember> CollectionSizeBetween<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, long min, long max)
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
                }, Phrases.Keys.Collections.CollectionSizeBetween,
                new[] {NumberArg.Create(nameof(min), min), NumberArg.Create(nameof(max), max)});
        }
    }
}