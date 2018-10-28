using System;
using CoreValidation.Errors;
using CoreValidation.Options;
using CoreValidation.Translations;

namespace CoreValidation.Results
{
    /// <summary>
    /// Validation result - contains the error collection. Entry point to create reports, merging and processing errors.
    /// </summary>
    /// <typeparam name="T">Type of the validated model.</typeparam>
    public interface IValidationResult<out T>
        where T : class
    {
        /// <summary>
        /// The validated model that is result is based on.
        /// </summary>
        T Model { get; }

        /// <summary>
        /// Errors collection for the <see cref="Model"/>.
        /// </summary>
        IErrorsCollection ErrorsCollection { get; }

        /// <summary>
        /// Returns true if the <see cref="Model"/> is valid (has no errors added during the validation).
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// <see cref="ValidationContext.Id">Id</see> of the source validation context that produced this result.
        /// </summary>
        Guid ValidationContextId { get; }

        /// <summary>
        /// Validation date, set when all the work is done and this result is created.
        /// </summary>
        DateTime ValidationDate { get; }

        /// <summary>
        /// Gets a value indicating whether this validation result was created by merging multiple results into one.
        /// </summary>
        bool IsMergeResult { get; }

        /// <summary>
        /// Execution options used to perform this validation set in the source validation context.
        /// </summary>
        IExecutionOptions ExecutionOptions { get; }

        /// <summary>
        /// Gets the object that translates the errors. Proxy to all the translations registered in the source validation context.
        /// </summary>
        ITranslationProxy TranslationProxy { get; }

        /// <summary>
        /// Creates the new instance of validation result with all errors from the input results.
        /// </summary>
        /// <param name="errorsCollections">Errors collections to merge.</param>
        /// <returns>New instance of validation result with all errors from the input results.</returns>
        IValidationResult<T> Merge(params IErrorsCollection[] errorsCollections);
    }
}