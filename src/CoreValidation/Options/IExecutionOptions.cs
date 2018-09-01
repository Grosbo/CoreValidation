using CoreValidation.Errors;

namespace CoreValidation.Options
{
    public interface IExecutionOptions
    {
        string CollectionForceKey { get; }

        Error RequiredError { get; }

        Error DefaultError { get; }

        int MaxDepth { get; }
    }
}