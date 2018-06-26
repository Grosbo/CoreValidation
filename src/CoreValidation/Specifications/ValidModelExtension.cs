using CoreValidation.Specifications;
using CoreValidation.Validators;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidModelExtension
    {
        public static IMemberSpecification<TModel, TMember> ValidModel<TModel, TMember>(this IMemberSpecification<TModel, TMember> @this, Validator<TMember> nestedValidator = null)
            where TModel : class
            where TMember : class
        {
            var memberSpecification = (MemberSpecification<TModel, TMember>) @this;

            memberSpecification.AddRule(new ValidModelRule<TMember>(nestedValidator));

            return @this;
        }
    }
}