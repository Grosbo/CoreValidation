using System.Linq;
using CoreValidation.Specifications;
using Newtonsoft.Json.Linq;
using Xunit;

namespace CoreValidation.FunctionalTests.Specifications
{
    // ReSharper disable once InconsistentNaming
    public class Model_Scope_Test
    {
        private class LogInModel
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Model_Scope_Order()
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
                Name = null,
                Email = null,
                Password = null
            };

            var validationContext = ValidationContext.Factory.Create(options => options
                .AddSpecification(logInSpecification)
            );

            var result = validationContext.Validate(model);

            Assert.Equal("Either email or name is required", result.ErrorsCollection.Errors.Single().Message);

            var expectedListReport = @"Either email or name is required
Password: Required
";

            Assert.Equal(expectedListReport, result.ToListReport().ToString());

            var expectedModelReportJson = @"
{
  'Password': [
    'Required'
  ],
  '': [
    'Either email or name is required'
  ]
}
";
            Assert.True(JToken.DeepEquals(JToken.FromObject(result.ToModelReport()), JToken.Parse(expectedModelReportJson)));
        }
    }
}