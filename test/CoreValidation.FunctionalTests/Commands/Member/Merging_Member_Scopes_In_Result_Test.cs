using System.Linq;
using CoreValidation.Specifications;
using Newtonsoft.Json.Linq;
using Xunit;
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Commands.Member
{
    public class Merging_Member_Scopes_In_Result_Test
    {
        private class SignUpModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Merging_Member_Scopes_In_Result()
        {
            Specification<SignUpModel> specification = s => s
                .Member(m => m.Email, m => m.Email())
                .Member(m => m.Password, m => m
                    .MinLength(16)
                    .MaxLength(128)
                    .AsRelative(p => p.Password != p.Email).WithMessage("Password cannot be same as email")
                )
                .Member(m => m.Password, m => m
                    .Valid(p => p.Any(char.IsDigit))
                    .Valid(p => p.Any(char.IsUpper))
                    .Valid(p => p.Any(char.IsLower))
                    .Valid(p => p.Any(char.IsWhiteSpace))
                ).WithMessage("Password isn't complex enough");

            var signUpModel = new SignUpModel
            {
                Email = "b@rt.com",
                Password = "b@rt.com"
            };

            var validationContext = ValidationContext.Factory.Create(options => options
                .AddSpecification(specification)
            );

            var result = validationContext.Validate(signUpModel);

            Assert.Equal(1, result.ErrorsCollection.Members.Count);

            var expectedListReport = @"Password: Text value should have minimum 16 characters
Password: Password cannot be same as email
Password: Password isn't complex enough
";

            var expectedModelReportJson = @"
{
  'Password': [
    'Text value should have minimum 16 characters',
    'Password cannot be same as email',
    'Password isn\'t complex enough'
  ]
}
";

            Assert.Equal(expectedListReport, result.ToListReport().ToString());

            Assert.True(JToken.DeepEquals(JToken.FromObject(result.ToModelReport()), JToken.Parse(expectedModelReportJson)));
        }
    }
}