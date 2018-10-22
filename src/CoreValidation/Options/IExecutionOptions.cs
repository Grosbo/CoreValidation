using CoreValidation.Errors;

namespace CoreValidation.Options
{
    public interface IExecutionOptions
    {
        string CollectionForceKey { get; }

        IError RequiredError { get; }

        IError DefaultError { get; }

        int MaxDepth { get; }
    }
}