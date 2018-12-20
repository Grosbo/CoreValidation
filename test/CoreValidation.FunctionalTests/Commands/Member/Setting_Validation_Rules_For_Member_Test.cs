using System.Linq;
using CoreValidation.Specifications;
using Newtonsoft.Json.Linq;
using Xunit;
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Commands.Member
{
    public class Setting_Validation_Rules_For_Member_Test
    {
        private class SignUpModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Setting_Validation_Rules_For_Member()
        {
            Specification<SignUpModel> specification = s => s
                .Member(m => m.Email, m => m
                    .Email()
                    .MaxLength(50)
                )
                .Member(m => m.Password, m => m
                    .LengthBetween(6, 128)
                    .Valid(p => p.Any(char.IsDigit)).WithMessage("At least one digit is required")
                    .Valid(p => p.Any(char.IsUpper)).WithMessage("At least one upper case letter is required")
                );

            var signUpModel = new SignUpModel
            {
                Email = "some@invalid@email",
                Password = "invalid_password"
            };

            var result = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(signUpModel);

            var expectedListReport = @"Email: Text value should be a valid email
Password: At least one digit is required
Password: At least one upper case letter is required
";

            var expectedModelReportJson = @"
{
  'Email': [
    'Text value should be a valid email'
  ],
  'Password': [
    'At least one digit is required',
    'At least one upper case letter is required',
  ]
}
";

            Assert.Equal(expectedListReport, result.ToListReport().ToString());

            Assert.True(JToken.DeepEquals(JToken.FromObject(result.ToModelReport()), JToken.Parse(expectedModelReportJson)));
        }
    }
}