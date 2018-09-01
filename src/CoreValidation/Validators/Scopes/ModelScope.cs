using System;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Specifications.Rules;

namespace CoreValidation.Validators.Scopes
{
    internal sealed class ModelScope<TModel> : IValidationScope<TModel> where TModel : class
    {
        public ModelScope(Predicate<TModel> isValid, Error error)
        {
            Rule = new ValidRelativeRule<TModel>(isValid, error);
        }

        public ValidRelativeRule<TModel> Rule { get; }

        public bool TryGetErrors(TModel model, IExecutionContext executionContext, int depth, out IErrorsCollection scopeErrorsCollection)
        {
            return Rule.TryGetErrors(model, executionContext, out scopeErrorsCollection);
        }

        public void InsertErrors(ErrorsCollection targetErrorsCollection, IErrorsCollection scopeErrorsCollection)
        {
            targetErrorsCollection.AddError(scopeErrorsCollection.Errors.Single());
        }
    }
}