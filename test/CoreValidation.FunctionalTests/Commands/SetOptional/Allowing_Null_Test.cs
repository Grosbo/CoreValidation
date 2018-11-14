using CoreValidation.Specifications;
using Xunit;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace CoreValidation.FunctionalTests.Commands.SetOptional
{
    public class Allowing_Null_Test
    {
        private class Credentials
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Allowing_Null()
        {
            var credentials = new Credentials
            {
                Email = "b@rt.com"
            };

            Specification<Credentials> specification = s => s
                .Member(m => m.Username, m => m
                    .SetOptional()
                    .NotWhiteSpace()
                )
                .Member(m => m.Email, m => m.Email())
                .Member(m => m.Password);

            var report = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(credentials)
                .ToListReport();

            var expectedListReport = @"Password: Required
";

            Assert.Equal(expectedListReport, report.ToString());
        }
    }
}