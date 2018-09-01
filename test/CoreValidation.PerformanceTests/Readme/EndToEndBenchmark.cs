using System;
using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using CoreValidation.Factory.Specifications;
using CoreValidation.Specifications;
using CoreValidation.Translations;

namespace CoreValidation.PerformanceTests.Readme
{
    [MemoryDiagnoser]
    public class EndToEndBenchmark
    {
        private UserModel[] _models;
        private IValidationContext _validationContext;


        [Params(1, 500, 5000)]
        public int N { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            Specification<AddressModel> addressSpecification = specs => specs
                .For(m => m.Street, be => be.NotWhiteSpace())
                .For(m => m.PostCode, be => be.MaxLength(10))
                .For(m => m.CountryId, be => be.GreaterThan(0))
                .Valid(m => (m.Street != null) &&
                            (m.PostCode != null) &&
                            !m.Street.Contains(m.PostCode),
                    "Both street and postcode are required and need to put separate");

            // ReSharper disable once ImplicitlyCapturedClosure
            Specification<UserModel> userSpecification = specs => specs
                .For(m => m.Email, be => be.Email())
                .For(m => m.Name, be => be
                    .Optional()
                    .LengthBetween(6, 15)
                    .Valid(v => char.IsLetter(v.FirstOrDefault()), "Must start with a letter")
                    .Valid(v => v.All(char.IsLetterOrDigit), "Must contains only letters and digits"))
                .For(m => m.Password, be => be
                    .MinLength(6)
                    .NotWhiteSpace()
                    .Valid(v => v.Any(char.IsUpper) && v.Any(char.IsDigit))
                    .WithSummaryError("Minimum 6 characters, at least one upper case and one digit"))
                .For(m => m.PasswordConfirmation, be => be
                    .WithName("Confirmation")
                    .ValidRelative(m => m.Password == m.PasswordConfirmation, "Confirmation doesn't match password"))
                .For(m => m.Address, be => be.ValidModel(addressSpecification))
                .For(m => m.Tags, be => be
                    .NotEmpty("At least one tag is required")
                    .MaxSize(5, "Max {max} tags allowed")
                    .ValidCollection(i => i
                        .NotWhiteSpace()
                        .MaxLength(10)
                        .Valid(v => v.All(char.IsLetter), "Tag can contains only letters")))
                .For(m => m.DateOfBirth, be => be
                    .WithRequiredError("Date of birth is required")
                    .After(new DateTime(1900, 1, 1), message: "Earliest allowed date is {min|format=yyyy-MM-dd}"));

            _validationContext = ValidationContext.Factory.Create(options => options
                .AddSpecification(userSpecification)
                .AddSpecification(addressSpecification)
                .AddPolishTranslation(false, new Dictionary<string, string>
                {
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
                    {Phrases.Keys.Texts.Email, "Email jest wymagany"},
                    {"Max {max} tags allowed", "Maksymalnie dozwolonych jest {max} tagów"},
                    {"Earliest allowed date is {min|format=yyyy-MM-dd}", "Najwcześniejsza dozwolona data to {min|format=dd.MM.yyy}"}
                })
                .SetValidationStrategy(ValidationStrategy.Complete)
                .SetNullRootStrategy(NullRootStrategy.ArgumentNullException)
                .SetMaxDepth(5)
            );
        }

        [IterationSetup]
        public void IterationSetup()
        {
            _models = Enumerable.Range(0, N).Select(n => new UserModel
            {
                Email = $"invalid@@@email.com_{n}",
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
            }).ToArray();
        }

        [Benchmark]
        public void EndToEnd()
        {
            for (var i = 0; i < N; ++i)
            {
                // ReSharper disable once UnusedVariable
                var result = _validationContext.Validate(_models[i]);
            }
        }

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
    }
}