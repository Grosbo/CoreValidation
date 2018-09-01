using System;
using CoreValidation.Errors;
using CoreValidation.Options;

namespace CoreValidation.Validators
{
    internal sealed class ExecutionContext : IExecutionContext
    {
        private readonly Lazy<ErrorsCollection> _defaultErrorCollection;

        public ExecutionContext()
        {
            _defaultErrorCollection = new Lazy<ErrorsCollection>(() =>
            {
                if (ExecutionOptions?.DefaultError == null)
                {
                    return null;
                }

                var defaultErrorCollection = new ErrorsCollection();
                defaultErrorCollection.AddError(ExecutionOptions.DefaultError);

                return defaultErrorCollection;
            });
        }

        public IExecutionOptions ExecutionOptions { get; set; }
        public IValidatorsFactory ValidatorsFactory { get; set; }
        public ValidationStrategy ValidationStrategy { get; set; }

        public IErrorsCollection DefaultErrorAsCollection => _defaultErrorCollection.Value;
    }
}