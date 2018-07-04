using System.Collections.Generic;
using System.Linq;
using CoreValidation.Results;
using CoreValidation.Validators;
using Newtonsoft.Json.Linq;
using Xunit;
// ReSharper disable ImplicitlyCapturedClosure
// ReSharper disable ArgumentsStyleStringLiteral

namespace CoreValidation.FunctionalTests.ShowMeTheCode
{
    public class ShowMeTheCodeTests
    {
        class UserModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string PasswordConfirmation { get; set; }
            public AddressModel Address { get; set; }
            public IEnumerable<string> Tags { get; set; }
            public int? Age { get; set; }
        }

        class AddressModel
        {
            public string Street { get; set; }
            public string PostCode { get; set; }
            public int CountryId { get; set; }
        }

        [Fact]
        public void BasicTutorial()
        {
            Validator<AddressModel> addressValidator = specs => specs
                // Validate model's members:
                .For(m => m.Street, be => be.NotWhiteSpace())
                .For(m => m.PostCode, be => be.MaxLength(10))
                .For(m => m.CountryId, be => be.GreaterThan(0))

                // Validate model in it's global scope:
                .Valid(m => (m.Street != null) &&
                            (m.PostCode != null) &&
                            !m.Street.Contains(m.Street),
                    "Both street and postcode are required and need to put separate");

            Validator<UserModel> userValidator = specs => specs
                // Validate members with predefined rules:
                .For(m => m.Email, be => be.Email())

                // Apply many rules along with custom predicates:
                .For(m => m.Password, be => be
                    .MinLength(6)
                    .NotWhiteSpace()
                    .Valid(v => v.Any(char.IsUpper), "Must contain an upper case letter")
                    .Valid(v => v.Any(char.IsDigit), "Must contain a digit"))

                // Validate relations with other members:
                .For(m => m.PasswordConfirmation, be => be
                    .ValidRelative(m => m.Password == m.PasswordConfirmation,
                        "Confirmation doesn't match password"))

                // Validate nested models:
                .For(m => m.Address, be => be.ValidModel(addressValidator))

                // Validate collections:
                .For(m => m.Tags, be => be
                    // Override default message for predefine rule:
                    .NotEmpty(message: "At least one tag required")
                    .MaxSize(10, message: "Max 10 tags allowed")
                    // Validate every item inside of the collection:
                    .ValidCollection(i => i
                        .NotWhiteSpace()
                        .MaxLength(10)
                        .Valid(v => char.IsLetter(v.FirstOrDefault()), "Tag must start with a letter")
                        .Valid(v => v.All(char.IsLetterOrDigit), "Tag can contains only letters and digits")))

                // Validate nullables:
                .For(m => m.Age, be => be
                    // Mark member as not required:
                    .Optional()
                    // If present, apply some rules:
                    .GreaterThan(0));

            var validationContext = ValidationContext.Factory.Create(options => options
                // Add validators
                .AddValidator(userValidator)
                .AddValidator(addressValidator)

                // Set additional options (there are lot of them available...)
                .SetValidationStategy(ValidationStrategy.Complete)
                .SetRequiredError("Value is required")
            );

            var userModel = new UserModel
            {
                Email = "invalid@@@email.com",
                Password = "12345",
                PasswordConfirmation = "1234567",
                Address = new AddressModel
                {
                    Street = null,
                    PostCode = "1234-5678-90",
                    CountryId = -1
                },
                Tags = new[]
                {
                    "Test",
                    "special#@",
                    "test2",
                    null,
                    "1stdigit"
                },
                Age = null
            };

            // When the time comes, use IValidationContext instance to validate incoming model
            var result = validationContext.Validate(userModel);

            // Once the result is produced, you can process it in many different ways, e.g. convert it to a report
            var modelReport = result.ToModelReport();

            // The outcome of JsonConvert.SerializeObject(modelReport) would be
            var expectedReportJson = @"{
                'Email': ['Text value should be a valid email'],
                'Password': ['Text value should have minimum 6 characters', 'Must contain an upper case letter'],
                'PasswordConfirmation': ['Confirmation doesn\'t match password'],
                'Address': {
                    '': ['Both street and postcode are required and need to put separate'],
                    'Street': ['Value is required'],
                    'PostCode': ['Text value should have maximum 10 characters'],
                    'CountryId': ['Number should be greater than 0']
                },
                'Tags': {
                    '1': ['Tag can contains only letters and digits'],
                    '3': ['Value is required'],
                    '4': ['Tag must start with a letter']
                }
            }";

            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedReportJson)));

            // Variety of other actions are available, like `ListReport`, which is a collection of strings
            var listReport = result.ToListReport();

            // You can enumerate over `listReport` to read the error messages or combine them all using  `listReport.ToString()`
            var expectedListReport =
@"Email: Text value should be a valid email
Password: Text value should have minimum 6 characters
Password: Must contain an upper case letter
PasswordConfirmation: Confirmation doesn't match password
Address: Both street and postcode are required and need to put separate
Address.Street: Value is required
Address.PostCode: Text value should have maximum 10 characters
Address.CountryId: Number should be greater than 0
Tags.1: Tag can contains only letters and digits
Tags.3: Value is required
Tags.4: Tag must start with a letter
";

            Assert.Equal(expectedListReport, listReport.ToString());

            Assert.False(result.IsValid());

            Assert.Throws<InvalidModelException<UserModel>>(() => { result.ThrowIfInvalid(); });
        }
    }
}