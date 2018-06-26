using CoreValidation.Validators;
using Xunit;

namespace CoreValidation.UnitTests
{
    internal static class ValidationContextTestsHelper
    {
        public static void AssertValidator<T>(IValidationContext validationContext, Validator<T> validator)
            where T : class
        {
            Assert.Contains(typeof(T), validationContext.Types);

            var validatorFromRepo = ((ValidationContext)validationContext).ValidatorsRepository.Get<T>();

            Assert.Equal(validator, validatorFromRepo);
        }
    }
}