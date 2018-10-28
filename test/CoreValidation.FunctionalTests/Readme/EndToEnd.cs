using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Factory.Specifications;
using CoreValidation.Results;
using CoreValidation.Specifications;
using Newtonsoft.Json.Linq;
using Xunit;

// ReSharper disable ImplicitlyCapturedClosure
// ReSharper disable ArgumentsStyleStringLiteral
// ReSharper disable ArgumentsStyleOther
// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable ArgumentsStyleLiteral

namespace CoreValidation.FunctionalTests.Readme
{
    public class EndToEnd
    {
        private class UserModel
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public string PasswordConfirmation { get; set; }
            public AddressModel Address { get; set; }
            public IEnumerable<string> Tags { get; set; }
            public DateTime? DateOfBirth { get; set; }
        }

        private class AddressModel
        {
            public string Street { get; set; }
            public string PostCode { get; set; }
            public int CountryId { get; set; }
        }

        [Fact]
        public void Should_Pass_EndToEnd()
        {
            Specification<AddressModel> addressSpecification = specs => specs
                // Validate members in their scope (error attached to the selected member):
                .Member(m => m.Street, be => be.NotWhiteSpace())
                .Member(m => m.PostCode, be => be.MaxLength(10))
                .Member(m => m.CountryId, be => be.GreaterThan(0))

                // Validate model in its global scope (error attached to the model instead of its member):
                .Valid(m => (m.Street != null) &&
                            (m.PostCode != null) &&
                            !m.Street.Contains(m.PostCode)).WithMessage("Both street and postcode are required and need to put separate");

            Specification<UserModel> userSpecification = specs => specs
                // Validate members with the predefined rules:
                .Member(m => m.Email, be => be.Email())

                // Apply many rules along with custom predicates:
                .Member(m => m.Name, be => be
                    // By default, everything specified is required, so marking the selected member as optional:
                    .SetOptional()
                    // If present, proceed with validation:
                    .LengthBetween(6, 15)
                    // The value is always guaranteed to be non-null inside the predicate:
                    .Valid(v => char.IsLetter(v.FirstOrDefault()), "Must start with a letter")
                    .Valid(v => v.All(char.IsLetterOrDigit), "Must contains only letters and digits"))

                // Replace all errors with a single one:
                .Member(m => m.Password, be => be
                    .MinLength(6)
                    .NotWhiteSpace()
                    .Valid(v => v.Any(char.IsUpper) && v.Any(char.IsDigit))
                    // If any rule in the chain fails, only the SingleError is recorded:
                    .SetSingleError("Minimum 6 characters, at least one upper case and one digit"))

                // Validate relations with other members:
                .Member(m => m.PasswordConfirmation, be => be
                    // Argument in predicate is the parent model, but error will be attached in the selected member scope:
                    .AsRelative(m => m.Password == m.PasswordConfirmation).WithMessage("Confirmation doesn't match password")
                )

                // Validate nested model:
                .Member(m => m.Address, be => be.AsModel(addressSpecification))

                // Validate collection:
                .Member(m => m.Tags, be => be
                    // Override default message for predefine rule:
                    .NotEmptyCollection().WithMessage("At least one tag is required")
                    // All rule arguments can be inserted in the message using {argumentName} pattern:
                    .MaxCollectionSize(max: 5).WithMessage("Max {max} tags allowed")
                    // Validate every item inside of the collection:
                    .AsCollection(i => i
                        .NotWhiteSpace()
                        .MaxLength(10)
                        .Valid(v => v.All(char.IsLetter), "Tag can contains only letters")))

                // Validate nullables:
                .Member(m => m.DateOfBirth, be => be
                    // Override default RequiredError for selected member:
                    .SetRequired("Date of birth is required")
                    // Arguments could be parametrized:
                    .After(min: new DateTime(1900, 1, 1)).WithMessage("Earliest allowed date is {min|format=yyyy-MM-dd}"));

            var validationContext = ValidationContext.Factory.Create(options => options
                // Add specifications for all models to validate (including nested ones)
                .AddSpecification(userSpecification)
                .AddSpecification(addressSpecification)

                // Add translations (to have possibility to serve results in different language), e.g. Polish
                .AddPolishTranslation(asDefault: false, include: new Dictionary<string, string>
                {
                    // Add more phrases to Polish translation - e.g. for the custom messages used in userValidator
                    // Translation is a standard dictionary, so you can easily deserialize it from JSON, or create inline like:
                    {
                        "Both street and postcode are required and need to put separate",
                        "Ulica i kod pocztowy są wymagane i muszą być umieszczone osobno"
                    },
                    {"Must start with a letter", "Musi zaczynać się literą"},
                    {"Must contains only letters and digits", "Musi zawierać tylko litery i cyfry"},
                    {
                        "Minimum 6 characters, at least one upper case and one digit",
                        "Minimum 6 znaków, przynajmniej jedna wielka litera i jedna cyfra"
                    },
                    {"Confirmation doesn't match password", "Potwierdzenie nie pasuje do hasła"},
                    {"At least one tag is required", "Przynamniej jeden tag jest wymagany"},
                    {"Tag can contains only letters", "Tag może zawierać tylko litery"},

                    {"Date of birth is required", "Data urodzenia jest wymagana"},

                    // Override the default entry (static Phrases.Keys holds all default keys for the phrases)
                    {Phrases.Keys.Texts.Email, "Email jest wymagany"},

                    // You can use arguments in translations...
                    {"Max {max} tags allowed", "Maksymalnie dozwolonych jest {max} tagów"},

                    // ... and parametrize them differently (e.g. change the format, culture, etc.)
                    {"Earliest allowed date is {min|format=yyyy-MM-dd}", "Najwcześniejsza dozwolona data to {min|format=dd.MM.yyy}"}
                })

                // Set additional options (lot of them available...)
                .SetValidationStrategy(ValidationStrategy.Complete)
                .SetNullRootStrategy(NullRootStrategy.ArgumentNullException)
                .SetMaxDepth(5)
            );

            var userModel = new UserModel
            {
                Email = "invalid@@@email.com",
                Name = null,
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
                    "validtag",
                    "inv@lid",
                    " ",
                    null,
                    "oktag",
                    null
                },
                DateOfBirth = null
            };

