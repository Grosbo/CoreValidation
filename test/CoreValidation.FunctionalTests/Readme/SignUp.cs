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
    public class SignUp
    {
        private class SignUpModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string PasswordConfirmation { get; set; }
            public bool? TermsAndConditionsConsent { get; set; }
        }

        [Fact]
        public void Should_PassScenario_SignUp()
        {
            Specification<SignUpModel> signUpModelSpecification = s => s
                .Member(m => m.Email, m => m
                    .Email()
                    .MaxLength(40))
                .Member(m => m.Password, m => m
                    .NotWhiteSpace()
                    .MinLength(min: 10).WithMessage("Password should contain at least {min} characters")
                    .Valid(p => p.Any(char.IsDigit)).WithMessage("Password should contain at least one digit")
                )
                .Member(m => m.PasswordConfirmation, m => m
                    .AsRelative(n => n.Password == n.PasswordConfirmation).WithMessage("Confirmation doesn't match the password")
                )
                .Member(m => m.TermsAndConditionsConsent)
                .Valid(m => m.TermsAndConditionsConsent == true).WithMessage("Without the consent, sign up is invalid");

            var validationContext = ValidationContext.Factory.Create(options => options
                .AddSpecification(signUpModelSpecification)
            );

            var incomingJson = @"{
                'Email': 'homer.jay.simpson@emailaccount.tempuri.org',
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
                @"Without the consent, sign up is invalid
Email: Text value should have maximum 40 characters
Password: Password should contain at least 10 characters
Password: Password should contain at least one digit
PasswordConfirmation: Confirmation doesn't match the password
TermsAndConditionsConsent: Required
";

            Assert.Equal(expectedListReport, listReport.ToString());

            var modelReport = validationResult.ToModelReport();

            var expectedReportJson = @"
{
  'Email': [
    'Text value should have maximum 40 characters'
  ],
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
    'Without the consent, sign up is invalid'
  ]
}
";

            Assert.NotNull(modelReport);
            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedReportJson)));
        }
    }
}