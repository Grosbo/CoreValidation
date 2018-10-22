using CoreValidation.Errors;
using CoreValidation.Validators;

namespace CoreValidation.UnitTests
{
    internal class ExecutionContextStub : IExecutionContext
    {
        public string CollectionForceKey { get; set; } = "*";

        public IError RequiredError { get; set; } = new Error("Required");

        public IError DefaultError { get; set; } = new Error("Invalid");

        public int MaxDepth { get; set; } = 10;

        public IValidatorsFactory ValidatorsFactory { get; set; }

        public IErrorsCollection DefaultErrorAsCollection
        {
            get
            {
                var defaultErrorCollection = new ErrorsCollection();
                defaultErrorCollection.AddError(DefaultError);

                return defaultErrorCollection;
            }
        }
    }
}