using System;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications.Rules
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

        public bool TryGetErrors(TMember memberValue, IExecutionContext executionContext, out IErrorsCollection errorsCollection)
        {
            if (executionContext == null)
            {
                throw new ArgumentNullException(nameof(executionContext));
            }

            if ((executionContext.ValidationStrategy == ValidationStrategy.Force) ||
                ((memberValue != null) && !IsValid(memberValue)))
            {
                errorsCollection = _errorInCollection ?? executionContext.DefaultErrorAsCollection;

                return true;
            }

            errorsCollection = ErrorsCollection.Empty;

            return false;
        }
    }
}