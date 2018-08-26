using System.Collections.Generic;
using CoreValidation.Specifications;
using CoreValidation.Validators;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidCollectionExtension
    {
        public static IMemberSpecificationBuilder<TModel, TMember> ValidCollection<TModel, TMember, TItem>(this IMemberSpecificationBuilder<TModel, TMember> @this, MemberValidator<TModel, TItem> itemValidator)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, TMember>) @this;

            memberSpecification.AddRule(new ValidCollectionRule<TModel, TItem>(itemValidator));

            return @this;
        }

        public static IMemberSpecificationBuilder<TModel, TItem[]> ValidCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, TItem[]> @this, MemberValidator<TModel, TItem> itemValidator)
            where TModel : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, TItem[]>) @this;

            memberSpecification.AddRule(new ValidCollectionRule<TModel, TItem>(itemValidator));

            return @this;
        }

        public static IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> ValidCollection<TModel, TItem>(this IMemberSpecificationBuilder<TModel, IEnumerable<TItem>> @this, MemberValidator<TModel, TItem> itemValidator)
            where TModel : class
        {
            var memberSpecification = (MemberSpecificationBuilder<TModel, IEnumerable<TItem>>) @this;

            memberSpecification.AddRule(new ValidCollectionRule<TModel, TItem>(itemValidator));

            return @this;
        }
    }
}