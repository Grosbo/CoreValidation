using System;
using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications.Rules
{
    internal abstract class ValidCollectionRule
    {
        public abstract bool TryGetErrors(object model, object memberValue, IExecutionContext executionContext, int depth, out IErrorsCollection errorsCollection);
    }

    internal sealed class ValidCollectionRule<TModel, TItem> : ValidCollectionRule, IRule
        where TModel : class
    {
        public ValidCollectionRule(MemberSpecification<TModel, TItem> memberSpecification)
        {
            MemberSpecification = memberSpecification ?? throw new ArgumentNullException(nameof(memberSpecification));

            MemberValidator = MemberValidatorCreator.Create(memberSpecification);

            if (MemberValidator.Name != null)
            {
                throw new InvalidOperationException($"Cannot call {nameof(IMemberSpecificationBuilder<TModel, TItem>.WithName)} inside {nameof(ValidCollectionRule)}");
            }
        }

        public IMemberValidator MemberValidator { get; }

        public MemberSpecification<TModel, TItem> MemberSpecification { get; }

        public override bool TryGetErrors(object model, object memberValue, IExecutionContext executionContext, int depth, out IErrorsCollection errorsCollection)
        {
            return TryGetErrors((TModel)model, (IEnumerable<TItem>)memberValue, executionContext, depth, out errorsCollection);
        }

        public bool TryGetErrors(TModel model, IEnumerable<TItem> memberValue, IExecutionContext executionContext, int depth, out IErrorsCollection errorsCollection)
        {
            errorsCollection = ValidatorExecutor.ExecuteCollectionMember(MemberValidator, model, memberValue, executionContext, depth);

            return (errorsCollection != null) && !errorsCollection.IsEmpty;
        }
    }
}