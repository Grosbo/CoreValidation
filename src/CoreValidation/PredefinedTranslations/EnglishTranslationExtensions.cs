using System;
using System.Collections.Generic;
using CoreValidation.Options;
using CoreValidation.PredefinedTranslations;
using CoreValidation.Translations.Template;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static partial class Phrases
    {
        public static IDictionary<string, string> English { get; } = TranslationTemplateService.CreateDictionary(new EnglishTranslation());
    }

    public static class IncludeInEnglishTranslationExtension
    {
        public static IValidationContextOptions IncludeInEnglishTranslation(this IValidationContextOptions @this, IDictionary<string, string> include)
        {
            if (include == null)
            {
                throw new ArgumentNullException(nameof(include));
            }

            @this.AddTranslation(nameof(Phrases.English), include);

            return @this;
        }
    }
}