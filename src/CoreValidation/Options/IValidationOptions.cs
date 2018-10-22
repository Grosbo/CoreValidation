namespace CoreValidation.Options
{
    public interface IValidationOptions : IExecutionOptions
    {
        string TranslationName { get; }

        NullRootStrategy NullRootStrategy { get; }

        ValidationStrategy ValidationStrategy { get; }
    }
}