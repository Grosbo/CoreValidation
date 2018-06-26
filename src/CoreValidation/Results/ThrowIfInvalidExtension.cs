using CoreValidation.Results;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ThrowIfInvalidExtension
    {
        public static void ThrowIfInvalid<T>(this IValidationResult<T> validationResult)
            where T : class
        {
            if (!validationResult.IsValid())
            {
                throw new InvalidModelException<T>(validationResult);
            }
        }
    }
}