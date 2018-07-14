namespace CoreValidation.Translations.Template
{
    public interface ICollectionsMessages
    {
        string Empty { get; }
        string NotEmpty { get; }
        string ExactSize { get; }
        string MaxSize { get; }
        string MinSize { get; }
        string SizeBetween { get; }
    }
}