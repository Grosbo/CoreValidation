using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Exceptions;

namespace CoreValidation.Translations
{
    public sealed class TranslatorsRepository : ITranslatorsRepository
    {
        public TranslatorsRepository(IReadOnlyCollection<Translation> translations)
        {
            if (translations == null)
            {
                throw new ArgumentNullException(nameof(translations));
            }

            EnsureNoDuplicates(translations);

            var translationsDictionary = translations
                .ToDictionary(item => item.Name,
                    item => new ReadOnlyDictionary<string, string>(item.Dictionary) as IReadOnlyDictionary<string, string>);

            Translations = new ReadOnlyDictionary<string, IReadOnlyDictionary<string, string>>(translationsDictionary);
        }

        public Translator Get(string translationName)
        {
            if (translationName == null)
            {
                throw new ArgumentNullException(nameof(translationName));
            }

            if (!Translations.TryGetValue(translationName, out var dictionary))
            {
                throw new TranslationNotFoundException(translationName);
            }

            return error => dictionary.TryGetValue(error.Message, out var translation)
                ? MessageFormatter.Format(translation, error.Arguments)
                : MessageFormatter.Format(error.Message, error.Arguments);
        }

        public Translator GetOriginal()
        {
            return error => Phrases.English.TryGetValue(error.Message, out var translation)
                ? MessageFormatter.Format(translation, error.Arguments)
                : MessageFormatter.Format(error.Message, error.Arguments);
        }

        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Translations { get; }

        private void EnsureNoDuplicates(IReadOnlyCollection<Translation> dictionaryItems)
        {
            var duplicateName = dictionaryItems.FirstOrDefault(item => dictionaryItems.Count(i => i.Name == item.Name) > 1);

            if (duplicateName != null)
            {
                throw new DuplicateTranslationException($"Duplicate dictionary name {duplicateName}");
            }
        }
    }
}