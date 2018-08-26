using System;
using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications
{
    internal abstract class ValidCollectionRule
    {
        public abstract bool TryGetErrors(object model, object memberValue, IRulesExecutionContext rulesExecutionContext, int depth, out IErrorsCollection errorsCollection);
    }

    internal sealed class ValidCollectionRule<TModel, TItem> : ValidCollectionRule, IRule
        where TModel : class
    {
        public ValidCollectionRule(MemberValidator<TModel, TItem> memberValidator)
        {
            MemberValidator = memberValidator ?? throw new ArgumentNullException(nameof(memberValidator));

            MemberSpecification = MemberValidatorProcessor.Process(memberValidator);

            if (MemberSpecification.Name != null)
            {
                throw new InvalidOperationException($"Cannot call {nameof(IMemberSpecificationBuilder<TModel, TItem>.WithName)} inside {nameof(ValidCollectionRule)}");
            }
        }

        public IMemberSpecification MemberSpecification { get; }

        public MemberValidator<TModel, TItem> MemberValidator { get; }

        public override bool TryGetErrors(object model, object memberValue, IRulesExecutionContext rulesExecutionContext, int depth, out IErrorsCollection errorsCollection)
        {
            return TryGetErrors((TModel)model, (IEnumerable<TItem>)memberValue, rulesExecutionContext, depth, out errorsCollection);
        }

        public bool TryGetErrors(TModel model, IEnumerable<TItem> memberValue, IRulesExecutionContext rulesExecutionContext, int depth, out IErrorsCollection errorsCollection)
        {
            errorsCollection = SpecificationRulesExecutor.ExecuteCollectionMemberSpecificationRules(MemberSpecification, model, memberValue, rulesExecutionContext, depth);

            return (errorsCollection != null) && !errorsCollection.IsEmpty;
        }
    }
}