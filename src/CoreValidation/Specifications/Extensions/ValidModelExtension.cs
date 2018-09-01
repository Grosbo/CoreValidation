using CoreValidation.Specifications;
using CoreValidation.Specifications.Rules;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidModelExtension
    {
        public static IMemberSpecificationBuilder<TModel, TMember> ValidModel<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember> @this, Specification<TMember> modelSpecification = null)
            where TModel : class
            where TMember : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, TMember>)@this;

            memberSpecification.AddRule(new ValidModelRule<TMember>(modelSpecification));

            return @this;
        }
    }
}