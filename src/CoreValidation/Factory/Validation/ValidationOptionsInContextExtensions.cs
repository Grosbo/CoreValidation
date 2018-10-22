﻿using CoreValidation.Options;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidationOptionsInContextExtensions
    {
        public static IValidationContextOptions SetNullRootStrategy(this IValidationContextOptions options, NullRootStrategy nullRootStrategy)
        {
            options.ValidationOptions.SetNullRootStrategy(nullRootStrategy);

            return options;
        }

        public static IValidationContextOptions SetTranslationName(this IValidationContextOptions options, string translationName)
        {
            options.ValidationOptions.SetTranslationName(translationName);

            return options;
        }

        public static IValidationContextOptions SetTranslationDisabled(this IValidationContextOptions options)
        {
            options.ValidationOptions.SetTranslationDisabled();

            return options;
        }

        public static IValidationContextOptions SetValidationStrategy(this IValidationContextOptions options, ValidationStrategy validationStrategy)
        {
            options.ValidationOptions.SetValidationStrategy(validationStrategy);

            return options;
        }

        public static IValidationContextOptions SetRequiredError(this IValidationContextOptions options, string errorMessage)
        {
            options.ValidationOptions.SetRequiredError(errorMessage);

            return options;
        }

        public static IValidationContextOptions SetDefaultError(this IValidationContextOptions options, string errorMessage)
        {
            options.ValidationOptions.SetDefaultError(errorMessage);

            return options;
        }

        public static IValidationContextOptions SetMaxDepth(this IValidationContextOptions options, int maxDepth)
        {
            options.ValidationOptions.SetMaxDepth(maxDepth);

            return options;
        }

        public static IValidationContextOptions SetCollectionForceKey(this IValidationContextOptions options, string collectionForceKey)
        {
            options.ValidationOptions.SetCollectionForceKey(collectionForceKey);

            return options;
        }
    }
}