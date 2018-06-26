namespace CoreValidation.Options
{
    public interface IValidationOptions : IRulesOptions
    {
        string TranslationName { get; }

        ValidationStrategy ValidationStrategy { get; }

        NullRootStrategy NullRootStrategy { get; }
    }
}