using System.Linq;
using CoreValidation.Specifications;
using Newtonsoft.Json.Linq;
using Xunit;

namespace CoreValidation.FunctionalTests.Commands.AsModel
{
    // ReSharper disable once InconsistentNaming
    public class Inline_Specification_Test
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
        public void Inline_Specification()
        {
            Specification<Street> streetSpecification = s => s
                .Member(m => m.Name, m => m.NotWhiteSpace())
                .Member(m => m.Number, m => m
                    .Valid(p => p.Any(char.IsDigit)).WithMessage("Digit is required in street number")
                );

            Specification<Address> addressSpecification = s => s
                .Member(m => m.Street, m => m.AsModel(streetSpecification))
                .Member(m => m.PostCode, m => m
                    .Valid(p => p.Any(char.IsDigit)).WithMessage("Digit is required in post code")
                );

            Specification<User> userSpecification = s => s
                .Member(m => m.Address, m => m.AsModel(addressSpecification))
                .Member(m => m.Name, m => m
                    .Valid(p => p.Contains(' ')).WithMessage("Please provide both name and surname")
                );

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

            var validationContext = ValidationContext.Factory.Create(options => options
                .AddSpecification(userSpecification)
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
        }
    }
}