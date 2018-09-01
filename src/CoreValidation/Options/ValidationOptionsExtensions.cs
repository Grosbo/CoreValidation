using System;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Options;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidationOptionsExtensions
    {
        public static IValidationOptions SetNullRootStrategy(this IValidationOptions options, NullRootStrategy nullRootStrategy)
        {
            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.NullRootStrategy = nullRootStrategy; });
        }

        public static IValidationOptions SetTranslationName(this IValidationOptions options, string translationName)
        {
            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.TranslationName = translationName; });
        }

        public static IValidationOptions SetTranslationDisabled(this IValidationOptions options)
        {
            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.TranslationName = null; });
        }

        public static IValidationOptions SetValidationStrategy(this IValidationOptions options, ValidationStrategy validationStrategy)
        {
            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.ValidationStrategy = validationStrategy; });
        }

        public static IValidationOptions SetRequiredError(this IValidationOptions options, string errorMessage, IMessageArg[] args = null)
        {
            var requiredError = new Error(errorMessage, args);

            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.RequiredError = requiredError; });
        }

        public static IValidationOptions SetDefaultError(this IValidationOptions options, string errorMessage, IMessageArg[] args = null)
        {
            var requiredError = new Error(errorMessage, args);

            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.DefaultError = requiredError; });
        }

        public static IValidationOptions SetMaxDepth(this IValidationOptions options, int maxDepth)
        {
            return OptionsUnwrapper.UnwrapValidationOptions(options, validationOptions => { validationOptions.MaxDepth = maxDepth; });
        }

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