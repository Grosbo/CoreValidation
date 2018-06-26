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
        public static IMemberSpecification<TModel, TMember> Empty<TModel, TMember, TItem>(this IMemberSpecification<TModel, TMember> @this, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            return @this.Valid(m => !m.Any(), message ?? Phrases.Keys.Collections.Empty);
        }

        public static IMemberSpecification<TModel, TMember> NotEmpty<TModel, TMember, TItem>(this IMemberSpecification<TModel, TMember> @this, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            return @this.Valid(m => m.Any(), message ?? Phrases.Keys.Collections.NotEmpty);
        }

        public static IMemberSpecification<TModel, TMember> ExactSize<TModel, TMember, TItem>(this IMemberSpecification<TModel, TMember> @this, int size, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, "Exact size cannot be less than zero");
            }

            return @this.Valid(m => m.Count() == size, message ?? Phrases.Keys.Collections.ExactSize, new[] {new NumberArg(nameof(size), size)});
        }

        public static IMemberSpecification<TModel, TMember> ExactSize<TModel, TMember, TItem>(this IMemberSpecification<TModel, TMember> @this, long size, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), size, "Exact size cannot be less than zero");
            }

            return @this.Valid(m => m.LongCount() == size, message ?? Phrases.Keys.Collections.ExactSize, new[] {new NumberArg(nameof(size), size)});
        }

        public static IMemberSpecification<TModel, TMember> MaxSize<TModel, TMember, TItem>(this IMemberSpecification<TModel, TMember> @this, int max, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Max size cannot be less than zero");
            }

            return @this.Valid(m => m.Count() <= max, message ?? Phrases.Keys.Collections.MaxSize, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, TMember> MaxSize<TModel, TMember, TItem>(this IMemberSpecification<TModel, TMember> @this, long max, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (max < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), max, "Max size cannot be less than zero");
            }

            return @this.Valid(m => m.LongCount() <= max, message ?? Phrases.Keys.Collections.MaxSize, new[] {new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, TMember> MinSize<TModel, TMember, TItem>(this IMemberSpecification<TModel, TMember> @this, int min, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, "Max size cannot be less than zero");
            }

            return @this.Valid(m => m.Count() >= min, message ?? Phrases.Keys.Collections.MinSize, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecification<TModel, TMember> MinSize<TModel, TMember, TItem>(this IMemberSpecification<TModel, TMember> @this, long min, string message = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            if (min < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(min), min, "Min size cannot be less than zero");
            }

            return @this.Valid(m => m.LongCount() >= min, message ?? Phrases.Keys.Collections.MinSize, new[] {new NumberArg(nameof(min), min)});
        }

        public static IMemberSpecification<TModel, TMember> SizeBetween<TModel, TMember, TItem>(this IMemberSpecification<TModel, TMember> @this, int min, int max, string message = null)
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
                message ?? Phrases.Keys.Collections.SizeBetween,
                new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }

        public static IMemberSpecification<TModel, TMember> SizeBetween<TModel, TMember, TItem>(this IMemberSpecification<TModel, TMember> @this, long min, long max, string message = null)
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
                }, message ?? Phrases.Keys.Collections.SizeBetween,
                new[] {new NumberArg(nameof(min), min), new NumberArg(nameof(max), max)});
        }
    }
}