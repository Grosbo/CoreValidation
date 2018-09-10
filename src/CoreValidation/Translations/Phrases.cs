using CoreValidation.Translations.Template;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static partial class Phrases
    {
        public static ITranslationTemplate Keys { get; } = TranslationTemplateService.Keys;
    }
}