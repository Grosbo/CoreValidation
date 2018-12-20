using CoreValidation.Specifications;
using Xunit;
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Commands.SetOptional
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
                    .SetOptional()
                    .NotWhiteSpace()
                    .LengthBetween(3, 20)
                )
                .Member(m => m.Email, m => m.Email())
                .Member(m => m.Password);

            Specification<Credentials> specification2 = s => s
                .Member(m => m.Username, m => m
                    .NotWhiteSpace()
                    .SetOptional()
                    .LengthBetween(3, 20)
                )
                .Member(m => m.Email, m => m.Email())
                .Member(m => m.Password);

            Specification<Credentials> specification3 = s => s
                .Member(m => m.Username, m => m
                    .NotWhiteSpace()
                    .LengthBetween(3, 20)
                    .SetOptional()
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

            var expectedReportOptional = @"Password: Required
";

            Assert.Equal(expectedReportOptional, report1.ToString());
            Assert.Equal(expectedReportOptional, report2.ToString());
            Assert.Equal(expectedReportOptional, report3.ToString());
        }
    }
}