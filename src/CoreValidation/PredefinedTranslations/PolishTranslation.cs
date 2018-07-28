using CoreValidation.Translations.Template;

namespace CoreValidation.PredefinedTranslations
{
    internal class PolishTranslation : ITranslationTemplate
    {
        public ICollectionsMessages Collections { get; } = new CollectionsMessages
        {
            Empty = "Kolekcja powinna być pusta",
            NotEmpty = "Kolekcja nie może być pusta",
            ExactSize = "Kolekcja powinna posiadać dokładnie {size|culture=pl-PL} elementów",
            MaxSize = "Kolekcja powinna posiadać maksymalnie {max|culture=pl-PL} elementów",
            MinSize = "Kolekcja powinna posiadać minimalnie {min|culture=pl-PL} elementów",
            SizeBetween = "Kolekcja powinna posiadać pomiędzy {min|culture=pl-PL} a {max} elementów"
        };

        public IBoolMessages Bool { get; } = new BoolMessages
        {
            True = "Wartość powinna być prawdą",
            False = "Wartość powinna być fałszem"
        };

        public INumbersMessages Numbers { get; } = new NumbersMessages
        {
            EqualTo = "Liczba powinna być równa {value|culture=pl-PL}",
            NotEqualTo = "Liczba nie może być równa {value|culture=pl-PL}",
            GreaterThan = "Liczba powinna być większa od {min|culture=pl-PL}",
            GreaterOrEqualTo = "Liczba powinna być większa od (lub równa) {min|culture=pl-PL}",
            LessThan = "Liczba powinna być mniejsza od {max|culture=pl-PL}",
            LessOrEqualTo = "Liczba powinna być większa od (lub równa) {max|culture=pl-PL}",
            Between = "Liczba powinna być pomiędzy {min|culture=pl-PL} and {max|culture=pl-PL}",
            BetweenOrEqualTo = "Liczba powinna być pomiędzy {min|culture=pl-PL} a {max|culture=pl-PL} (włącznie)",
            CloseTo = "Liczba powinna być równa {value|culture=pl-PL}",
            NotCloseTo = "Liczba nie może być równa {value|culture=pl-PL}"
        };

        public ITimeSpanMessages TimeSpan { get; } = new TimeSpanMessages
        {
            EqualTo = "Przedział czasowy powinien być równy {value|culture=pl-PL}",
            NotEqualTo = "Przedział czasowy nie może być równy {value|culture=pl-PL}",
            GreaterThan = "Przedział czasowy powinien być większy od {min|culture=pl-PL}",
            GreaterOrEqualTo = "Przedział czasowy powinien być większy od (lub równy) {min|culture=pl-PL}",
            LessThan = "Przedział czasowy powinien być mniejszy od {max|culture=pl-PL}",
            LessOrEqualTo = "Przedział czasowy powinien być mniejszy od (lub równy) {max|culture=pl-PL}",
            Between = "Przedział czasowy powinien być pomiędzy {min|culture=pl-PL} and {max|culture=pl-PL}",
            BetweenOrEqualTo = "Przedział czasowy powinien być pomiędzy {min|culture=pl-PL} a {max|culture=pl-PL} (włącznie)"
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
            ExactLength = "Wartość tekstowa musi mieć dokładnie {length|culture=pl-PL} znaków",
            MaxLength = "Wartość tekstowa musi mieć maksimum {max|culture=pl-PL} znaków",
            MinLength = "Wartość tekstowa musi mieć minimum {min|culture=pl-PL} znaków",
            LengthBetween = "Wartość tekstowa musi mieć pomiędzy {min|culture=pl-PL} a {max|culture=pl-PL} znaków",
            IsGuid = "Wartość tekstowa musi być prawidłową wartością GUID"
        };


        public ITimesMessages Times { get; } = new TimesMessages
        {
            EqualTo = "Data powinna być równa {value|culture=pl-PL}",
            NotEqualTo = "Data nie może być równa {value|culture=pl-PL}",
            After = "Data powinna być po {min|culture=pl-PL}",
            AfterOrEqualTo = "Data powinna być po (lub równa) {min|culture=pl-PL}",
            Before = "Data powinna być przed {max|culture=pl-PL}",
            BeforeOrEqualTo = "Data powinna być przed (lub równa) {max|culture=pl-PL}",
            Between = "Data powinna być pomiędzy {min|culture=pl-PL} a {max|culture=pl-PL}",
            BetweenOrEqualTo = "Data powinna być pomiędzy {min|culture=pl-PL} a {max|culture=pl-PL} (włącznie)",
            AfterNow = "Data powinna być przyszłości (aktualnie = {now|culture=pl-PL})",
            BeforeNow = "Data powinna być w przeszłości (aktualnie = {now|culture=pl-PL})",
            FromNow = "Data powinna być w przedziale od teraz: {timeSpan|culture=pl-PL} (aktualnie = {now|culture=pl-PL})"
        };

        public string Required { get; } = "Wartość wymagana";
        public string Invalid { get; } = "Nieprawidłowa wartość";
    }
}