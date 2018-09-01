namespace CoreValidation.Options
{
    public interface IValidationOptions : IExecutionOptions
    {
        string TranslationName { get; }

        ValidationStrategy ValidationStrategy { get; }

        NullRootStrategy NullRootStrategy { get; }
    }
}