using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class AsModelExtension
    {
        /// <summary>
        /// Sets the validation logic for the member as a model.
        /// </summary>
        /// <typeparam name="TModel">Type of the parent model.</typeparam>
        /// <typeparam name="TMember">Type of the nested model.</typeparam>
        /// <param name="this"></param>
        /// <param name="modelSpecification">
        /// Specification for the member's type.
        /// If not provided, specification is acquired from validation context.
        /// If also not registered there, <see cref="SpecificationNotFoundException"/> exception is thrown.
        /// </param>
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