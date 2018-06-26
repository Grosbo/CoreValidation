using CoreValidation.Errors;

namespace CoreValidation.Options
{
    public sealed class ValidationOptions : IValidationOptions
    {
        public string TranslationName { get; set; }

        public ValidationStrategy ValidationStrategy { get; set; }

        public NullRootStrategy NullRootStrategy { get; set; }

        public string CollectionForceKey { get; set; }

        public Error RequiredError { get; set; }

        public int MaxDepth { get; set; }
    }
}