using System.Linq;
using CoreValidation.Results;
using CoreValidation.Specifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable ArgumentsStyleLiteral

namespace CoreValidation.FunctionalTests.Readme
{
    public class Readme_Showcase_Test
    {
        private class SignUpModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string PasswordConfirmation { get; set; }
            public bool? TermsAndConditionsConsent { get; set; }
        }

        [Fact]
        public void Readme_Showcase()
        {
            Specification<SignUpModel> signUpSpecification = s => s
                .Member(m => m.Name, m => m
                    .SetOptional()
                    .MaxLength(40)
                )
                .Member(m => m.Email, m => m
                    .SetOptional()
                    .Email()
                    .MaxLength(40)
                )
                .Member(m => m.Password, m => m
                    .NotWhiteSpace()
                    .MinLength(min: 10).WithMessage("Password should contain at least {min} characters")
                    .Valid(p => p.Any(char.IsDigit)).WithMessage("Password should contain at least one digit")
                )
                .Member(m => m.PasswordConfirmation, m => m
                    .AsRelative(n => n.Password == n.PasswordConfirmation).WithMessage("Confirmation doesn't match the password")
                )
                .Member(m => m.TermsAndConditionsConsent)
                .Valid(m => (m.Name != null) || (m.Email != null)).WithMessage("At least one value is required - Name or Email");

            var validationContext = ValidationContext.Factory.Create(options => options
                .AddSpecification(signUpSpecification)
            );

            var incomingJson = @"{
                'Password': 'homerBEST',
                'PasswordConfirmation': 'homerbest'
            }";

            var logInModel = JsonConvert.DeserializeObject<SignUpModel>(incomingJson);

            var validationResult = validationContext.Validate(logInModel);

            var isLogInModelValid = validationResult.IsValid;

            Assert.False(isLogInModelValid);

            var exception = Assert.Throws<ValidationResultException<SignUpModel>>(() => { validationResult.ThrowResultIfInvalid(); });

            Assert.Same(validationResult, exception.ValidationResult);

            var listReport = validationResult.ToListReport();

            var expectedListReport =
                @"At least one value is required - Name or Email
Password: Password should contain at least 10 characters
Password: Password should contain at least one digit
PasswordConfirmation: Confirmation doesn't match the password
TermsAndConditionsConsent: Required
";

            Assert.Equal(expectedListReport, listReport.ToString());

            var modelReport = validationResult.ToModelReport();

            var expectedReportJson = @"
{
  'Password': [
    'Password should contain at least 10 characters',
    'Password should contain at least one digit',
  ],
  'PasswordConfirmation': [
    'Confirmation doesn\'t match the password'
  ],
  'TermsAndConditionsConsent': [
    'Required'
  ],
  '': [
    'At least one value is required - Name or Email'
  ]
}
";

            Assert.NotNull(modelReport);
            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedReportJson)));
        }
    }
}