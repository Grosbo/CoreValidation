using CoreValidation.Specifications;
using Xunit;
// ReSharper disable InconsistentNaming
// ReSharper disable PossibleInvalidOperationException

namespace CoreValidation.FunctionalTests.Commands.AsNullable
{
    public class Nullable_Without_Value_Test
    {
        private class UserRecord
        {
            public int UserId { get; set; }
            public int Age { get; set; }
            public int? AgeWhenGraduated { get; set; }
        }

        [Fact]
        public void Nullable_Without_Value()
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

            Specification<UserRecord> specificationWithOptional = s => s
                .Member(m => m.UserId, m => m.GreaterOrEqualTo(0))
                .Member(m => m.Age, m => m.BetweenOrEqualTo(0, 130))
                .Member(m => m.AgeWhenGraduated, m => m
                    .SetOptional()
                    .AsNullable(n => n
                        .BetweenOrEqualTo(0, 130)
                        .AsRelative(p => p.AgeWhenGraduated.Value <= p.Age).WithMessage("Cannot be greater than Age")
                    )
                );

            var signUpModel = new UserRecord
            {
                UserId = 0,
                Age = 100,
                AgeWhenGraduated = null
            };

            var report = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(signUpModel)
                .ToListReport();

            var listReport = @"AgeWhenGraduated: Required
";

            Assert.Equal(listReport, report.ToString());

            var resultWithOptional = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specificationWithOptional)
                )
                .Validate(signUpModel);

            Assert.True(resultWithOptional.IsValid);
        }
    }
}