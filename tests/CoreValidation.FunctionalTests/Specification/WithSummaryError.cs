using System.Linq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace CoreValidation.FunctionalTests.Specification
{
    public class SummaryError
    {
        private class ResetPasswordModel
        {
            public string SecurityToken { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Should_Fail_When_SettingMultipleSummaryErrors()
        {
            var validationContext = ValidationContext.Factory.Create(o => o
                .AddValidator<ResetPasswordModel>(u => u
                        .For(m => m.SecurityToken, be => be
                            .NotWhiteSpace()
                            .ExactLength(6))
                        .WithSummaryError("Invalid security token") // summary error set for the first time
                        .For(m => m.Password, be => be
                            .NotEmpty()
                            .MinLength(5)
                            .Valid(v => v.Any(char.IsDigit), "At least one digit is required")
                            .Valid(v => v.Any(char.IsUpper), "At least one upper case character is required"))
                        .WithSummaryError("Invalid password") // invalid! multiple 'WithSummaryError' for one model!
                )
            );

            var model = new ResetPasswordModel
            {
                SecurityToken = "xxx",
                Password = "bartxxx" // model doesn't pass validation, because 'Password' contains 'SecurityToken'
            };

            Assert.Throws<ValidationException>(() => { validationContext.Validate(model); });
        }

        [Fact]
        public void Should_ReturnSummaryError_When_ModelInvalid()
        {
            var validationContext = ValidationContext.Factory.Create(o => o
                .AddValidator<ResetPasswordModel>(u => u
                    .Valid(m => (m.Password != null) &&
                                (m.SecurityToken != null) &&
                                !m.Password.Contains(m.SecurityToken), "Password cannot contain security token")
                    .WithSummaryError("Invalid password")
                )
            );

            var model = new ResetPasswordModel
            {
                SecurityToken = "xxx",
                Password = "bartxxx" // model doesn't pass validation, because 'Password' contains 'SecurityToken'
            };

            // summary error will overwrite all of the errors from the model
            const string expectedResultJson = @"[
                    'Invalid password'
            ]";

            var modelReport = validationContext.Validate(model).ToModelReport();

            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedResultJson)));
        }

        [Fact]
        public void Should_ReturnSummaryError_When_MultipleMembersInvalid()
        {
            var validationContext = ValidationContext.Factory.Create(o => o
                .AddValidator<ResetPasswordModel>(u => u
                    .For(m => m.SecurityToken, be => be
                        .NotWhiteSpace()
                        .ExactLength(6))
                    .For(m => m.Password, be => be
                        .NotEmpty()
                        .MinLength(5)
                        .Valid(v => v.Any(char.IsDigit), "At least one digit is required")
                        .Valid(v => v.Any(char.IsUpper), "At least one upper case character is required"))
                    .WithSummaryError("Invalid password")
                )
            );

            var model = new ResetPasswordModel
            {
                SecurityToken = "xxx", // value doesn't pass 1/2 rules: exact length
                Password = "bart" // value doesn't pass 3/4 rules: minimum length, digit and upper case requirements
            };

            // no matter how many invalid members will fail validation, only one error will be returned
            const string expectedResultJson = @"[
                    'Invalid password',
            ]";

            var modelReport = validationContext.Validate(model).ToModelReport();

            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedResultJson)));
        }
    }
}