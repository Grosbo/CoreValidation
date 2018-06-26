using CoreValidation.Errors;

namespace CoreValidation.Options
{
    public interface IRulesOptions
    {
        string CollectionForceKey { get; }

        Error RequiredError { get; }

        int MaxDepth { get; }
    }
}