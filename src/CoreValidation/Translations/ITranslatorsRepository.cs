using System.Collections.Generic;

namespace CoreValidation.Translations
{
    public interface ITranslatorsRepository
    {
        IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Translations { get; }

        Translator Get(string translationName);

        Translator GetOriginal();
    }
}