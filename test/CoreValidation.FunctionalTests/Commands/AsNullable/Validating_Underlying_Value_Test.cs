using CoreValidation.Specifications;
using Xunit;
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleInvalidOperationException

namespace CoreValidation.FunctionalTests.Commands.AsNullable
{
    public class Validating_Underlying_Value_Test
    {
        private class UserRecord
        {
            public int UserId { get; set; }
            public int Age { get; set; }
            public int? AgeWhenGraduated { get; set; }
        }

        [Fact]
        public void Validating_Underlying_Value()
        {
            Specification<UserRecord> specification = s => s
                .Member(m => m.UserId, m => m.GreaterOrEqualTo(0))
                .Member(m => m.Age, m => m.BetweenOrEqualTo(0, 130))
                .Member(m => m.AgeWhenGraduated, m => m
                    .AsNullable(n => n
                        .BetweenOrEqualTo(0, 130)
                        .AsRelative(p => p.AgeWhenGraduated.Value <= p.Age).WithMessage("Cannot be greater than Age")
                    )
                );

            var signUpModel = new UserRecord
            {
                UserId = 0,
                Age = 100,
                AgeWhenGraduated = 200
            };

            var report = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(signUpModel)
                .ToListReport();

            var expectedListReport = @"AgeWhenGraduated: Number should be between 0 and 130 (inclusive)
AgeWhenGraduated: Cannot be greater than Age
";

            Assert.Equal(expectedListReport, report.ToString());
        }
    }
}