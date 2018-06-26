namespace CoreValidation.Translations.Template
{
    internal class TextsMessages : ITextsMessages
    {
        public string Email { get; set; }
        public string EqualTo { get; set; }
        public string NotEqualTo { get; set; }
        public string Contains { get; set; }
        public string NotContains { get; set; }
        public string NotEmpty { get; set; }
        public string NotWhiteSpace { get; set; }
        public string SingleLine { get; set; }
        public string ExactLength { get; set; }
        public string MaxLength { get; set; }
        public string MinLength { get; set; }
        public string LengthBetween { get; set; }
        public string IsGuid { get; set; }
    }
}