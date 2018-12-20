using CoreValidation.Specifications;
using Xunit;
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Commands.SetSingleError
{
    public class Overriding_All_Member_Errors_Test
    {
        private class Customer
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }

        [Fact]
        public void Overriding_All_Member_Errors()
        {
            Specification<Customer> customerSpecification = s => s
                .Member(m => m.Name, m => m
                    .NotWhiteSpace()
                    .MinLength(4)
                    .MaxLength(50)
                    .Valid(n => n.Contains(" ")).WithMessage("At least two words required")
                    .SetSingleError("Invalid name")
                )
                .Member(m => m.Email, m => m.Email());

            var validationContext = ValidationContext.Factory.Create(options => options
                .AddSpecification(customerSpecification)
            );

            var customer1 = new Customer
            {
                Name = "Bart",
                Email = "b@rt.com"
            };

            var customer2 = new Customer
            {
                Name = "B R",
                Email = "b@rt.com"
            };

            var customer3 = new Customer
            {
                Name = "   ",
                Email = "b@rt.com"
            };

            var customer4 = new Customer
            {
                Name = "b",
                Email = "b@rt.com"
            };

            var report1 = validationContext.Validate(customer1).ToListReport();
            var report2 = validationContext.Validate(customer2).ToListReport();
            var report3 = validationContext.Validate(customer3).ToListReport();
            var report4 = validationContext.Validate(customer4).ToListReport();

            var expectedReport = @"Name: Invalid name
";

            Assert.Equal(expectedReport, report1.ToString());
            Assert.Equal(expectedReport, report2.ToString());
            Assert.Equal(expectedReport, report3.ToString());
            Assert.Equal(expectedReport, report4.ToString());
        }
    }
}