using CoreValidation.Specifications;
using Xunit;
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace CoreValidation.FunctionalTests.Commands.SetRequired
{
    public class Order_Doesnt_Matter_Test
    {
        private class Credentials
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Order_Doesnt_Matter()
        {
            var credentials = new Credentials
            {
                Email = "b@rt.com"
            };

            Specification<Credentials> specification1 = s => s
                .Member(m => m.Username, m => m
                    .SetRequired("Please provide the username")
                    .NotWhiteSpace()
                    .LengthBetween(3, 20)
                )
                .Member(m => m.Email, m => m.Email())
                .Member(m => m.Password);

            Specification<Credentials> specification2 = s => s
                .Member(m => m.Username, m => m
                    .NotWhiteSpace()
                    .SetRequired("Please provide the username")
                    .LengthBetween(3, 20)
                )
                .Member(m => m.Email, m => m.Email())
                .Member(m => m.Password);

            Specification<Credentials> specification3 = s => s
                .Member(m => m.Username, m => m
                    .NotWhiteSpace()
                    .LengthBetween(3, 20)
                    .SetRequired("Please provide the username")
                )
                .Member(m => m.Email, m => m.Email())
                .Member(m => m.Password);

            var report1 = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification1)
                )
                .Validate(credentials)
                .ToListReport();

            var report2 = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification2)
                )
                .Validate(credentials)
                .ToListReport();

            var report3 = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification3)
                )
                .Validate(credentials)
                .ToListReport();

            var expectedListReport = @"Username: Please provide the username
Password: Required
";

            Assert.Equal(expectedListReport, report1.ToString());
            Assert.Equal(expectedListReport, report2.ToString());
            Assert.Equal(expectedListReport, report3.ToString());
        }
    }
}