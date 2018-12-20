using CoreValidation.Specifications;
using Xunit;
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Commands.SetSingleError
{
    public class Overriding_All_Model_Errors_Test
    {
        private class Address
        {
            public string Street { get; set; }
            public string PostCode { get; set; }
        }

        private class Customer
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public Address BillingAddress { get; set; }
        }

        [Fact]
        public void Overriding_All_Model_Errors()
        {
            Specification<Address> addressSpecification = s => s
                .Member(m => m.Street)
                .Member(m => m.PostCode);

            Specification<Customer> customerSpecification = s => s
                .Member(m => m.Name, m => m.NotWhiteSpace())
                .Member(m => m.Email, m => m.Email())
                .Member(m => m.BillingAddress, m => m.AsModel(addressSpecification))
                .Valid(m => (m.Name != null) && (m.Email != null)).WithMessage("Both name and email are missing")
                .SetSingleError("This customer is invalid");

            var validationContext = ValidationContext.Factory.Create(options => options
                .AddSpecification(customerSpecification)
            );

            var customer1 = new Customer
            {
                Name = "Bart",
                Email = "b@rt.com",
                BillingAddress = new Address
                {
                    Street = "Lincoln Ave"
                }
            };

            var customer2 = new Customer
            {
                Name = "Bart",
                Email = "invalid@email@com",
                BillingAddress = new Address
                {
                    Street = "Lincoln Ave",
                    PostCode = "123-456"
                }
            };

            var customer3 = new Customer
            {
                Name = "Bart",
                Email = "b@rt.com"
            };

            var customer4 = new Customer
            {
                BillingAddress = new Address
                {
                    Street = "Lincoln Ave",
                    PostCode = "123-456"
                }
            };

            var report1 = validationContext.Validate(customer1).ToListReport();
            var report2 = validationContext.Validate(customer2).ToListReport();
            var report3 = validationContext.Validate(customer3).ToListReport();
            var report4 = validationContext.Validate(customer4).ToListReport();

            var expectedReport = @"This customer is invalid
";

            Assert.Equal(expectedReport, report1.ToString());
            Assert.Equal(expectedReport, report2.ToString());
            Assert.Equal(expectedReport, report3.ToString());
            Assert.Equal(expectedReport, report4.ToString());
        }
    }
}