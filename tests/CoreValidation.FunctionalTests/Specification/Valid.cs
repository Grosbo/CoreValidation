using System;
using Newtonsoft.Json.Linq;
using Xunit;

// ReSharper disable ConvertToLambdaExpression
// ReSharper disable ConvertIfStatementToReturnStatement

namespace CoreValidation.FunctionalTests.Specification
{
    public class Valid
    {
        public class RegisterUserModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public string PasswordConfirmation { get; set; }
        }

        [Fact]
        public void Should_ReturnError()
        {
            var validationContext = ValidationContext.Factory.Create(o => o
                .AddValidator<RegisterUserModel>(u => u
                    .Valid(registerUserModel => // a way to validate model globally, errors won't be attached any member
                        {
                            // the input parameter has the same reference as the validated model
                            return registerUserModel.Password == registerUserModel.PasswordConfirmation;
                        },
                        "Confirmation doesn't match the password")
                )
            );

            var model = new RegisterUserModel
            {
                Password = "1234",
                PasswordConfirmation = "1234567" // not the same as in 'Password' property, will return error
            };

            const string expectedResultJson = @"[
                    'Confirmation doesn\'t match the password'
                ]";

            var modelReport = validationContext.Validate(model).ToModelReport();

            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedResultJson)));
        }

        [Fact]
        public void Should_ReturnMultipleErrors()
        {
            bool PasswordProvided(RegisterUserModel registerUserModel)
            {
                // any kind of complex logic in a separate method
                if ((registerUserModel.Password != null) && (registerUserModel.PasswordConfirmation != null))
                {
                    return true;
                }

                return false;
            }


            var validationContext = ValidationContext.Factory.Create(o => o
                .AddValidator<RegisterUserModel>(u => u
                    .Valid(PasswordProvided, "No password provided")
                    .Valid(m => m.Password == m.PasswordConfirmation, "Confirmation doesn't match the password")
                    .Valid(m => // logic wrapped in inline function
                        {
                            if (!m.Password.Contains(m.Username, StringComparison.InvariantCultureIgnoreCase))
                            {
                                return true;
                            }

                            return false;
                        },
                        "Password cannot contain user name")
                )
            );

            // the model contains 'Password' and 'PasswordConfirmation', so passes 'PasswordProvided' validation
            var model = new RegisterUserModel
            {
                Username = "john_smith_89",
                Password = "john_SMITH_89_password", // contains 'Username' value (ignoring case), will return error
                PasswordConfirmation = "john_smith_89__password" // not same as in 'Password' property
            };

            const string expectedResultJson = @"[
                    'Confirmation doesn\'t match the password',
                    'Password cannot contain user name'
                ]";

            var modelReport = validationContext.Validate(model).ToModelReport();

            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedResultJson)));
        }
    }
}