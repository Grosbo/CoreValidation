namespace CoreValidation.Translations.Template
{
    public interface ITimesMessages
    {
        string EqualTo { get; }
        string NotEqualTo { get; }
        string After { get; }
        string AfterOrEqualTo { get; }
        string Before { get; }
        string BeforeOrEqualTo { get; }
        string Between { get; }
        string BetweenOrEqualTo { get; }
        string AfterNow { get; }
        string BeforeNow { get; }
        string FromNow { get; }
    }
}