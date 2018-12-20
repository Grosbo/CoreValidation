using System.Collections.Generic;
using CoreValidation.Specifications;
using Xunit;
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Customization
{
    internal static class Making_The_Rule_Translatable_Test_Extension
    {
        public static IMemberSpecificationBuilder<TModel, int> InRangeInclusive<TModel>(this IMemberSpecificationBuilder<TModel, int> @this, int min, int max)
            where TModel : class
        {
            return @this.Valid(
                m => (m >= min) && (m <= max),
                "Namespace.InRangeInclusive",
                new[]
                {
                    Arg.Number(nameof(min), min),
                    Arg.Number(nameof(max), max)
                });
        }
    }


    public class Making_The_Rule_Translatable_Test
    {
        private class UserModel
        {
            public int Age { get; set; }
        }


        [Fact]
        public void Making_The_Rule_Translatable()
        {
            Specification<UserModel> specification = s => s
                .Member(m => m.Age, m => m
                    .InRangeInclusive(0, 120)
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

            var expectedListReport = @"Age: Namespace.InRangeInclusive
";

            Assert.Equal(expectedListReport, report.ToString());

            var validationContext = ValidationContext.Factory.Create(options => options
                .AddSpecification(specification)
                .IncludeInEnglishTranslation(new Dictionary<string, string>
                {
                    {"Namespace.InRangeInclusive", "Value needs to be between {min} and {max} (inclusive)"}
                })
                .AddPolishTranslation()
                .IncludeInPolishTranslation(new Dictionary<string, string>
                {
                    {"Namespace.InRangeInclusive", "Wartość powinna być pomiędzy {min|culture=pl-PL} a {max|culture=pl-PL} (włącznie)"}
                })
            );

            var result = validationContext.Validate(model);

            var englishReport = result.ToListReport("English");

            var expectedEnglishReport = @"Age: Value needs to be between 0 and 120 (inclusive)
";

            Assert.Equal(expectedEnglishReport, englishReport.ToString());

            var polishReport = result.ToListReport(nameof(Phrases.Polish));

            var expectedPolishReport = @"Age: Wartość powinna być pomiędzy 0 a 120 (włącznie)
";

            Assert.Equal(expectedPolishReport, polishReport.ToString());
        }
    }
}