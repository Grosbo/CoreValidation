using CoreValidation.Specifications;
using Xunit;

namespace CoreValidation.UnitTests
{
    internal static class ValidationContextTestsHelper
    {
        public static void AssertSpecification<T>(IValidationContext validationContext, Specification<T> specification)
            where T : class
        {
            Assert.Contains(typeof(T), validationContext.Types);

            var validatorFromRepo = ((ValidationContext)validationContext).SpecificationsRepository.Get<T>();

            Assert.Equal(specification, validatorFromRepo);
        }
    }
}