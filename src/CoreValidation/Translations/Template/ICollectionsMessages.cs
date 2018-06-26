namespace CoreValidation.Translations.Template
{
    internal interface ICollectionsMessages
    {
        string Empty { get; }
        string NotEmpty { get; }
        string ExactSize { get; }
        string MaxSize { get; }
        string MinSize { get; }
        string SizeBetween { get; }
    }
}