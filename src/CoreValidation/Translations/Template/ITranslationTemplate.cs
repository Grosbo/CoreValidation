namespace CoreValidation.Translations.Template
{
    public interface ITranslationTemplate
    {
        ICollectionsMessages Collections { get; }
        IBoolMessages Bool { get; }
        INumbersMessages Numbers { get; }
        ITimeSpanMessages TimeSpan { get; }
        ICharMessages Char { get; }
        ITextsMessages Texts { get; }
        ITimesMessages Times { get; }
        string Required { get; }
    }
}