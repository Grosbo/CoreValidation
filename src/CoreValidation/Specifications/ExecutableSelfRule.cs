using System;
using System.Linq;
using CoreValidation.Errors;

namespace CoreValidation.Specifications
{
    internal sealed class ExecutableSelfRule<TModel> : IExecutableRule<TModel> where TModel : class
    {
        public ExecutableSelfRule(Predicate<TModel> isValid, Error error)
        {
            Rule = new ValidRelativeRule<TModel>(isValid, error);
        }

        public ValidRelativeRule<TModel> Rule { get; }

        public bool TryExecuteAndGetErrors(TModel model, IRulesExecutionContext rulesExecutionContext, int depth, out IErrorsCollection errorsCollection)
        {
            return Rule.TryGetErrors(model, rulesExecutionContext, out errorsCollection);
        }

        public void InsertErrors(ErrorsCollection targetErrorsCollection, IErrorsCollection errorsCollectionToInclude)
        {
            targetErrorsCollection.AddError(errorsCollectionToInclude.Errors.Single());
        }
    }
}