using CoreValidation.Specifications;
using Xunit;
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Commands.Member
{
    public class Marking_As_Required_Test
    {
        private class LogInModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Marking_As_Required()
        {
            Specification<LogInModel> specification = s => s
                .Member(m => m.Email, m => m.Email())
                .Member(m => m.Password);

            var signUpModel = new LogInModel
            {
                Email = "b@rt.com",
                Password = null
            };

            var report = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(signUpModel)
                .ToListReport();

            var expectedListReport = @"Password: Required
";

            Assert.Equal(expectedListReport, report.ToString());
        }
    }
}