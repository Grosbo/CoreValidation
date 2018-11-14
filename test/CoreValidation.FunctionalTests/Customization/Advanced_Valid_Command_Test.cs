using CoreValidation.Specifications;
using Xunit;
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Customization
{
    public class Advanced_Valid_Command_Test
    {
        private class UserModel
        {
            public int Age { get; set; }
        }

        [Fact]
        public void Advanced_Valid_Command()
        {
            Specification<UserModel> specification = s => s
                .Member(m => m.Age, m => m.Valid(
                    age => (age >= 0) && (age <= 120),
                    "Age needs to be between {min} and {max}",
                    new[]
                    {
                        Arg.Number("min", 0),
                        Arg.Number("max", 120)
                    })
                );

            var model = new UserModel
            {
                Age = 130
            };

            var report = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(model)
                .ToListReport();

            var expectedListReport = @"Age: Age needs to be between 0 and 120
";

            Assert.Equal(expectedListReport, report.ToString());

            Specification<UserModel> specificationWithMessage = s => s
                .Member(m => m.Age, m => m.Valid(
                        age => (age >= 0) && (age <= 120),
                        "Age needs to be between {min} and {max}",
                        new[]
                        {
                            Arg.Number("min", 0),
                            Arg.Number("max", 120)
                        }).WithMessage("Range is from {min|format=0.00} to {max|format=0.00}")
                );

            var reportWithMessage = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specificationWithMessage)
                )
                .Validate(model)
                .ToListReport();

            var expectedListReportWithMessage = @"Age: Range is from 0.00 to 120.00
";

            Assert.Equal(expectedListReportWithMessage, reportWithMessage.ToString());
        }
    }
}