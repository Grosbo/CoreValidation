using System;
using CoreValidation.Options;
using CoreValidation.Results;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidationOptionsInContextExtensions
    {
        /// <summary>
        ///     Sets the behavior for the null reference passed to be validated.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="nullRootStrategy">Behavior for the null reference passed to be validated.</param>
        public static IValidationContextOptions SetNullRootStrategy(this IValidationContextOptions options, NullRootStrategy nullRootStrategy)
        {
            options.ValidationOptions.SetNullRootStrategy(nullRootStrategy);

            return options;
        }

        /// <summary>
        ///     Default translation name. Used to create <see cref="ITranslationProxy.DefaultTranslator" /> in the
        ///     <see cref="IValidationResult{T}" />.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="translationName">Translation name.</param>
        public static IValidationContextOptions SetTranslationName(this IValidationContextOptions options, string translationName)
        {
            options.ValidationOptions.SetTranslationName(translationName);

            return options;
        }

        /// <summary>
        ///     Disables the default translation. <see cref="ITranslationProxy.DefaultTranslator" /> in the
        ///     <see cref="IValidationResult{T}" /> will be using original phrases.
        /// </summary>
        /// <param name="options"></param>
        public static IValidationContextOptions SetTranslationDisabled(this IValidationContextOptions options)
        {
            options.ValidationOptions.SetTranslationDisabled();

            return options;
        }

        /// <summary>
        ///     Sets the default strategy of the validation process.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="validationStrategy">Strategy of the validation process.</param>
        public static IValidationContextOptions SetValidationStrategy(this IValidationContextOptions options, ValidationStrategy validationStrategy)
        {
            options.ValidationOptions.SetValidationStrategy(validationStrategy);

            return options;
        }

        /// <summary>
        ///     Sets the default error added to the null member if the it's required.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="errorMessage">Error message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="errorMessage" /> is null.</exception>
        public static IValidationContextOptions SetRequiredError(this IValidationContextOptions options, string errorMessage)
        {
            options.ValidationOptions.SetRequiredError(errorMessage);

            return options;
        }

        /// <summary>
        ///     Sets the default error added to the member if it's invalid but no error message is assigned.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="errorMessage">Error message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="errorMessage" /> is null.</exception>
        public static IValidationContextOptions SetDefaultError(this IValidationContextOptions options, string errorMessage)
        {
            options.ValidationOptions.SetDefaultError(errorMessage);

            return options;
        }

        /// <summary>
        ///     Sets the maximum allowed level of depth within the validated model. The default value is 10.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="maxDepth">Max depth.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxDepth" /> is less than zero.</exception>
        public static IValidationContextOptions SetMaxDepth(this IValidationContextOptions options, int maxDepth)
        {
            options.ValidationOptions.SetMaxDepth(maxDepth);

            return options;
        }

        /// <summary>
        ///     Sets the key (a member name) for the collection's members errors if validating using
        ///     <see cref="ValidationStrategy.Force" /> strategy.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="collectionForceKey">
        ///     Key (a member name) for the collection's members errors if validating using
        ///     <see cref="ValidationStrategy.Force" /> strategy.
        /// </param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="collectionForceKey" /> is null.</exception>
        public static IValidationContextOptions SetCollectionForceKey(this IValidationContextOptions options, string collectionForceKey)
        {
            options.ValidationOptions.SetCollectionForceKey(collectionForceKey);

            return options;
        }
    }
}