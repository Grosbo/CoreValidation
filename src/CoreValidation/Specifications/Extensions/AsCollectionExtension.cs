using System.Collections.Generic;
using System.Collections.ObjectModel;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;


// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class AsCollectionExtension
    {
        public static IMemberSpecificationBuilder<TModel, TMember> AsCollection<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, MemberSpecification<TModel, TItem> itemSpecification)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, TMember>)@this;

            memberSpecification.AddCommand(new AsCollectionRule<TModel, TItem>(itemSpecification));

            return @this;
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> AsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, MemberSpecification<TModel, TItem> itemSpecification)
            where TModel : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, TItem[]>)@this;

            memberSpecification.AddCommand(new AsCollectionRule<TModel, TItem>(itemSpecification));

            return @this;
        }

        public static IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> AsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> @this, MemberSpecification<TModel, TItem> itemSpecification)
            where TModel : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, IEnumerable<TItem>>)@this;

            memberSpecification.AddCommand(new AsCollectionRule<TModel, TItem>(itemSpecification));

            return @this;
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> AsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, MemberSpecification<TModel, TItem> itemSpecification)
            where TModel : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, ICollection<TItem>>)@this;

            memberSpecification.AddCommand(new AsCollectionRule<TModel, TItem>(itemSpecification));

            return @this;
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> AsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, MemberSpecification<TModel, TItem> itemSpecification)
            where TModel : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, Collection<TItem>>)@this;

            memberSpecification.AddCommand(new AsCollectionRule<TModel, TItem>(itemSpecification));

            return @this;
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> AsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, MemberSpecification<TModel, TItem> itemSpecification)
            where TModel : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>>)@this;

            memberSpecification.AddCommand(new AsCollectionRule<TModel, TItem>(itemSpecification));

            return @this;
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> AsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, MemberSpecification<TModel, TItem> itemSpecification)
            where TModel : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>>)@this;

            memberSpecification.AddCommand(new AsCollectionRule<TModel, TItem>(itemSpecification));

            return @this;
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> AsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, MemberSpecification<TModel, TItem> itemSpecification)
            where TModel : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, IList<TItem>>)@this;

            memberSpecification.AddCommand(new AsCollectionRule<TModel, TItem>(itemSpecification));

            return @this;
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> AsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, MemberSpecification<TModel, TItem> itemSpecification)
            where TModel : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, List<TItem>>)@this;

            memberSpecification.AddCommand(new AsCollectionRule<TModel, TItem>(itemSpecification));

            return @this;
        }
    }
}