using System.Linq;
using CoreValidation.Specifications;
using Newtonsoft.Json.Linq;
using Xunit;

namespace CoreValidation.FunctionalTests.Specifications
{
    // ReSharper disable once InconsistentNaming
    public class Member_Scope_Test
    {
        private class LogInModel
        {
            public string Email { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void Member_Scope()
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
                Name = "   ",
                Email = "inv@lidem@il.com",
                Password = null
            };

            var validationContext = ValidationContext.Factory.Create(options => options
                .AddSpecification(logInSpecification)
            );

            var result = validationContext.Validate(model);

            Assert.Equal(3, result.ErrorsCollection.Members.Count);
            Assert.True(result.ErrorsCollection.Members.ContainsKey("Email"));
            Assert.True(result.ErrorsCollection.Members.ContainsKey("Name"));
            Assert.True(result.ErrorsCollection.Members.ContainsKey("Password"));

            var expectedListReport = @"Email: Text value should be a valid email
Name: Text value cannot be whitespace
Name: Only letters are accepted
Password: Required
";

            Assert.Equal(expectedListReport, result.ToListReport().ToString());

            var expectedModelReportJson = @"
{
  'Email': [
    'Text value should be a valid email'
  ],
  'Name': [
    'Text value cannot be whitespace',
    'Only letters are accepted'
  ],
  'Password': [
    'Required'
  ]
}
";
            Assert.True(JToken.DeepEquals(JToken.FromObject(result.ToModelReport()), JToken.Parse(expectedModelReportJson)));
        }
    }
}