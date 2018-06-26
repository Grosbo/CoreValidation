namespace CoreValidation.Translations.Template
{
    public interface ITextsMessages
    {
        string Email { get; }
        string EqualTo { get; }
        string NotEqualTo { get; }
        string Contains { get; }
        string NotContains { get; }
        string NotEmpty { get; }
        string NotWhiteSpace { get; }
        string SingleLine { get; }
        string ExactLength { get; }
        string MaxLength { get; }
        string MinLength { get; }
        string LengthBetween { get; }
        string IsGuid { get; }
    }
}