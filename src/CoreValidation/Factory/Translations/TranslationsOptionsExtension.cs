using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Factory.Translations;
using CoreValidation.Options;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class TranslationsOptionsExtension
    {
        public static IValidationContextOptions AddTranslation(this IValidationContextOptions options, string name, IDictionary<string, string> dictionary, bool asDefault = false)
        {
            var translation = new Translation(name, dictionary);

            var processedOptions = OptionsUnwrapper.UnwrapTranslations(options, translations => { translations.Add(translation); });

            if (asDefault)
            {
                processedOptions.SetTranslationName(name);
            }

            return processedOptions;
        }

        public static IValidationContextOptions AddTranslations(this IValidationContextOptions options, TranslationsPackage translationsPackage)
        {
            if (translationsPackage == null)
            {
                throw new ArgumentNullException(nameof(translationsPackage));
            }

            return OptionsUnwrapper.UnwrapTranslations(options, translations =>
            {
                var translationsFromPackage = translationsPackage.Select(pair => new Translation(pair.Key, pair.Value)).ToArray();

                foreach (var translation in translationsFromPackage)
                {
                    translations.Add(translation);
                }
            });
        }
    }
}