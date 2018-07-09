using System.Collections.Generic;
using CoreValidation.Options;
using CoreValidation.PredefinedTranslations;
using CoreValidation.Translations.Template;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class AddPolishTranslationExtension
    {
        private static readonly TranslationTemplateService _translationTemplateService = new TranslationTemplateService();

        private static readonly IDictionary<string, string> _polishTranslation = _translationTemplateService.CreateDictionary(new PolishTranslation());

        private static readonly string PolishTranslationName = "Polish";

        public static IValidationContextOptions AddPolishTranslation(this IValidationContextOptions @this, bool asDefault = false)
        {
            @this.AddTranslation(PolishTranslationName, _polishTranslation, asDefault);

            return @this;
        }
    }
}