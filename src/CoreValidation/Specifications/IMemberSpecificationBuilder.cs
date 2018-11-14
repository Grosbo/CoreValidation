using System;
using System.Collections.Generic;
using CoreValidation.Errors.Args;

namespace CoreValidation.Specifications
{
    /// <summary>
    ///     Builder with all of the Fluent API extensions for specifying a valid state for <typeparamref name="TModel" />
    ///     models
    /// </summary>
    /// <typeparam name="TModel">Type of the model that the member belongs to.</typeparam>
    /// <typeparam name="TMember">Type of the member to be specified.</typeparam>
    public interface IMemberSpecificationBuilder<out TModel, out TMember>
        where TModel : class
    {
        /// <summary>
        ///     Sets the validation logic for the member as a relative to other members of the same model.
        /// </summary>
        /// <param name="isValid">
        ///     Predicate determining if the current member is valid as a relative. The parent model is provided
        ///     in the predicate.
        /// </param>
        IMemberSpecificationBuilder<TModel, TMember> AsRelative(Predicate<TModel> isValid);

        /// <summary>
        ///     Sets the custom validation logic for the member in its scope.
        ///     If not overriden using 'WithMessage', the default error message ('DefaultError' in settings) will be used.
        /// </summary>
        /// <param name="isValid">Predicate determining if the current member is valid.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="isValid" /> is null</exception>
        IMemberSpecificationBuilder<TModel, TMember> Valid(Predicate<TMember> isValid);

        /// <summary>
        /// Sets the custom validation logic for the member in its scope.
        /// </summary>
        /// <param name="isValid">Predicate determining if the current member is valid.</param>
        /// <param name="message">Error message.</param>
        /// <param name="args">The arguments available in the error message.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="isValid" /> is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message" /> is null</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="args" /> contains null</exception>
        IMemberSpecificationBuilder<TModel, TMember> Valid(Predicate<TMember> isValid, string message, IReadOnlyCollection<IMessageArg> args = null);


        /// <summary>
        /// Sets the error message that replaces all errors from the current scope.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message" /> is null</exception>
        IMemberSpecificationBuilder<TModel, TMember> SetSingleError(string message);

        /// <summary>
        /// Sets (and overrides) the error messages returned from the preceding command.
        /// If placed after 'Valid' (or any predefined rule using it), replaces the original error message.
        /// If placed after command that can return multiple error messages (like 'AsModel', 'AsCollection', etc.), replaces
        /// them all as a single error message (effectively works as putting SetSingleError within their scope).
        /// </summary>
        /// <param name="message">Error message</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="message" /> is null</exception>
        IMemberSpecificationBuilder<TModel, TMember> WithMessage(string message);
    }
}