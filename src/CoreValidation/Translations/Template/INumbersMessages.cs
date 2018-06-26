namespace CoreValidation.Translations.Template
{
    public interface INumbersMessages
    {
        string EqualTo { get; }
        string NotEqualTo { get; }
        string GreaterThan { get; }
        string GreaterOrEqualTo { get; }
        string LessThan { get; }
        string LessOrEqualTo { get; }
        string Between { get; }
        string BetweenOrEqualTo { get; }
        string CloseTo { get; }
        string NotCloseTo { get; }
    }
}