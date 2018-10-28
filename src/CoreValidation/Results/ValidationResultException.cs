namespace CoreValidation.Results
{
    public sealed class ValidationResultException<T> : InvalidModelException<T>
        where T : class
    {
        public ValidationResultException(IValidationResult<T> validationResult)
            : base($"Invalid model of type {typeof(T).FullName}. Please check `ValidationResult` for details")
        {
            ValidationResult = validationResult;
        }

        public IValidationResult<T> ValidationResult { get; }

        public T Model => ValidationResult.Model;
    }
}