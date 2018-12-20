using CoreValidation.Specifications;
using CoreValidation.Validators;
using Xunit;
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Commands.SetOptional
{
    public class One_Command_Per_Scope_Test
    {
        private class Credentials
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [Fact]
        public void One_Command_Per_Scope()
        {
            var credentials = new Credentials
            {
                Email = "b@rt.com"
            };

            Specification<Credentials> specification1 = s => s
                .Member(m => m.Username, m => m
                    .SetOptional()
                    .NotWhiteSpace()
                    .SetOptional()
                )
                .Member(m => m.Email, m => m.Email())
                .Member(m => m.Password);

            Specification<Credentials> specification2 = s => s
                .Member(m => m.Username, m => m
                    .SetOptional()
                    .SetOptional()
                    .NotWhiteSpace()
                )
                .Member(m => m.Email, m => m.Email())
                .Member(m => m.Password);

            Assert.Throws<InvalidCommandDuplicationException>(() =>
            {
                ValidationContext.Factory
                    .Create(options => options.AddSpecification(specification1))
                    .Validate(credentials);
            });

            Assert.Throws<InvalidCommandDuplicationException>(() =>
            {
                ValidationContext.Factory
                    .Create(options => options.AddSpecification(specification2))
                    .Validate(credentials);
            });
        }
    }
}