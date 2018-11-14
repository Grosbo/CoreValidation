using CoreValidation.Specifications;
using Xunit;
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Commands.SetSingleError
{
    public class Order_Doesnt_Matter_Test
    {
        private class Customer
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }

        [Fact]
        public void Order_Doesnt_Matter()
        {
            Specification<Customer> specification1 = s => s
                .Member(m => m.Name, m => m
                    .NotWhiteSpace()
                    .MinLength(3)
                    .MaxLength(50)
                    .Valid(n => n.Contains(" ")).WithMessage("At least two words required")
                    .SetSingleError("Invalid name")
                )
                .Member(m => m.Email, m => m.Email());

            Specification<Customer> specification2 = s => s
                .Member(m => m.Name, m => m
                    .SetSingleError("Invalid name")
                    .NotWhiteSpace()
                    .MinLength(3)
                    .MaxLength(50)
                    .Valid(n => n.Contains(" ")).WithMessage("At least two words required")
                )
                .Member(m => m.Email, m => m.Email());

            Specification<Customer> specification3 = s => s
                .Member(m => m.Name, m => m
                    .NotWhiteSpace()
                    .MinLength(3)
                    .SetSingleError("Invalid name")
                    .MaxLength(50)
                    .Valid(n => n.Contains(" ")).WithMessage("At least two words required")
                )
                .Member(m => m.Email, m => m.Email());

            var data = new Customer
            {
                Name = "b",
                Email = "b@rt.com"
            };

            var report1 = ValidationContext.Factory.Create(options => options
                .AddSpecification(specification1)
            ).Validate(data).ToListReport();

            var report2 = ValidationContext.Factory.Create(options => options
                .AddSpecification(specification2)
            ).Validate(data).ToListReport();

            var report3 = ValidationContext.Factory.Create(options => options
                .AddSpecification(specification3)
            ).Validate(data).ToListReport();

            var expectedReport = @"Name: Invalid name
";

            Assert.Equal(expectedReport, report1.ToString());
            Assert.Equal(expectedReport, report2.ToString());
            Assert.Equal(expectedReport, report3.ToString());
        }
    }
}