using System.Collections.Generic;
using System.Collections.ObjectModel;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Rules;


// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidModelsCollectionExtension
    {
        public static IMemberSpecificationBuilder<TModel, TMember> ValidModelsCollection<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, Specification<TItem> specification = null, bool itemsOptional = false)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
            where TItem : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, TMember>)@this;

            memberSpecification.AddRule(itemsOptional
                ? new ValidCollectionRule<TModel, TItem>(be => be.ValidModel(specification).Optional())
                : new ValidCollectionRule<TModel, TItem>(be => be.ValidModel(specification)));

            return @this;
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> ValidModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, Specification<TItem> specification = null, bool itemsOptional = false)
            where TModel : class
            where TItem : class
        {
            return @this.ValidModelsCollection<TModel, TItem[], TItem>(specification, itemsOptional);
        }

        public static IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> ValidModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> @this, Specification<TItem> specification = null, bool itemsOptional = false)
            where TModel : class
            where TItem : class
        {
            return @this.ValidModelsCollection<TModel, IEnumerable<TItem>, TItem>(specification, itemsOptional);
        }

        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> ValidModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, Specification<TItem> specification = null, bool itemsOptional = false)
            where TModel : class
            where TItem : class
        {
            return @this.ValidModelsCollection<TModel, ICollection<TItem>, TItem>(specification, itemsOptional);
        }

        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> ValidModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, Specification<TItem> specification = null, bool itemsOptional = false)
            where TModel : class
            where TItem : class
        {
            return @this.ValidModelsCollection<TModel, Collection<TItem>, TItem>(specification, itemsOptional);
        }

        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> ValidModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, Specification<TItem> specification = null, bool itemsOptional = false)
            where TModel : class
            where TItem : class
        {
            return @this.ValidModelsCollection<TModel, IReadOnlyCollection<TItem>, TItem>(specification, itemsOptional);
        }

        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> ValidModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, Specification<TItem> specification = null, bool itemsOptional = false)
            where TModel : class
            where TItem : class
        {
            return @this.ValidModelsCollection<TModel, ReadOnlyCollection<TItem>, TItem>(specification, itemsOptional);
        }

        public static IMemberSpecificationBuilder<TModel, IList<TItem>> ValidModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, Specification<TItem> specification = null, bool itemsOptional = false)
            where TModel : class
            where TItem : class
        {
            return @this.ValidModelsCollection<TModel, IList<TItem>, TItem>(specification, itemsOptional);
        }

        public static IMemberSpecificationBuilder<TModel, List<TItem>> ValidModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, Specification<TItem> specification = null, bool itemsOptional = false)
            where TModel : class
            where TItem : class
        {
            return @this.ValidModelsCollection<TModel, List<TItem>, TItem>(specification, itemsOptional);
        }
    }
}