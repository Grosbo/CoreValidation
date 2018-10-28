using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class AsModelExtension
    {
        /// <summary>
        /// Sets the validation logic for the member as a nested model.
        /// If the member is null, the validation logic is not triggered and no error is added.
        /// </summary>
        /// <typeparam name="TModel">Type of the parent model.</typeparam>
        /// <typeparam name="TMember">Type of the nested model.</typeparam>
        /// <param name="modelSpecification">The specification for the nested model's type <typeparamref name="TMember"/>. If null, using specification registered in the related ValidationContext.</param>
        public static IMemberSpecificationBuilder<TModel, TMember> AsModel<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember> @this, Specification<TMember> modelSpecification = null)
            where TModel : class
            where TMember : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, TMember>)@this;

            memberSpecification.AddCommand(new AsModelRule<TMember>(modelSpecification));

            return @this;
        }
    }
}