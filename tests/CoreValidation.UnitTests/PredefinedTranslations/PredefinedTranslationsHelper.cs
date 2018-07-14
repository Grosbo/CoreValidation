using System.Collections.Generic;
using System.Linq;
using CoreValidation.Translations;
using CoreValidation.Translations.Template;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedTranslations
{
    public static class PredefinedTranslationsHelper
    {
        public static void AssertAddTranslations(IValidationContext validationContext, string translationName, ITranslationTemplate translation)
        {
            AssertTranslationAdded(validationContext, translationName);

            Assert.Null(validationContext.ValidationOptions.TranslationName);

            AssertPhrasesAdded(validationContext.Translations[translationName], translation);
        }

        public static void AssertSetAsDefault(IValidationContext validationContexts, string translationName)
        {
            Assert.NotEmpty(validationContexts.Translations);

            Assert.Equal(translationName, validationContexts.ValidationOptions.TranslationName);
        }

        public static void AssertInclude(IValidationContext validationContexts, string translationName, IReadOnlyDictionary<string, string> include)
        {
            var dictionary = validationContexts.Translations[translationName];

            foreach (var pair in include)
            {
                Assert.Contains(pair.Key, dictionary.Keys);
                Assert.Equal(pair.Value, dictionary[pair.Key]);
            }
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void AssertTranslationAdded(IValidationContext validationContext, string translationName)
        {
            Assert.Single(validationContext.Translations);

            var entry = validationContext.Translations.Single();

            Assert.Equal(translationName, entry.Key);
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void AssertPhrasesAdded(IReadOnlyDictionary<string, string> dictionary, ITranslationTemplate translation)
        {
            Assert.Equal(Phrases.English.Keys.Count, dictionary.Keys.Count());

            foreach (var key in Phrases.English.Keys)
            {
                Assert.Contains(key, dictionary.Keys);
            }

            var translationProperties = typeof(ITranslationTemplate).GetProperties();

            foreach (var translationProperty in translationProperties)
            {
                if (translationProperty.PropertyType == typeof(string))
                {
                    Assert.Contains(translationProperty.Name, dictionary.Keys);

                    Assert.Equal((string)translationProperty.GetValue(translation), dictionary[translationProperty.Name]);
                }
                else
                {
                    var group = translationProperty.GetValue(translation);

                    var groupProperties = translationProperty.PropertyType.GetProperties();

                    foreach (var groupProperty in groupProperties)
                    {
                        var key = $"{translationProperty.Name}.{groupProperty.Name}";

                        var value = (string)groupProperty.GetValue(group);

                        Assert.Contains(key, dictionary.Keys);

                        Assert.Equal(value, dictionary[key]);
                    }
                }
            }
        }
    }
}