using CoreValidation.Errors;

namespace CoreValidation.Options
{
    public interface IExecutionOptions
    {
        /// <summary>
        /// Key (a member name) for the collection item errors if validating using <see cref="ValidationStrategy.Force"/> strategy.
        /// </summary>
        string CollectionForceKey { get; }

        /// <summary>
        /// Error added to the null member if the it's required.
        /// </summary>
        IError RequiredError { get; }

        /// <summary>
        /// Error added to the member if it's invalid but no error message is assigned.
        /// </summary>
        IError DefaultError { get; }

        /// <summary>
        /// Maximum allowed level of depth within the validated model.
        /// </summary>
        int MaxDepth { get; }
    }
}