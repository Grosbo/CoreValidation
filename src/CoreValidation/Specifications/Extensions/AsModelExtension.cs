using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class AsModelExtension
    {
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