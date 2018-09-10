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
        public static IDictionary<string, string> Polish { get; } = TranslationTemplateService.CreateDictionary(new PolishTranslation());
    }

    public static class AddPolishTranslationExtension
    {
        public static IValidationContextOptions AddPolishTranslation(this IValidationContextOptions @this, bool asDefault = false, IDictionary<string, string> include = null)
        {
            @this.AddTranslation(nameof(Phrases.Polish), Phrases.Polish, asDefault);

            if (include != null)
            {
                @this.AddTranslation(nameof(Phrases.Polish), include);
            }

            return @this;
        }

        public static IValidationContextOptions IncludeInPolishTranslation(this IValidationContextOptions @this, IDictionary<string, string> include)
        {
            if (include == null)
            {
                throw new ArgumentNullException(nameof(include));
            }

            @this.AddTranslation(nameof(Phrases.Polish), include);

            return @this;
        }
    }
}