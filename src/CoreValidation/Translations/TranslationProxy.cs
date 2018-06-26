namespace CoreValidation.Translations
{
    public sealed class TranslationProxy : ITranslationProxy
    {
        public TranslationProxy(Translator defaultTranslator, ITranslatorsRepository translatorsRepository)
        {
            DefaultTranslator = defaultTranslator;
            TranslatorsRepository = translatorsRepository;
        }

        public Translator DefaultTranslator { get; }

        public ITranslatorsRepository TranslatorsRepository { get; }
    }
}