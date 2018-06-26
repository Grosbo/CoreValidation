using CoreValidation.Exceptions;

namespace CoreValidation.Translations
{
    public sealed class TranslationNotFoundException : CoreValidationException
    {
        public TranslationNotFoundException(string name)
            : base($"Translation '{name}' not found.")
        {
            Name = name;
        }

        public string Name { get; }
    }
}