using CoreValidation.Specifications;
using CoreValidation.Validators;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidModelExtension
    {
        public static IMemberSpecificationBuilder<TModel, TMember> ValidModel<TModel, TMember>(this IMemberSpecificationBuilder<TModel, TMember> @this, Validator<TMember> nestedValidator = null)
            where TModel : class
            where TMember : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, TMember>) @this;

            memberSpecification.AddRule(new ValidModelRule<TMember>(nestedValidator));

            return @this;
        }
    }
}