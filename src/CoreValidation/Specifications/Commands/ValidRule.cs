using System;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications.Commands
{
    internal abstract class ValidRule : IRule
    {
        public string Name => "Valid";
        public Error RuleSingleError { get; set; }
    }

    internal sealed class ValidRule<TMember> : ValidRule
    {
        private IErrorsCollection _errorInCollection;

        private bool _errorInCollectionEvaluated;

        public ValidRule(Predicate<TMember> isValid, Error error = null)
        {
            IsValid = isValid ?? throw new ArgumentNullException(nameof(isValid));

            Error = error;
        }

        public Predicate<TMember> IsValid { get; }

        public Error Error { get; }

        public bool TryGetErrors(TMember memberValue, IExecutionContext executionContext, ValidationStrategy validationStrategy, out IErrorsCollection errorsCollection)
        {
            if (executionContext == null)
            {
                throw new ArgumentNullException(nameof(executionContext));
            }

            if (!_errorInCollectionEvaluated)
            {
                _errorInCollection = GetErrorInCollection();
            }

            if ((validationStrategy == ValidationStrategy.Force) || ((memberValue != null) && !IsValid(memberValue)))
            {
                errorsCollection = _errorInCollection ?? executionContext.DefaultErrorAsCollection;

                return true;
            }

            errorsCollection = ErrorsCollection.Empty;

            return false;
        }

        private IErrorsCollection GetErrorInCollection()
        {
            if (Error == null)
            {
                _errorInCollectionEvaluated = true;

                return ErrorsCollection.WithSingleOrNull(RuleSingleError);
            }

            if (RuleSingleError != null)
            {
                Error.Message = RuleSingleError.Message;
            }

            _errorInCollectionEvaluated = true;

            return ErrorsCollection.WithSingleOrNull(Error);
        }
    }
}