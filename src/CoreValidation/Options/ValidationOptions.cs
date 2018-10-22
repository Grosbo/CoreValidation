using CoreValidation.Errors;

namespace CoreValidation.Options
{
    public sealed class ValidationOptions : IValidationOptions
    {
        public string TranslationName { get; set; }

        public ValidationStrategy ValidationStrategy { get; set; }

        public NullRootStrategy NullRootStrategy { get; set; }

        public string CollectionForceKey { get; set; }

        public IError RequiredError { get; set; }

        public IError DefaultError { get; set; }

        public int MaxDepth { get; set; }

        public static ValidationOptions CreateDefault()
        {
            return new ValidationOptions
            {
                NullRootStrategy = NullRootStrategy.RequiredError,
                ValidationStrategy = ValidationStrategy.Complete,
                TranslationName = null,
                CollectionForceKey = "*",
                MaxDepth = 10,
                RequiredError = new Error(Phrases.English[Phrases.Keys.Required]),
                DefaultError = new Error(Phrases.English[Phrases.Keys.Invalid])
            };
        }
    }
}