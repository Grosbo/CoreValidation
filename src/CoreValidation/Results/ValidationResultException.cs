namespace CoreValidation.Results
{
    /// <summary>
    ///     The exception that is thrown when the model is invalid.
    /// </summary>
    /// <typeparam name="T">Type of the validated model.</typeparam>
    public sealed class ValidationResultException<T> : InvalidModelException<T>
        where T : class
    {
        public ValidationResultException(IValidationResult<T> validationResult)
            : base($"Invalid model of type {typeof(T).FullName}. Please check `ValidationResult` for details")
        {
            ValidationResult = validationResult;
        }

        /// <summary>
        ///     Validation result.
        /// </summary>
        public IValidationResult<T> ValidationResult { get; }

        /// <summary>
        ///     The validated model.
        /// </summary>
        public T Model => ValidationResult.Model;
    }
}