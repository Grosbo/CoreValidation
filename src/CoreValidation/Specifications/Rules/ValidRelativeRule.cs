using System;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications.Rules
{
    public abstract class ValidRelativeRule
    {
    }

    internal sealed class ValidRelativeRule<TModel> : ValidRelativeRule, IRule
        where TModel : class
    {
        private readonly ErrorsCollection _errorInCollection;

        public ValidRelativeRule(Predicate<TModel> isValid, Error error = null)
        {
            IsValid = isValid ?? throw new ArgumentNullException(nameof(isValid));

            Error = error;

            if (error != null)
            {
                _errorInCollection = new ErrorsCollection();
                _errorInCollection.AddError(error);
            }
        }

        public Predicate<TModel> IsValid { get; }

        public Error Error { get; }

        public bool TryGetErrors(TModel model, IExecutionContext executionContext, out IErrorsCollection errorsCollection)
        {
            if (executionContext == null)
            {
                throw new ArgumentNullException(nameof(executionContext));
            }

            if ((executionContext.ValidationStrategy == ValidationStrategy.Force) ||
                ((model != null) && !IsValid(model)))
            {
                errorsCollection = _errorInCollection ?? executionContext.DefaultErrorAsCollection;

                return true;
            }

            errorsCollection = ErrorsCollection.Empty;

            return false;
        }
    }
}