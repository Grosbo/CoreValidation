using CoreValidation.Translations.Template;

namespace CoreValidation.PredefinedTranslations
{
    internal class EnglishTranslation : ITranslationTemplate
    {
        public ICollectionsMessages Collections { get; } = new CollectionsMessages
        {
            Empty = "Collection should be empty",
            NotEmpty = "Collection cannot be empty",
            ExactSize = "Collection should have exactly {size} elements",
            MaxSize = "Collection should have maximum {max} elements",
            MinSize = "Collection should have minimum {min} elements",
            SizeBetween = "Collection size should be between {min} and {max} elements"
        };

        public IBoolMessages Bool { get; } = new BoolMessages
        {
            True = "Logical value should be true",
            False = "Logical value should be false"
        };

        public INumbersMessages Numbers { get; } = new NumbersMessages
        {
            EqualTo = "Number should be equal to {value}",
            NotEqualTo = "Number cannot be equal to {value}",
            GreaterThan = "Number should be greater than {min}",
            GreaterOrEqualTo = "Number should be greater than (or equal to) {min}",
            LessThan = "Number should be less than {max}",
            LessOrEqualTo = "Number should be less than (or equal to) {max}",
            Between = "Number should be between {min} and {max}",
            BetweenOrEqualTo = "Number should be between {min} and {max} (inclusive)",
            CloseTo = "Number should be equal to {value}",
            NotCloseTo = "Number cannot be equal to {value}"
        };

        public ITimeSpanMessages TimeSpan { get; } = new TimeSpanMessages
        {
            EqualTo = "Time span should be equal to {value}",
            NotEqualTo = "Time span cannot be equal to {value}",
            GreaterThan = "Time span should be greater than {min}",
            GreaterOrEqualTo = "Time span should be greater than (or equal to) {min}",
            LessThan = "Time span should be less than {max}",
            LessOrEqualTo = "Time span should be less than (or equal to) {max}",
            Between = "Time span should be between {min} and {max}",
            BetweenOrEqualTo = "Time span should be between {min} and {max} (inclusive)"
        };

        public ICharMessages Char { get; } = new CharMessages
        {
            EqualIgnoreCase = "Character should be equal to {value} (case insensitive)",
            NotEqualIgnoreCase = "Character cannot be equal to {value} (case insensitive)"
        };

        public ITextsMessages Texts { get; } = new TextsMessages
        {
            Email = "Text value should be a valid email",
            EqualTo = "Text value should be equal to '{value}'",
            NotEqualTo = "Text value cannot be equal to '{value}'",
            Contains = "Text value should contain '{value}'",
            NotContains = "Text value cannot contain '{value}'",
            NotEmpty = "Text value cannot be empty",
            NotWhiteSpace = "Text value cannot be whitespace",
            SingleLine = "Text value must be single line",
            ExactLength = "Text value should have exactly {length} characters",
            MaxLength = "Text value should have maximum {max} characters",
            MinLength = "Text value should have minimum {min} characters",
            LengthBetween = "Text value should have between {min} and {max} characters",
            IsGuid = "Text value should be a valid GUID"
        };


        public ITimesMessages Times { get; } = new TimesMessages
        {
            EqualTo = "Date should be equal to {value}",
            NotEqualTo = "Date cannot be equal to {value}",
            After = "Date should be after {min}",
            AfterOrEqualTo = "Date should be after (or equal to) {min}",
            Before = "Date should be before {max}",
            BeforeOrEqualTo = "Date should be before (or equal to) {max}",
            Between = "Date should be between {min} and {max}",
            BetweenOrEqualTo = "Date should be between (or equal to) {min} and {max}",
            AfterNow = "Date should be after now ({now})",
            BeforeNow = "Date should be before now ({now})",
            FromNow = "Date should be during the time frow now: {timeSpan} (now = {now})"
        };


        public string Required { get; } = "Required";
    }
}