using System;
using CoreValidation.Specifications;
using Xunit;
// ReSharper disable InconsistentNaming

namespace CoreValidation.FunctionalTests.Commands.AsRelative
{
    public class Null_Relatives_In_Predicate_Test
    {
        private class SignUpModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string PasswordConfirmation { get; set; }
        }

        [Fact]
        public void Null_Relatives_In_Predicate()
        {
            Specification<SignUpModel> specification = s => s
                .Member(m => m.Email, m => m.Email())
                .Member(m => m.Password, m => m
                    .MinLength(6)
                    .AsRelative(p => !p.Password.Contains(p.Email))
                )
                .Member(m => m.PasswordConfirmation, m => m
                    .AsRelative(p => p.Password == p.PasswordConfirmation).WithMessage("Confirmation not same as the password")
                );

            var signUpModel = new SignUpModel
            {
                Email = null,
                Password = "p@ssword1",
                PasswordConfirmation = "P@SSWORD!"
            };

            var validationContext = ValidationContext.Factory.Create(options => options
                .AddSpecification(specification)
            );

            Assert.Throws<ArgumentNullException>(() => { validationContext.Validate(signUpModel); });
        }
    }
}