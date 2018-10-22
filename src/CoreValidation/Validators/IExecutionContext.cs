using CoreValidation.Errors;
using CoreValidation.Options;

namespace CoreValidation.Validators
{
    internal interface IExecutionContext : IExecutionOptions
    {
        IValidatorsFactory ValidatorsFactory { get; }
        IErrorsCollection DefaultErrorAsCollection { get; }
    }
}