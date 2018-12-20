using System.Linq;
using CoreValidation.Specifications;
using Xunit;

// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Commands.Valid
{
    public class Custom_Logic_For_Member_Test
    {
        private class SignUpModel
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Custom_Logic_For_Member()
        {
            Specification<SignUpModel> specification = s => s
                .Member(m => m.Name, m => m.Valid(name => !name.Any(char.IsWhiteSpace)))
                .Member(m => m.Password, m => m.Valid(password =>
                    {
                        return password.Any() &&
                               password.Any(char.IsUpper) &&
                               password.Any(char.IsLower) &&
                               password.Any(char.IsDigit);
                    })
                );

            var model = new SignUpModel
            {
                Name = "John Doe",
                Password = "john123"
            };

            var report = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(model)
                .ToListReport();

            var expectedListReport = @"Name: Invalid
Password: Invalid
";

            Assert.Equal(expectedListReport, report.ToString());
        }
    }
}