            // When the time comes, use IValidationContext instance to validate incoming model
            var result = validationContext.Validate(userModel);

            // Once the result is produced, you can process it in many different ways, e.g. convert it to a report
            var modelReport = result.ToModelReport();

            // The outcome of JsonConvert.SerializeObject(modelReport) would be
            var expectedReportJson = @"{
                'Email': ['Text value should be a valid email'],
                'Password': ['Minimum 6 characters, at least one upper case and one digit'],
                'PasswordConfirmation': ['Confirmation doesn\'t match password'],
                'Address': {
                    '': ['Both street and postcode are required and need to put separate'],
                    'Street': ['Required'],
                    'PostCode': ['Text value should have maximum 10 characters'],
                    'CountryId': ['Number should be greater than 0']
                },
                'Tags': {
                    '': ['Max 5 tags allowed'],
                    '1': ['Tag can contains only letters'],
                    '2': ['Text value cannot be whitespace', 'Tag can contains only letters'],
                    '3': ['Required'],
                    '5': ['Required']
                },
                'DateOfBirth': ['Date of birth is required']
            }";

            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedReportJson)));

            // Variety of other actions are available, like `ListReport`, which is a collection of strings
            var listReport = result.ToListReport();

            // You can enumerate over `listReport` to read the error messages or combine them all using  `listReport.ToString()`
            var expectedListReport =
                @"Email: Text value should be a valid email
Password: Minimum 6 characters, at least one upper case and one digit
PasswordConfirmation: Confirmation doesn't match password
Address: Both street and postcode are required and need to put separate
Address.Street: Required
Address.PostCode: Text value should have maximum 10 characters
Address.CountryId: Number should be greater than 0
Tags: Max 5 tags allowed
Tags.1: Tag can contains only letters
Tags.2: Text value cannot be whitespace
Tags.2: Tag can contains only letters
Tags.3: Required
Tags.5: Required
DateOfBirth: Date of birth is required
";

            Assert.Equal(expectedListReport, listReport.ToString());

            // Adjust ValidationOptions for a single validation
            var failFastResult = validationContext.Validate(userModel, options => options
                // You can override all options of the IValidationContext (except validators and translations):
                // `FailFast` strategy - model will be validated until the first error
                .SetValidationStrategy(ValidationStrategy.FailFast)
            );

            var failFastModelReport = failFastResult.ToModelReport();

            var expectedfailFastReportJson = @"{
                'Email': ['Text value should be a valid email']
            }";

            // All report creators provide `translationName` optional argument
            var listReportInPolish = failFastResult.ToListReport(translationName: "Polish");

            Assert.True(JToken.DeepEquals(JToken.FromObject(failFastModelReport), JToken.Parse(expectedfailFastReportJson)));

            var expectedPolishFailFastListReport =
                @"Email: Email jest wymagany
";

            Assert.Equal(expectedPolishFailFastListReport, listReportInPolish.ToString());

            Assert.False(result.IsValid);

            Assert.Throws<ValidationResultException<UserModel>>(() => { result.ThrowResultIfInvalid(); });
        }
    }
}