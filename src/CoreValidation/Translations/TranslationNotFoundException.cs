using System;

namespace CoreValidation.Translations
{
    public sealed class TranslationNotFoundException : ArgumentException
    {
        public TranslationNotFoundException(string name)
            : base($"Translation '{name}' not found.")
        {
            Name = name;
        }

        public string Name { get; }
    }
}