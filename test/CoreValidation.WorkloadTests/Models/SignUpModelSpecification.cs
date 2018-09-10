using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Factory.Specifications;
using CoreValidation.Factory.Translations;
using CoreValidation.Specifications;

namespace CoreValidation.WorkloadTests.Models
{
    public class SignUpModelSpecification : ISpecificationHolder<SignUpModel>
    {
        private static readonly Specification<AddressModel> _addressSpecification = specs => specs
            .For(m => m.Street, be => be.NotWhiteSpace())
            .For(m => m.PostCode, be => be.MaxLength(10))
            .For(m => m.CountryId, be => be.GreaterThan(0))
            .Valid(m => (m.Street != null) &&
                        (m.PostCode != null) &&
                        !m.Street.Contains(m.PostCode),
                "Both street and postcode are required and need to put separate");

        private static readonly Specification<SignUpModel> _signUpSpecification = signUpSpecs => signUpSpecs
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
            .For(m => m.Address, be => be.ValidModel(_addressSpecification))
            .For(m => m.Tags, be => be
                .NotEmptyCollection("At least one tag is required")
                .MaxCollectionSize(5, "Max {max} tags allowed")
                .ValidCollection(i => i
                    .NotWhiteSpace()
                    .MaxLength(10)
                    .Valid(v => v.All(char.IsLetter), "Tag can contains only letters")))
            .For(m => m.DateOfBirth, be => be
                .WithRequiredError("Date of birth is required")
                .After(new DateTime(1900, 1, 1), message: "Earliest allowed date is {min|format=yyyy-MM-dd}"));


        public TranslationsPackage TranslationsPackage => new TranslationsPackage
        {
            {
                "Polish", new Dictionary<string, string>
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
                }
            }
        };

        public Specification<SignUpModel> Specification => _signUpSpecification;
    }
}