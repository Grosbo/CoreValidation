using System;
using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.Specifications.Commands
{
    internal abstract class AsRelativeRule : IRule
    {
        public string Name => "AsRelative";
        public Error RuleSingleError { get; set; }
    }

    internal sealed class AsRelativeRule<TModel> : AsRelativeRule
        where TModel : class
    {
        private IErrorsCollection _errorInCollection;

        public AsRelativeRule(Predicate<TModel> isValid, Error error = null)
        {
            IsValid = isValid ?? throw new ArgumentNullException(nameof(isValid));

            Error = error;
        }

        public Predicate<TModel> IsValid { get; }

        public Error Error { get; }

        public bool TryGetErrors(TModel model, IExecutionContext executionContext, ValidationStrategy validationStrategy, out IErrorsCollection errorsCollection)
        {
            if (executionContext == null)
            {
                throw new ArgumentNullException(nameof(executionContext));
            }

            if (_errorInCollection == null)
            {
                _errorInCollection = GetErrorInCollection();
            }

            if ((validationStrategy == ValidationStrategy.Force) || ((model != null) && !IsValid(model)))
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
                return ErrorsCollection.WithSingleOrNull(RuleSingleError);
            }

            if (RuleSingleError != null)
            {
                Error.Message = RuleSingleError.Message;
            }

            return ErrorsCollection.WithSingleOrNull(Error);
        }
    }
}