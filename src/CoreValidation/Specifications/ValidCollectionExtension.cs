using System.Collections.Generic;
using CoreValidation.Specifications;
using CoreValidation.Validators;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidCollectionExtension
    {
        public static IMemberSpecification<TModel, TMember> ValidCollection<TModel, TMember, TItem>(this IMemberSpecification<TModel, TMember> @this, MemberValidator<TModel, TItem> itemValidator)
            where TModel : class
            where TMember : class, IEnumerable<TItem>
        {
            var memberSpecification = (MemberSpecification<TModel, TMember>) @this;

            memberSpecification.AddRule(new ValidCollectionRule<TModel, TItem>(itemValidator));

            return @this;
        }

        public static IMemberSpecification<TModel, TItem[]> ValidCollection<TModel, TItem>(this IMemberSpecification<TModel, TItem[]> @this, MemberValidator<TModel, TItem> itemValidator)
            where TModel : class
        {
            var memberSpecification = (MemberSpecification<TModel, TItem[]>) @this;

            memberSpecification.AddRule(new ValidCollectionRule<TModel, TItem>(itemValidator));

            return @this;
        }

        public static IMemberSpecification<TModel, IEnumerable<TItem>> ValidCollection<TModel, TItem>(this IMemberSpecification<TModel, IEnumerable<TItem>> @this, MemberValidator<TModel, TItem> itemValidator)
            where TModel : class
        {
            var memberSpecification = (MemberSpecification<TModel, IEnumerable<TItem>>) @this;

            memberSpecification.AddRule(new ValidCollectionRule<TModel, TItem>(itemValidator));

            return @this;
        }
    }
}