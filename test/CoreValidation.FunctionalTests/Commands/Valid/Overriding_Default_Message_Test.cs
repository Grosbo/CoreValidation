using System.Linq;
using CoreValidation.Specifications;
using Xunit;
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Commands.Valid
{
    public class Overriding_Default_Message_Test
    {
        private class SignUpModel
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Overriding_Default_Message()
        {
            Specification<SignUpModel> specification = s => s
                .Member(m => m.Name, m => m
                    .Valid(name => !name.Any(char.IsWhiteSpace)).WithMessage("White space is not allowed")
                )
                .Member(m => m.Password, m => m.Valid(password =>
                    {
                        return password.Any() &&
                               password.Any(char.IsUpper) &&
                               password.Any(char.IsLower) &&
                               password.Any(char.IsDigit);
                    }).WithMessage("Password must contain digits, upper and lower case letters")
                )
                .Valid(m =>
                {
                    return (m.Name != null) &&
                           (m.Password != null) &&
                           !m.Password.Contains(m.Name);
                }).WithMessage("Password cannot contain the name");

            var model = new SignUpModel
            {
                Name = "john doe",
                Password = "john doe 123"
            };

            var report = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(model)
                .ToListReport();

            var expectedListReport = @"Password cannot contain the name
Name: White space is not allowed
Password: Password must contain digits, upper and lower case letters
";

            Assert.Equal(expectedListReport, report.ToString());
        }
    }
}