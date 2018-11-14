using CoreValidation.Specifications;
using Xunit;

// ReSharper disable InconsistentNaming
namespace CoreValidation.FunctionalTests.Commands.WithMessage
{
    public class Valid_Custom_Message_Test
    {
        private class MessageModel
        {
            public string Content { get; set; }
        }

        [Fact]
        public void Valid_Custom_Message()
        {
            var messageModel = new MessageModel
            {
                Content = "This is a very long message content that would certainly fail the validation."
            };

            Specification<MessageModel> specification = s => s
                .Member(m => m.Content, m => m
                    .Valid(c => c.Length < 50).WithMessage("The message content is too long (max 50 characters)")
                );

            var report2 = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(messageModel)
                .ToListReport();

            var expectedListReport2 = @"Content: The message content is too long (max 50 characters)
";
            Assert.Equal(expectedListReport2, report2.ToString());
        }
    }
}