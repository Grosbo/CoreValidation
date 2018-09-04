namespace CoreValidation.Translations.Template
{
    public interface ICollectionsMessages
    {
        string EmptyCollection { get; }
        string NotEmptyCollection { get; }
        string ExactCollectionSize { get; }
        string MaxCollectionSize { get; }
        string MinCollectionSize { get; }
        string CollectionSizeBetween { get; }
    }
}