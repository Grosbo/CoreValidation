using System.Linq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace CoreValidation.FunctionalTests.Specification
{
    public class For
    {
        public class UserLoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Should_ReturnError()
        {
            var validationContext = ValidationContext.Factory.Create(o => o
                .AddValidator<UserLoginModel>(u => u
                        .For(m => m.Email, be => be.Email()) // rule for 'Email' property
                )
            );

            var model = new UserLoginModel
            {
                Email = "bartosz@@@@tempuri....org", // invalid email, will return error
                Password = "1234" // never validated
            };

            const string expectedResultJson = @"{
                'Email': [
                    'Text value should be a valid email'
                ]
            }";

            var modelReport = validationContext.Validate(model).ToModelReport();

            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedResultJson)));
        }

        [Fact]
        public void Should_ReturnErrors_For_DifferentMembers()
        {
            var validationContext = ValidationContext.Factory.Create(o => o
                .AddValidator<UserLoginModel>(u => u
                    .For(m => m.Email, be => be
                        .Email()
                        .Valid(v => v.EndsWith("@tempuri.org"), "Email should be from the tempuri.org domain"))
                    .For(m => m.Password, be => be
                        .MinLength(5))
                )
            );

            var model = new UserLoginModel
            {
                Email = "bartosz@bing.com",
                // the value in 'Email':
                // - is valid email address, passes Email() rule without returning error
                // - isn't from @tempuri.org domain, fails custom validation and returns error
                Password = "bart" // too short, doesn't pass MinLenght(5), will return error
            };

            const string expectedResultJson = @"{
                'Email' : [
                    'Email should be from the tempuri.org domain'
                ],
                'Password': [
                    'Text value should have minimum 5 characters'
                ]
            }";

            var modelReport = validationContext.Validate(model).ToModelReport();

            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedResultJson)));
        }

        [Fact]
        public void Should_ReturnMultipleErrors_When_GroupDefinitions()
        {
            var validationContext = ValidationContext.Factory.Create(o => o
                .AddValidator<UserLoginModel>(u => u
                    .For(m => m.Password, be => be // three rules for Password property defined within one group
                        .NotEmpty()
                        .MinLength(5)
                        .Valid(v => v.Any(char.IsDigit), "At least one digit is required")
                        .Valid(v => v.Any(char.IsUpper), "At least one upper case character is required"))
                )
            );

            var model = new UserLoginModel
            {
                Email = "bartosz@tempuri.org", // never validated
                Password = "bart"
                // the value in 'Password':
                // - is not empty, so passes NotEmpty() rule and won't return error
                // - is too short to pass the MinLength(5) rule, will return first error
                // - doesn't contain digits (custom rule), will return second error
                // - doesn't contain upper case characters (custom rule), will return third error
            };

            const string expectedResultJson = @"{
                'Password': [
                    'Text value should have minimum 5 characters',
                    'At least one digit is required',
                    'At least one upper case character is required',
                ]
            }";

            var modelReport = validationContext.Validate(model).ToModelReport();

            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedResultJson)));
        }


        [Fact]
        public void Should_ReturnMultipleErrors_When_SeparateDefinitions()
        {
            var validationContext = ValidationContext.Factory.Create(o => o
                .AddValidator<UserLoginModel>(u => u
                    .For(m => m.Password, be => be.NotEmpty())
                    .For(m => m.Password, be => be.MinLength(5))
                    .For(m => m.Password, be => be.Valid(v => v.Any(char.IsDigit), "At least one digit is required"))
                    .For(m => m.Password, be => be.Valid(v => v.Any(char.IsUpper), "At least one upper case character is required"))
                )
            );

            var model = new UserLoginModel
            {
                Email = "bartosz@tempuri.org", // never validated
                Password = "bart"
                // the value in 'Password':
                // - is not empty, so passes NotEmpty() rule and won't return error
                // - is too short to pass the MinLength(5) rule, will return first error
                // - doesn't contain digits (custom rule), will return second error
                // - doesn't contain upper case characters (custom rule), will return third error
            };

            const string expectedResultJson = @"{
                'Password': [
                    'Text value should have minimum 5 characters',
                    'At least one digit is required',
                    'At least one upper case character is required',
                ]
            }";

            var modelReport = validationContext.Validate(model).ToModelReport();

            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedResultJson)));
        }

        [Fact]
        public void Should_ReturnNoError_When_NoRuleSpecified()
        {
            var validationContext = ValidationContext.Factory.Create(o => o
                .AddValidator<UserLoginModel>(u => u
                        .For(m => m.Password, be => be.NotEmpty()) // rule for 'Password' property
                )
            );

            var model = new UserLoginModel
            {
                Email = "bartosz@@@@tempuri.org", // invalid email, but there is no rule for 'Email' property specified
                Password = "1234" // 'Password' is valid (passes 'NotEmpty' rule), so won't return error
            };

            const string expectedResultJson = @"{ }";

            var modelReport = validationContext.Validate(model).ToModelReport();

            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedResultJson)));
        }

        [Fact]
        public void Should_ReturnNoError_When_ValidModel()
        {
            var validationContext = ValidationContext.Factory.Create(o => o
                .AddValidator<UserLoginModel>(u => u
                        .For(m => m.Email, be => be.Email()) // rule for 'Email' property
                )
            );

            var model = new UserLoginModel
            {
                Email = "bartosz@tempuri.org", // valid email, the validation won't return error
                Password = "1234" // never validated
            };

            const string expectedResultJson = @"{ }";

            var modelReport = validationContext.Validate(model).ToModelReport();

            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedResultJson)));
        }
    }
}