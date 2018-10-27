namespace CoreValidation.Results
{
    public sealed class InvalidModelResultException<T> : InvalidModelException<T>
        where T : class
    {
        public InvalidModelResultException(IValidationResult<T> validationResult)
            : base($"Invalid model of type {typeof(T).FullName}. Please check `ValidationResult` for details")
        {
            ValidationResult = validationResult;
        }

        public IValidationResult<T> ValidationResult { get; }

        public T Model => ValidationResult.Model;
    }
}