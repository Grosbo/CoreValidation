using System;
using System.Collections.Generic;
using CoreValidation.Translations;

namespace CoreValidation.Options
{
    internal static class OptionsUnwrapper
    {
        private static IValidationContextOptions Unwrap(IValidationContextOptions wrapped, Func<ValidationContextOptions, ValidationContextOptions> processUnwrapped)
        {
            if (!(wrapped is ValidationContextOptions unwrapped))
            {
                throw new InvalidOperationException($"Invalid reference of {nameof(IValidationContextOptions)}");
            }

            var processedUnwrapped = processUnwrapped(unwrapped);

            return processedUnwrapped;
        }

        public static IValidationOptions UnwrapValidationOptions(IValidationOptions wrapped, Action<ValidationOptions> processUnwrapped)
        {
            if (wrapped == null)
            {
                throw new ArgumentNullException(nameof(wrapped));
            }

            if (!(wrapped is ValidationOptions unwrapped))
            {
                throw new InvalidOperationException($"Invalid reference of {nameof(IValidationOptions)}");
            }

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
            if (processUnwrapped == null)
            {
                throw new ArgumentNullException(nameof(processUnwrapped));
            }

            return Unwrap(wrapped, options =>
            {
                processUnwrapped((Dictionary<Type, object>)options.Specifications);

                return options;
            });
        }
    }
}