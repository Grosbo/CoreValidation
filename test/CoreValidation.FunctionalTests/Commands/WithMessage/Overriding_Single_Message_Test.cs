using CoreValidation.Specifications;
using Xunit;

// ReSharper disable InconsistentNaming
namespace CoreValidation.FunctionalTests.Commands.WithMessage
{
    public class Overriding_Single_Message_Test
    {
        private class MessageModel
        {
            public int? Priority { get; set; }
        }

        [Fact]
        public void Single_Message_Overriden()
        {
            var messageModel = new MessageModel
            {
                Priority = -1
            };

            Specification<MessageModel> specification = s => s
                .Member(m => m.Priority, m => m
                    .AsNullable(p => p.GreaterOrEqualTo(0)).WithMessage("Priority cannot be below {min}")
                );

            var report = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(messageModel)
                .ToListReport();

            var expectedListReport2 = @"Priority: Priority cannot be below 0
";
            Assert.Equal(expectedListReport2, report.ToString());
        }
    }
}