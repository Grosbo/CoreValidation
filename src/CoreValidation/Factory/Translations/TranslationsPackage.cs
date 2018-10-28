using System.Collections.Generic;

namespace CoreValidation.Factory.Translations
{
    /// <summary>
    /// Translations package could contain multiple translations.
    /// Key is the name of the translation.
    /// Value is the dictionary with all translation entries. Keys are the original phrases. Values are the translations.
    /// </summary>
    public class TranslationsPackage : Dictionary<string, IDictionary<string, string>>
    {
    }
}