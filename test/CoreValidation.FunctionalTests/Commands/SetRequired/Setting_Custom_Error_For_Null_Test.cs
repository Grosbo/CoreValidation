using CoreValidation.Specifications;
using Xunit;
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Commands.SetRequired
{
    public class Setting_Custom_Error_For_Null_Test
    {
        private class Credentials
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Setting_Custom_Error_For_Null()
        {
            var credentials = new Credentials
            {
                Email = "b@rt.com"
            };

            Specification<Credentials> specification = s => s
                .Member(m => m.Username, m => m
                    .SetRequired("Please provide the username")
                )
                .Member(m => m.Email, m => m.Email())
                .Member(m => m.Password);

            var report = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(credentials)
                .ToListReport();

            var expectedListReport = @"Username: Please provide the username
Password: Required
";

            Assert.Equal(expectedListReport, report.ToString());
        }
    }
}