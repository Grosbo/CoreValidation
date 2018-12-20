using System.Linq;
using CoreValidation.Specifications;
using Newtonsoft.Json.Linq;
using Xunit;

// ReSharper disable ImplicitlyCapturedClosure

namespace CoreValidation.FunctionalTests.Commands.AsModel
{
    // ReSharper disable once InconsistentNaming
    public class Specification_From_ValidationContext_Test
    {
        private class Street
        {
            public string Name { get; set; }
            public string Number { get; set; }
        }

        private class Address
        {
            public Street Street { get; set; }
            public string PostCode { get; set; }
        }

        private class User
        {
            public string Name { get; set; }
            public Address Address { get; set; }
        }

        [Fact]
        public void Specification_From_ValidationContext()
        {
            var user = new User
            {
                Name = "Bart",
                Address = new Address
                {
                    Street = new Street
                    {
                        Name = "Pitt Street",
                        Number = "Fifth"
                    },
                    PostCode = "One-One-Two-Zero"
                }
            };

            Specification<Street> streetSpecification = s => s
                .Member(m => m.Name, m => m.NotWhiteSpace())
                .Member(m => m.Number, m => m
                    .Valid(p => p.Any(char.IsDigit)).WithMessage("Digit is required in street number")
                );

            Specification<Address> addressSpecification = s => s
                .Member(m => m.Street, m => m.AsModel())
                .Member(m => m.PostCode, m => m
                    .Valid(p => p.Any(char.IsDigit)).WithMessage("Digit is required in post code")
                );

            Specification<User> userSpecification = s => s
                .Member(m => m.Address, m => m.AsModel())
                .Member(m => m.Name, m => m
                    .Valid(p => p.Contains(' ')).WithMessage("Please provide both name and surname")
                );

            var validationContext = ValidationContext.Factory.Create(options => options
                .AddSpecification(userSpecification)
                .AddSpecification(addressSpecification)
                .AddSpecification(streetSpecification)
            );

            var result = validationContext.Validate(user);

            var expectedListReport = @"Address.Street.Number: Digit is required in street number
Address.PostCode: Digit is required in post code
Name: Please provide both name and surname
";

            var expectedModelReportJson = @"
{
  'Address': {
        'Street': {
            'Number': [
                'Digit is required in street number'
            ]
        },
        'PostCode': [
            'Digit is required in post code'
        ]
  },
  'Name': [
        'Please provide both name and surname'
  ]
}
";

            Assert.True(JToken.DeepEquals(JToken.FromObject(result.ToModelReport()), JToken.Parse(expectedModelReportJson)));
            Assert.Equal(expectedListReport, result.ToListReport().ToString());

            var validationContextWithMissingSpec = ValidationContext.Factory.Create(options => options
                .AddSpecification(userSpecification)
                .AddSpecification(addressSpecification)
            );

            Assert.Throws<SpecificationNotFoundException>(() => { validationContextWithMissingSpec.Validate(user); });
        }
    }
}