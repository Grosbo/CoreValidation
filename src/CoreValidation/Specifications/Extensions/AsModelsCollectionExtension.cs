using System.Collections.Generic;
using System.Collections.ObjectModel;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;


// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class AsModelsCollectionExtension
    {
        /// <summary>
        /// Sets the validation logic for the member as a collection of nested models.
        /// </summary>
        /// <typeparam name="TModel">Type of the parent model.</typeparam>
        /// <typeparam name="TMember">Type of the collection.</typeparam>
        /// <typeparam name="TItem">Type of the nested models in the collection.</typeparam>
        /// <param name="itemSpecification">The specification for the single nested model in the collection.</param>
        public static IMemberSpecificationBuilder<TModel, TMember> AsModelsCollection<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, Specification<TItem> specification = null)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
            where TItem : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, TMember>)@this;

            memberSpecification.AddCommand(new AsCollectionRule<TModel, TItem>(be => be.AsModel(specification)));

            return @this;
        }

        /// <summary>
        /// Sets the validation logic for the member as a collection of nested models.
        /// </summary>
        /// <typeparam name="TModel">Type of the parent model.</typeparam>
        /// <typeparam name="TItem">Type of the nested models in the collection.</typeparam>
        /// <param name="itemSpecification">The specification for the single nested model in the collection.</param>
        public static IMemberSpecificationBuilder<TModel, TItem[]> AsModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, Specification<TItem> specification = null)
            where TModel : class
            where TItem : class
        {
            return @this.AsModelsCollection<TModel, TItem[], TItem>(specification);
        }

        /// <summary>
        /// Sets the validation logic for the member as a collection of nested models.
        /// </summary>
        /// <typeparam name="TModel">Type of the parent model.</typeparam>
        /// <typeparam name="TItem">Type of the nested models in the collection.</typeparam>
        /// <param name="itemSpecification">The specification for the single nested model in the collection.</param>
        public static IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> AsModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> @this, Specification<TItem> specification = null)
            where TModel : class
            where TItem : class
        {
            return @this.AsModelsCollection<TModel, IEnumerable<TItem>, TItem>(specification);
        }

        /// <summary>
        /// Sets the validation logic for the member as a collection of nested models.
        /// </summary>
        /// <typeparam name="TModel">Type of the parent model.</typeparam>
        /// <typeparam name="TItem">Type of the nested models in the collection.</typeparam>
        /// <param name="itemSpecification">The specification for the single nested model in the collection.</param>
        public static IMemberSpecificationBuilder<TModel, ICollection<TItem>> AsModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ICollection<TItem>> @this, Specification<TItem> specification = null)
            where TModel : class
            where TItem : class
        {
            return @this.AsModelsCollection<TModel, ICollection<TItem>, TItem>(specification);
        }

        /// <summary>
        /// Sets the validation logic for the member as a collection of nested models.
        /// </summary>
        /// <typeparam name="TModel">Type of the parent model.</typeparam>
        /// <typeparam name="TItem">Type of the nested models in the collection.</typeparam>
        /// <param name="itemSpecification">The specification for the single nested model in the collection.</param>
        public static IMemberSpecificationBuilder<TModel, Collection<TItem>> AsModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, Collection<TItem>> @this, Specification<TItem> specification = null)
            where TModel : class
            where TItem : class
        {
            return @this.AsModelsCollection<TModel, Collection<TItem>, TItem>(specification);
        }

        /// <summary>
        /// Sets the validation logic for the member as a collection of nested models.
        /// </summary>
        /// <typeparam name="TModel">Type of the parent model.</typeparam>
        /// <typeparam name="TItem">Type of the nested models in the collection.</typeparam>
        /// <param name="itemSpecification">The specification for the single nested model in the collection.</param>
        public static IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> AsModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IReadOnlyCollection<TItem>> @this, Specification<TItem> specification = null)
            where TModel : class
            where TItem : class
        {
            return @this.AsModelsCollection<TModel, IReadOnlyCollection<TItem>, TItem>(specification);
        }

        /// <summary>
        /// Sets the validation logic for the member as a collection of nested models.
        /// </summary>
        /// <typeparam name="TModel">Type of the parent model.</typeparam>
        /// <typeparam name="TItem">Type of the nested models in the collection.</typeparam>
        /// <param name="itemSpecification">The specification for the single nested model in the collection.</param>
        public static IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> AsModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, ReadOnlyCollection<TItem>> @this, Specification<TItem> specification = null)
            where TModel : class
            where TItem : class
        {
            return @this.AsModelsCollection<TModel, ReadOnlyCollection<TItem>, TItem>(specification);
        }

        /// <summary>
        /// Sets the validation logic for the member as a collection of nested models.
        /// </summary>
        /// <typeparam name="TModel">Type of the parent model.</typeparam>
        /// <typeparam name="TItem">Type of the nested models in the collection.</typeparam>
        /// <param name="itemSpecification">The specification for the single nested model in the collection.</param>
        public static IMemberSpecificationBuilder<TModel, IList<TItem>> AsModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IList<TItem>> @this, Specification<TItem> specification = null)
            where TModel : class
            where TItem : class
        {
            return @this.AsModelsCollection<TModel, IList<TItem>, TItem>(specification);
        }

        /// <summary>
        /// Sets the validation logic for the member as a collection of nested models.
        /// </summary>
        /// <typeparam name="TModel">Type of the parent model.</typeparam>
        /// <typeparam name="TItem">Type of the nested models in the collection.</typeparam>
        /// <param name="itemSpecification">The specification for the single nested model in the collection.</param>
        public static IMemberSpecificationBuilder<TModel, List<TItem>> AsModelsCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, List<TItem>> @this, Specification<TItem> specification = null)
            where TModel : class
            where TItem : class
        {
            return @this.AsModelsCollection<TModel, List<TItem>, TItem>(specification);
        }
    }
}