using System;
using CoreValidation.Errors;
using CoreValidation.Options;

namespace CoreValidation.Validators
{
    internal sealed class ExecutionContext : IExecutionContext
    {
        private readonly Lazy<ErrorsCollection> _defaultErrorCollection;

        public ExecutionContext(IExecutionOptions executionOptions, IValidatorsFactory validatorsFactory)
        {
            CollectionForceKey = executionOptions.CollectionForceKey;
            RequiredError = executionOptions.RequiredError;
            DefaultError = executionOptions.DefaultError;
            MaxDepth = executionOptions.MaxDepth;
            ValidatorsFactory = validatorsFactory;

            _defaultErrorCollection = new Lazy<ErrorsCollection>(() =>
            {
                if (DefaultError == null)
                {
                    return null;
                }

                var defaultErrorCollection = new ErrorsCollection();
                defaultErrorCollection.AddError(DefaultError);

                return defaultErrorCollection;
            });
        }

        public IValidatorsFactory ValidatorsFactory { get; set; }

        public IErrorsCollection DefaultErrorAsCollection => _defaultErrorCollection.Value;

        public string CollectionForceKey { get; }
        public IError RequiredError { get; }
        public IError DefaultError { get; }
        public int MaxDepth { get; }
    }
}