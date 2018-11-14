using CoreValidation.Specifications;
using Xunit;
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Customization
{
    internal static class Creating_FluentApi_Extension_Test_Extension
    {
        public static IMemberSpecificationBuilder<TModel, int> InRange<TModel>(this IMemberSpecificationBuilder<TModel, int> @this, int min, int max)
            where TModel : class
        {
            return @this.Valid(
                m => (m > min) && (m < max),
                "Value needs to be between {min} and {max}",
                new[]
                {
                    Arg.Number(nameof(min), min),
                    Arg.Number(nameof(max), max)
                });
        }
    }


    public class Creating_FluentApi_Extension_Test
    {
        private class UserModel
        {
            public int Age { get; set; }
        }


        [Fact]
        public void Creating_FluentApi_Extension()
        {
            Specification<UserModel> specification = s => s
                .Member(m => m.Age, m => m
                    .InRange(0, 120)
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

            var expectedListReport = @"Age: Value needs to be between 0 and 120
";

            Assert.Equal(expectedListReport, report.ToString());

            Specification<UserModel> specificationWithMessage = s => s
                .Member(m => m.Age, m => m
                    .InRange(0, 120).WithMessage("Range is from {min|format=0.00} to {max|format=0.00}")
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