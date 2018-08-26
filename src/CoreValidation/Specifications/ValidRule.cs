using System;
using CoreValidation.Errors;

namespace CoreValidation.Specifications
{
    internal abstract class ValidRule
    {
    }

    internal sealed class ValidRule<TMember> : ValidRule, IRule
    {
        private readonly ErrorsCollection _errorInCollection;

        public ValidRule(Predicate<TMember> isValid, Error error = null)
        {
            IsValid = isValid ?? throw new ArgumentNullException(nameof(isValid));

            Error = error;

            if (error != null)
            {
                _errorInCollection = new ErrorsCollection();
                _errorInCollection.AddError(error);
            }
        }

        public Predicate<TMember> IsValid { get; }

        public Error Error { get; }

        public bool TryGetErrors(TMember memberValue, IRulesExecutionContext rulesExecutionContext, out IErrorsCollection errorsCollection)
        {
            if (rulesExecutionContext == null)
            {
                throw new ArgumentNullException(nameof(rulesExecutionContext));
            }

            if ((rulesExecutionContext.ValidationStrategy == ValidationStrategy.Force) ||
                ((memberValue != null) && !IsValid(memberValue)))
            {
                errorsCollection = _errorInCollection ?? rulesExecutionContext.DefaultErrorCollection;

                return true;
            }

            errorsCollection = ErrorsCollection.Empty;

            return false;
        }
    }
}