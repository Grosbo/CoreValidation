using CoreValidation.Specifications;
using CoreValidation.Validators;
using Xunit;
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Commands.SetSingleError
{
    public class One_Command_Per_Scope_Test
    {
        private class Customer
        {
            public string Name { get; set; }
            public string Email { get; set; }
        }

        [Fact]
        public void One_Command_Per_Scope()
        {
            var data = new Customer
            {
                Name = "b",
                Email = "b@rt.com"
            };

            Specification<Customer> specification1 = s => s
                .Member(m => m.Name, m => m
                    .SetSingleError("Invalid name")
                    .NotWhiteSpace()
                    .MinLength(3)
                    .MaxLength(50)
                    .Valid(n => n.Contains(" ")).WithMessage("At least two words required")
                    .SetSingleError("Invalid name (duplicate)")
                )
                .Member(m => m.Email, m => m.Email());

            Specification<Customer> specification2 = s => s
                .Member(m => m.Name, m => m
                    .NotWhiteSpace()
                    .MinLength(3)
                    .SetSingleError("Invalid name")
                    .SetSingleError("Invalid name (duplicate)")
                    .MaxLength(50)
                    .Valid(n => n.Contains(" ")).WithMessage("At least two words required")
                )
                .Member(m => m.Email, m => m.Email());

            Assert.Throws<InvalidCommandDuplicationException>(() =>
            {
                ValidationContext.Factory
                    .Create(options => options.AddSpecification(specification1))
                    .Validate(data);
            });

            Assert.Throws<InvalidCommandDuplicationException>(() =>
            {
                ValidationContext.Factory
                    .Create(options => options.AddSpecification(specification2))
                    .Validate(data);
            });
        }
    }
}