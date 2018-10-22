using System;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications.Commands
{
    internal sealed class ModelScope<TModel> : IScope<TModel> where TModel : class
    {
        public ModelScope(Predicate<TModel> isValid)
        {
            Rule = new AsRelativeRule<TModel>(isValid);
        }

        public AsRelativeRule<TModel> Rule { get; }

        public bool TryGetErrors(TModel model, IExecutionContext executionContext, ValidationStrategy validationStrategy, int depth, out IErrorsCollection scopeErrorsCollection)
        {
            var anyErrors = Rule.TryGetErrors(model, executionContext, validationStrategy, out var errorsCollection);

            if (!errorsCollection.IsEmpty && (RuleSingleError != null))
            {
                scopeErrorsCollection = ErrorsCollection.WithSingleOrNull(RuleSingleError);

                return true;
            }

            scopeErrorsCollection = errorsCollection;

            return anyErrors;
        }

        public void InsertErrors(ErrorsCollection targetErrorsCollection, IErrorsCollection scopeErrorsCollection)
        {
            if (targetErrorsCollection == null)
            {
                throw new ArgumentNullException(nameof(targetErrorsCollection));
            }

            if (scopeErrorsCollection == null)
            {
                throw new ArgumentNullException(nameof(scopeErrorsCollection));
            }

            targetErrorsCollection.AddError(scopeErrorsCollection.Errors.Single());
        }

        public Error RuleSingleError { get; set; }

        public string Name => "Valid";
    }
}