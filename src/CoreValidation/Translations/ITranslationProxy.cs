namespace CoreValidation.Translations
{
    public interface ITranslationProxy
    {
        Translator DefaultTranslator { get; }
        ITranslatorsRepository TranslatorsRepository { get; }
    }
}