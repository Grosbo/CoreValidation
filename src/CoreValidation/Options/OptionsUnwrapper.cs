using System;
using System.Collections.Generic;
using CoreValidation.Translations;

namespace CoreValidation.Options
{
    internal static class OptionsUnwrapper
    {
        private static IValidationContextOptions Unwrap(IValidationContextOptions wrapped, Func<ValidationContextOptions, ValidationContextOptions> processUnwrapped)
        {
            var unwrapped = (ValidationContextOptions)wrapped;

            var processedUnwrapped = processUnwrapped(unwrapped);

            return processedUnwrapped;
        }

        public static IValidationOptions UnwrapValidationOptions(IValidationOptions wrapped, Action<ValidationOptions> processUnwrapped)
        {
            var unwrapped = (ValidationOptions)wrapped;

            processUnwrapped(unwrapped);

            return unwrapped;
        }

        public static IValidationContextOptions UnwrapTranslations(IValidationContextOptions wrapped, Action<List<Translation>> processUnwrapped)
        {
            return Unwrap(wrapped, options =>
            {
                processUnwrapped((List<Translation>)options.Translations);

                return options;
            });
        }

        public static IValidationContextOptions UnwrapSpecifications(IValidationContextOptions wrapped, Action<Dictionary<Type, object>> processUnwrapped)
        {
            return Unwrap(wrapped, options =>
            {
                processUnwrapped((Dictionary<Type, object>)options.Specifications);

                return options;
            });
        }
    }
}