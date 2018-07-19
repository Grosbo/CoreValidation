using System;
using CoreValidation.Errors;
using CoreValidation.Options;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidationOptionsExtensions
    {
        private static readonly OptionsUnwrapper _optionsUnwrapper = new OptionsUnwrapper();

        public static IValidationOptions SetNullRootStrategy(this IValidationOptions options, NullRootStrategy nullRootStrategy)
        {
            return _optionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.NullRootStrategy = nullRootStrategy; });
        }

        public static IValidationOptions SetTranslationName(this IValidationOptions options, string translationName)
        {
            return _optionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.TranslationName = translationName; });
        }

        public static IValidationOptions SetTranslationDisabled(this IValidationOptions options)
        {
            return _optionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.TranslationName = null; });
        }

        public static IValidationOptions SetValidationStategy(this IValidationOptions options, ValidationStrategy validationStrategy)
        {
            return _optionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.ValidationStrategy = validationStrategy; });
        }

        public static IValidationOptions SetRequiredError(this IValidationOptions options, string errorMessage, IMessageArg[] args = null)
        {
            var requiredError = new Error(errorMessage, args);

            return _optionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.RequiredError = requiredError; });
        }

        public static IValidationOptions SetDefaultError(this IValidationOptions options, string errorMessage, IMessageArg[] args = null)
        {
            var requiredError = new Error(errorMessage, args);

            return _optionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.DefaultError = requiredError; });
        }

        public static IValidationOptions SetMaxDepth(this IValidationOptions options, int maxDepth)
        {
            return _optionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.MaxDepth = maxDepth; });
        }

        public static IValidationOptions SetCollectionForceKey(this IValidationOptions options, string collectionForceKey)
        {
            if (collectionForceKey == null)
            {
                throw new ArgumentNullException(nameof(collectionForceKey));
            }

            return _optionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.CollectionForceKey = collectionForceKey; });
        }
    }
}