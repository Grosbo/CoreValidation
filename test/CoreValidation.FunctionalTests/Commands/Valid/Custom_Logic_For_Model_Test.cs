using CoreValidation.Specifications;
using Xunit;
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Commands.Valid
{
    public class Custom_Logic_For_Model_Test
    {
        private class SignUpModel
        {
            public string Name { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Custom_Logic_For_Model()
        {
            Specification<SignUpModel> specification = s => s
                .Valid(m =>
                {
                    return (m.Name != null) &&
                           (m.Password != null) &&
                           !m.Password.Contains(m.Name);
                });

            var model = new SignUpModel
            {
                Name = "John",
                Password = "John123"
            };

            var report = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(model)
                .ToListReport();

            var expectedListReport = @"Invalid
";

            Assert.Equal(expectedListReport, report.ToString());
        }
    }
}