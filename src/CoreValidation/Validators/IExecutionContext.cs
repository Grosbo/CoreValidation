using CoreValidation.Errors;
using CoreValidation.Options;

namespace CoreValidation.Validators
{
    internal interface IExecutionContext
    {
        IExecutionOptions ExecutionOptions { get; }
        IValidatorsFactory ValidatorsFactory { get; }
        ValidationStrategy ValidationStrategy { get; }
        IErrorsCollection DefaultErrorAsCollection { get; }
    }
}