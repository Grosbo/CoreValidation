using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Factory.Translations;
using CoreValidation.Options;
using CoreValidation.Results;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class TranslationsOptionsExtension
    {
        /// <summary>
        /// Adds translation.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="name">Name of the translation. Usually the language name.</param>
        /// <param name="dictionary">Dictionary with all translation entries. Keys are the original phrases. Values are the translations.</param>
        /// <param name="asDefault">If true, sets the translations as the default one. The default is used to create <see cref="ITranslationProxy.DefaultTranslator"/> in the <see cref="IValidationResult{T}"/>.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Add translations from a <see cref="TranslationsPackage"/>.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="translationsPackage">Translations package. Key is the name of the translation. Value is the dictionary in which key is the original phrase and the value is the translated one.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="translationsPackage"/> is null.</exception>
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