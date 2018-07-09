using CoreValidation.Translations.Template;

namespace CoreValidation.PredefinedTranslations
{
    internal class PolishTranslation : ITranslationTemplate
    {
        public ICollectionsMessages Collections { get; } = new CollectionsMessages
        {
            Empty = "Kolekcja powinna być pusta",
            NotEmpty = "Kolekcja nie może być pusta",
            ExactSize = "Kolekcja powinna posiadać dokładnie {size} elementów",
            MaxSize = "Kolekcja powinna posiadać maksymalnie {max} elementów",
            MinSize = "Kolekcja powinna posiadać minimalnie {min} elementów",
            SizeBetween = "Kolekcja powinna posiadać pomiędzy {min} a {max} elementów"
        };

        public IBoolMessages Bool { get; } = new BoolMessages
        {
            True = "Wartość powinna być prawdą",
            False = "Wartość powinna być fałszem"
        };

        public INumbersMessages Numbers { get; } = new NumbersMessages
        {
            EqualTo = "Liczba powinna być równa {value}",
            NotEqualTo = "Liczba nie może być równa {value}",
            GreaterThan = "Liczba powinna być większa od {min}",
            GreaterOrEqualTo = "Liczba powinna być większa od (lub równa) {min}",
            LessThan = "Liczba powinna być mniejsza od {max}",
            LessOrEqualTo = "Liczba powinna być większa od (lub równa) {max}",
            Between = "Liczba powinna być pomiędzy {min} and {max}",
            BetweenOrEqualTo = "Liczba powinna być pomiędzy {min} a {max} (włącznie)",
            CloseTo = "Liczba powinna być równa {value}",
            NotCloseTo = "Liczba nie może być równa {value}"
        };

        public ITimeSpanMessages TimeSpan { get; } = new TimeSpanMessages
        {
            EqualTo = "Przedział czasowy powinien być równy {value}",
            NotEqualTo = "Przedział czasowy nie może być równy {value}",
            GreaterThan = "Przedział czasowy powinien być większy od {min}",
            GreaterOrEqualTo = "Przedział czasowy powinien być większy od (lub równy) {min}",
            LessThan = "Przedział czasowy powinien być mniejszy od {max}",
            LessOrEqualTo = "Przedział czasowy powinien być mniejszy od (lub równy) {max}",
            Between = "Przedział czasowy powinien być pomiędzy {min} and {max}",
            BetweenOrEqualTo = "Przedział czasowy powinien być pomiędzy {min} a {max} (włącznie)"
        };

        public ICharMessages Char { get; } = new CharMessages
        {
            EqualIgnoreCase = "Znak powinien być równy {value} (wielkość znaku nieistotna)",
            NotEqualIgnoreCase = "Znak nie może być równy {value} (wielkość znaku nieistotna)"
        };

        public ITextsMessages Texts { get; } = new TextsMessages
        {
            Email = "Wartość tekstowa powinna zawierać prawidłowy adres email",
            EqualTo = "Wartość tekstowa powinna być równa '{value}'",
            NotEqualTo = "Wartość tekstowa nie może być równa '{value}'",
            Contains = "Wartość tekstowa powinna zawierać '{value}'",
            NotContains = "Wartość tekstowa nie może zawierać '{value}'",
            NotEmpty = "Wartość tekstowa nie może być pusta",
            NotWhiteSpace = "Wartość tekstowa nie może składać się tylko z białych znaków",
            SingleLine = "Wartość tekstowa musi zawierać się w jednej linii",
            ExactLength = "Wartość tekstowa musi mieć dokładnie {length} znaków",
            MaxLength = "Wartość tekstowa musi mieć maksimum {max} znaków",
            MinLength = "Wartość tekstowa musi mieć minimum {min} znaków",
            LengthBetween = "Wartość tekstowa musi mieć pomiędzy {min} a {max} znaków",
            IsGuid = "Wartość tekstowa musi być prawidłową wartością GUID"
        };


        public ITimesMessages Times { get; } = new TimesMessages
        {
            EqualTo = "Data powinna być równa {value}",
            NotEqualTo = "Data nie może być równa {value}",
            After = "Data powinna być po {min}",
            AfterOrEqualTo = "Data powinna być po (lub równa) {min}",
            Before = "Data powinna być przed {max}",
            BeforeOrEqualTo = "Data powinna być przed (lub równa) {max}",
            Between = "Data powinna być pomiędzy {min} a {max}",
            BetweenOrEqualTo = "Data powinna być pomiędzy {min} a {max} (włącznie)",
            AfterNow = "Data powinna być przyszłości (aktualnie = {now})",
            BeforeNow = "Data powinna być w przeszłości (aktualnie = {now})",
            FromNow = "Data powinna być w przedziale od teraz: {timeSpan} (aktualnie = {now})"
        };

        public string Required { get; } = "Wartość wymagana";
    }
}