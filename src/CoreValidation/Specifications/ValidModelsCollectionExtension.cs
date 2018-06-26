using System.Collections.Generic;
using System.Collections.ObjectModel;
using CoreValidation.Specifications;
using CoreValidation.Validators;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidModelsCollectionExtension
    {
        public static IMemberSpecification<TModel, TMember> ValidModelsCollection<TModel, TMember, TItem>(this IMemberSpecification<TModel, TMember> @this, Validator<TItem> validator = null, bool itemsOptional = false)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
            where TItem: class
        {
            var memberSpecification = (MemberSpecification<TModel, TMember>) @this;

            memberSpecification.AddRule(itemsOptional
                ? new ValidCollectionRule<TModel, TItem>(be => be.ValidModel(validator).Optional())
                : new ValidCollectionRule<TModel, TItem>(be => be.ValidModel(validator)));

            return @this;
        }

        public static IMemberSpecification<TModel, TItem[]> ValidModelsCollection<TModel, TItem>(this IMemberSpecification<TModel, TItem[]> @this, Validator<TItem> validator = null, bool itemsOptional = false)
            where TModel : class
            where TItem: class
        {
            return @this.ValidModelsCollection<TModel, TItem[], TItem>(validator, itemsOptional);
        }

        public static IMemberSpecification<TModel, IEnumerable<TItem>> ValidModelsCollection<TModel, TItem>(this IMemberSpecification<TModel, IEnumerable<TItem>> @this, Validator<TItem> validator = null, bool itemsOptional = false)
            where TModel : class
            where TItem: class
        {
            return @this.ValidModelsCollection<TModel, IEnumerable<TItem>, TItem>(validator, itemsOptional);
        }

        public static IMemberSpecification<TModel, ICollection<TItem>> ValidModelsCollection<TModel, TItem>(this IMemberSpecification<TModel, ICollection<TItem>> @this, Validator<TItem> validator = null, bool itemsOptional = false)
            where TModel : class
            where TItem: class
        {
            return @this.ValidModelsCollection<TModel, ICollection<TItem>, TItem>(validator, itemsOptional);
        }

        public static IMemberSpecification<TModel, Collection<TItem>> ValidModelsCollection<TModel, TItem>(this IMemberSpecification<TModel, Collection<TItem>> @this, Validator<TItem> validator = null, bool itemsOptional = false)
            where TModel : class
            where TItem: class
        {
            return @this.ValidModelsCollection<TModel, Collection<TItem>, TItem>(validator, itemsOptional);
        }

        public static IMemberSpecification<TModel, IReadOnlyCollection<TItem>> ValidModelsCollection<TModel, TItem>(this IMemberSpecification<TModel, IReadOnlyCollection<TItem>> @this, Validator<TItem> validator = null, bool itemsOptional = false)
            where TModel : class
            where TItem: class
        {
            return @this.ValidModelsCollection<TModel, IReadOnlyCollection<TItem>, TItem>(validator, itemsOptional);
        }

        public static IMemberSpecification<TModel, ReadOnlyCollection<TItem>> ValidModelsCollection<TModel, TItem>(this IMemberSpecification<TModel, ReadOnlyCollection<TItem>> @this, Validator<TItem> validator = null, bool itemsOptional = false)
            where TModel : class
            where TItem: class
        {
            return @this.ValidModelsCollection<TModel, ReadOnlyCollection<TItem>, TItem>(validator, itemsOptional);
        }

        public static IMemberSpecification<TModel, IList<TItem>> ValidModelsCollection<TModel, TItem>(this IMemberSpecification<TModel, IList<TItem>> @this, Validator<TItem> validator = null, bool itemsOptional = false)
            where TModel : class
            where TItem: class
        {
            return @this.ValidModelsCollection<TModel, IList<TItem>, TItem>(validator, itemsOptional);
        }

        public static IMemberSpecification<TModel, List<TItem>> ValidModelsCollection<TModel, TItem>(this IMemberSpecification<TModel, List<TItem>> @this, Validator<TItem> validator = null, bool itemsOptional = false)
            where TModel : class
            where TItem: class
        {
            return @this.ValidModelsCollection<TModel, List<TItem>, TItem>(validator, itemsOptional);
        }
    }
}