using System.Linq;
using CoreValidation.Specifications;
using Xunit;

namespace CoreValidation.FunctionalTests.Specifications
{
    // ReSharper disable once InconsistentNaming
    public class Validation_Order_Test
    {
        private class LogInModel
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Validation_Order()
        {
            Specification<LogInModel> logInSpecification = s => s
                .Member(m => m.Email, m => m
                    .SetOptional()
                    .Email()
                )
                .Member(m => m.Name, m => m
                    .SetOptional()
                    .NotWhiteSpace()
                    .MaxLength(20)
                    .Valid(n => n.All(char.IsLetter)).WithMessage("Only letters are accepted")
                )
                .Member(m => m.Password)
                .Valid(m => (m.Email != null) || (m.Name != null)).WithMessage("Either email or name is required");

            var model = new LogInModel
            {
                Email = "invalid@email@com",
                Name = "this_name_is_too_long_to_be_valid",
                Password = null
            };

            var listReport = ValidationContext.Factory.Create(options => options
                .AddSpecification(logInSpecification)
            ).Validate(model).ToListReport();

            var expectedListReport = @"Email: Text value should be a valid email
Name: Text value should have maximum 20 characters
Name: Only letters are accepted
Password: Required
";

            Assert.Equal(expectedListReport, listReport.ToString());

            Specification<LogInModel> logInSpecificationReordered = s => s
                .Member(m => m.Name, m => m
                    .SetOptional()
                    .NotWhiteSpace()
                    .Valid(n => n.All(char.IsLetter)).WithMessage("Only letters are accepted")
                    .MaxLength(20)
                )
                .Member(m => m.Password)
                .Member(m => m.Email, m => m
                    .SetOptional()
                    .Email()
                )
                .Valid(m => (m.Email != null) || (m.Name != null)).WithMessage("Either email or name is required");

            var expectedListReportReordered = @"Name: Only letters are accepted
Name: Text value should have maximum 20 characters
Password: Required
Email: Text value should be a valid email
";

            var listReportReordered = ValidationContext.Factory.Create(options => options
                .AddSpecification(logInSpecificationReordered)
            ).Validate(model).ToListReport();

            Assert.Equal(expectedListReportReordered, listReportReordered.ToString());
        }
    }
}