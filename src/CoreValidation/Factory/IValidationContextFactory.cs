using System;
using CoreValidation.Exceptions;
using CoreValidation.Options;

namespace CoreValidation.Factory
{
    /// <summary>
    /// Factory for the <see cref="IValidationContext"/> instances.
    /// </summary>
    public interface IValidationContextFactory
    {
        /// <summary>
        /// Creates new validation context.
        /// </summary>
        /// <param name="options">Fluent api for validation context options.</param>
        /// <exception cref="InvalidProcessedReferenceException">Thrown when <paramref name="options"/> returns different reference than the one on the input.</exception>
        /// <returns>Validation context.</returns>
        IValidationContext Create(Func<IValidationContextOptions, IValidationContextOptions> options);
    }
}