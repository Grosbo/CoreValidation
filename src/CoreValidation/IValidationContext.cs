using System;
using System.Collections.Generic;
using CoreValidation.Options;
using CoreValidation.Results;

namespace CoreValidation
{
    /// <summary>
    /// The starting point for the validation process.
    /// Contains specifications (used to validate the objects) and translations (passed to the produced results).
    /// </summary>
    public interface IValidationContext
    {
        /// <summary>
        /// Unique identifier of this context.
        /// </summary>
        /// <value>
        /// Unique identifier of this context.
        /// </value>
        Guid Id { get; }

        /// <summary>
        /// Types ready to be validated in this context.
        /// </summary>
        /// <value>
        /// Types ready to be validated in this context.
        /// </value>
        IReadOnlyCollection<Type> Types { get; }

        /// <summary>
        /// Translations available in this context.
        /// </summary>
        /// <value>
        /// Translations available in this context.
        /// </value>
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Translations { get; }


        /// <summary>
        /// Default <see cref="IValidationOptions"/> used in this context
        /// </summary>
        /// <value>
        /// Default <see cref="IValidationOptions"/> used in this context..
        /// </value>
        IValidationOptions ValidationOptions { get; }


        /// <summary>
        /// Validates the specified model
        /// </summary>
        /// <typeparam name="T">Type of the validated model.</typeparam>
        /// <param name="model">The model to validate.</param>
        /// <param name="modifyOptions">Options modification for the current validation process.</param>
        /// <returns>The result (<see cref="IValidationResult{T}"/>) of the validation process.</returns>
        IValidationResult<T> Validate<T>(T model, Func<IValidationOptions, IValidationOptions> modifyOptions = null)
            where T : class;


        /// <summary>
        /// Clones the current instance of <see cref="IValidationContext"/>.
        /// </summary>
        /// <param name="modifyOptions">Options modification for the clone. If null, the clone will have the same options as the current instance.</param>
        /// <returns>A clone of the current instance of <see cref="IValidationContext"/>.</returns>
        IValidationContext Clone(Func<IValidationContextOptions, IValidationContextOptions> modifyOptions = null);
    }
}