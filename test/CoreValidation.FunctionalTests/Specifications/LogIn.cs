using System;
using System.Linq;
using CoreValidation.Results;
using CoreValidation.Specifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable ArgumentsStyleLiteral

namespace CoreValidation.FunctionalTests.Specifications
{
    public class LogIn
    {
        private class LogInModel
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
        }

        private static Specification<LogInModel> logInModelSpecification = s => s
            .Member(m => m.Email, m => m
                .SetOptional()
                .Email())
            .Member(m => m.Name, m => m
                .SetOptional()
                .NotEmpty()
                .MaxLength(20))
            .Member(m => m.Password)
            .Valid(m => (m.Email != null) || (m.Name != null)).WithMessage("Either email or name is required");

        private static IValidationContext validationContext = ValidationContext.Factory.Create(options => options
            .AddSpecification(logInModelSpecification)
        );

        [Fact]
        public void Should_PassScenario_LogIn_With_EmailOnly()
        {
            var json = @"{
                'Email': 'some_inv@lid_Em@il'
            }";

            var model = JsonConvert.DeserializeObject<LogInModel>(json);

            var validationResult = validationContext.Validate(model);

            Assert.False(validationResult.IsValid);

            var listReport = validationResult.ToListReport();

            var expectedListReport =
                @"Email: Text value should be a valid email
Password: Required
";

            Assert.Equal(expectedListReport, listReport.ToString());

            var modelReport = validationResult.ToModelReport();

            var expectedReportJson = @"{
            'Email': ['Text value should be a valid email'],
            'Password': ['Required'],
}";

            Assert.NotNull(modelReport);
            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedReportJson)));
        }


        [Fact]
        public void Should_PassScenario_LogIn_With_PasswordOnly()
        {
            var json = @"{
                'Password': 'p@ssw0rd'
            }";

            var model = JsonConvert.DeserializeObject<LogInModel>(json);

            var validationResult = validationContext.Validate(model);

            Assert.False(validationResult.IsValid);

            var listReport = validationResult.ToListReport();

            var expectedListReport =
                @"Either email or name is required
";

            Assert.Equal(expectedListReport, listReport.ToString());

            var modelReport = validationResult.ToModelReport();

            var expectedReportJson = @"['Either email or name is required']";

            Assert.NotNull(modelReport);
            Assert.True(JToken.DeepEquals(JToken.FromObject(modelReport), JToken.Parse(expectedReportJson)));
        }
    }
}