using System;
using CoreValidation.Errors;
using CoreValidation.Options;
using CoreValidation.Results;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidationOptionsExtensions
    {
        /// <summary>
        /// Sets the behavior for the null reference passed to be validated.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="nullRootStrategy">Behavior for the null reference passed to be validated.</param>
        public static IValidationOptions SetNullRootStrategy(this IValidationOptions options, NullRootStrategy nullRootStrategy)
        {
            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.NullRootStrategy = nullRootStrategy; });
        }

        /// <summary>
        /// Default translation name. Used to create <see cref="ITranslationProxy.DefaultTranslator"/> in the <see cref="IValidationResult{T}"/>.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="translationName">Translation name.</param>
        public static IValidationOptions SetTranslationName(this IValidationOptions options, string translationName)
        {
            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.TranslationName = translationName; });
        }

        /// <summary>
        /// Disables the default translation. <see cref="ITranslationProxy.DefaultTranslator"/> in the <see cref="IValidationResult{T}"/> will be using original phrases.
        /// </summary>
        /// <param name="options"></param>
        public static IValidationOptions SetTranslationDisabled(this IValidationOptions options)
        {
            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.TranslationName = null; });
        }

        /// <summary>
        /// Sets the default strategy of the validation process.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="validationStrategy">Strategy of the validation process.</param>
        public static IValidationOptions SetValidationStrategy(this IValidationOptions options, ValidationStrategy validationStrategy)
        {
            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.ValidationStrategy = validationStrategy; });
        }

        /// <summary>
        /// Sets the default error added to the null member if the it's required.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="errorMessage">Error message.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="errorMessage"/> is null.</exception>
        public static IValidationOptions SetRequiredError(this IValidationOptions options, string errorMessage)
        {
            var requiredError = new Error(errorMessage);

            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.RequiredError = requiredError; });
        }

        /// <summary>
        /// Sets the default error added to the member if it's invalid but no error message is assigned.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="errorMessage">Error message</param>
        /// <exception cref="ArgumentNullException">Thrown if <see cref="errorMessage"/> is null.</exception>
        public static IValidationOptions SetDefaultError(this IValidationOptions options, string errorMessage)
        {
            var requiredError = new Error(errorMessage);

            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.DefaultError = requiredError; });
        }

        /// <summary>
        /// Sets the maximum allowed level of depth within the validated model. The default value is 10.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="maxDepth">Max depth.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maxDepth"/> is less than zero.</exception>
        public static IValidationOptions SetMaxDepth(this IValidationOptions options, int maxDepth)
        {
            if (maxDepth < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxDepth), maxDepth, $"{nameof(maxDepth)} cannot be less than 0");
            }

            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.MaxDepth = maxDepth; });
        }

        /// <summary>
        /// Sets the key (a member name) for the collection's members errors if validating using <see cref="ValidationStrategy.Force"/> strategy.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="collectionForceKey">Key (a member name) for the collection's members errors if validating using <see cref="ValidationStrategy.Force"/> strategy.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="collectionForceKey"/> is null.</exception>
        public static IValidationOptions SetCollectionForceKey(this IValidationOptions options, string collectionForceKey)
        {
            if (collectionForceKey == null)
            {
                throw new ArgumentNullException(nameof(collectionForceKey));
            }

            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.CollectionForceKey = collectionForceKey; });
        }
    }
}