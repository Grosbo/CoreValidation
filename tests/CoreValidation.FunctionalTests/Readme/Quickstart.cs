using CoreValidation.Validators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace CoreValidation.FunctionalTests.Readme
{
    public class Quickstart
    {
        private class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Should_Pass_Quickstart()
        {
            var incomingJson = @"{
                'Email': 'sample_very_long_email@deep.domain.level.tempuri.org',
                'Password': ''
            }";

            // Setting the rules for the model:
            Validator<LoginModel> loginValidator = login => login
                .For(m => m.Email, be => be
                    .Email()
                    .MaxLength(50)
                    .Valid(email => email.EndsWith("gmail.com"), "Only gmails are accepted"))
                .For(m => m.Password, be => be.NotEmpty());

            var loginModel = JsonConvert.DeserializeObject<LoginModel>(incomingJson);

            // Creating the validation context and instantly triggering the validation to get the result:
            var result = ValidationContext.Factory
                .Create(options => options.AddValidator(loginValidator))
                .Validate(loginModel);

            // Creating json-ready ModelReport from the result:
            var modelReport = result.ToModelReport();

            var expectedReportJson = @"{
                'Email': [
                    'Text value should have maximum 50 characters',
                    'Only gmails are accepted'
                ],
                'Password': [
                    'Text value cannot be empty'
                ]
            }";

            Assert.NotNull(modelReport);
            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedReportJson)));
        }
    }
}