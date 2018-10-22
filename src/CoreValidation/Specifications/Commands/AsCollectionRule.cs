using System;
using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications.Commands
{
    internal abstract class AsCollectionRule : IRule
    {
        public Error RuleSingleError { get; set; }

        public string Name => "AsCollection";
        public abstract bool TryGetErrors(object model, object memberValue, IExecutionContext executionContext, ValidationStrategy validationStrategy, int depth, out IErrorsCollection errorsCollection);
    }

    internal sealed class AsCollectionRule<TModel, TItem> : AsCollectionRule
        where TModel : class
    {
        private IErrorsCollection _ruleSingleErrorInCollection;

        public AsCollectionRule(MemberSpecification<TModel, TItem> itemSpecification)
        {
            ItemSpecification = itemSpecification ?? throw new ArgumentNullException(nameof(itemSpecification));

            MemberValidator = MemberValidatorCreator.Create(itemSpecification);
        }

        public IMemberValidator MemberValidator { get; }

        public MemberSpecification<TModel, TItem> ItemSpecification { get; }

        public override bool TryGetErrors(object model, object memberValue, IExecutionContext executionContext, ValidationStrategy validationStrategy, int depth, out IErrorsCollection errorsCollection)
        {
            return TryGetErrors((TModel)model, (IEnumerable<TItem>)memberValue, executionContext, validationStrategy, depth, out errorsCollection);
        }

        public bool TryGetErrors(TModel model, IEnumerable<TItem> memberValue, IExecutionContext executionContext, ValidationStrategy validationStrategy, int depth, out IErrorsCollection errorsCollection)
        {
            errorsCollection = ValidatorExecutor.ExecuteCollectionMember(MemberValidator, model, memberValue, executionContext, validationStrategy, depth);

            if (!errorsCollection.IsEmpty && (RuleSingleError != null))
            {
                if (_ruleSingleErrorInCollection == null)
                {
                    _ruleSingleErrorInCollection = ErrorsCollection.WithSingleOrNull(RuleSingleError);
                }

                errorsCollection = _ruleSingleErrorInCollection;

                return true;
            }

            return !errorsCollection.IsEmpty;
        }
    }
}