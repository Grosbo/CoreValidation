using System.Collections.Generic;
using CoreValidation.PredefinedTranslations;
using CoreValidation.Translations.Template;

namespace CoreValidation.Translations
{
    internal static class Phrases
    {
        private static readonly TranslationTemplateService _translationTemplateService = new TranslationTemplateService();

        public static ITranslationTemplate Keys { get; } = _translationTemplateService.CreateKeysTemplate();

        public static IReadOnlyDictionary<string, string> English { get; } = _translationTemplateService.CreateDictionary(new EnglishTranslation());
    }
}