using System;
using CoreValidation.Errors;

namespace CoreValidation.Specifications
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

        public bool TryGetErrors(TModel model, IRulesExecutionContext rulesExecutionContext, out IErrorsCollection errorsCollection)
        {
            if (rulesExecutionContext == null)
            {
                throw new ArgumentNullException(nameof(rulesExecutionContext));
            }

            if ((rulesExecutionContext.ValidationStrategy == ValidationStrategy.Force) ||
                ((model != null) && !IsValid(model)))
            {
                errorsCollection = _errorInCollection ?? rulesExecutionContext.DefaultErrorCollection;

                return true;
            }

            errorsCollection = ErrorsCollection.Empty;

            return false;
        }
    }
}