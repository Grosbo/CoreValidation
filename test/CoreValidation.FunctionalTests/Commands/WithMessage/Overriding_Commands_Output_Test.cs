using System.Collections.Generic;
using CoreValidation.Specifications;
using Xunit;

// ReSharper disable InconsistentNaming
namespace CoreValidation.FunctionalTests.Commands.WithMessage
{
    public class Overriding_Commands_Output_Test
    {
        private class ReceiverModel
        {
            public string Email { get; set; }
            public string Name { get; set; }
        }

        private class MessageModel
        {
            public ReceiverModel Receiver { get; set; }
            public IEnumerable<ReceiverModel> Cc { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public int? Priority { get; set; }
        }

        [Fact]
        public void Overriding_Commands_Output()
        {
            var messageModel = new MessageModel
            {
                Receiver = new ReceiverModel
                {
                    Email = "b@rt_invalid@email.com",
                    Name = "Bart"
                },
                Cc = new[]
                {
                    new ReceiverModel
                    {
                        Email = "valid@email.com",
                        Name = "Guest1"
                    },
                    new ReceiverModel
                    {
                        Email = "invalid@email@com",
                        Name = "Guest2"
                    }
                },
                Title = null,
                Content = "",
                Priority = -1
            };

            Specification<ReceiverModel> receiverSpecificaiton = s => s
                .Member(m => m.Email, m => m
                    .Email()
                    .MaxLength(100)
                )
                .Member(m => m.Name, m => m
                    .SetOptional()
                    .NotWhiteSpace()
                    .MaxLength(1000)
                );

            Specification<MessageModel> messageSpecificationRaw = s => s
                .Member(m => m.Receiver, m => m
                    .AsModel(receiverSpecificaiton)
                )
                .Member(m => m.Cc, m => m
                    .SetOptional()
                    .AsModelsCollection(receiverSpecificaiton)
                )
                .Member(m => m.Title, m => m
                    .MaxLength(10)
                )
                .Member(m => m.Content, m => m
                    .NotWhiteSpace()
                    .MinLength(5)
                    .MaxLength(100)
                )
                .Member(m => m.Priority, m => m
                    .AsNullable(p => p.GreaterOrEqualTo(0))
                );

            var reportRaw = ValidationContext.Factory.Create(options => options
                    .AddSpecification(messageSpecificationRaw)
                )
                .Validate(messageModel)
                .ToListReport();

            var expectedListReportRaw = @"Receiver.Email: Text value should be a valid email
Cc.1.Email: Text value should be a valid email
Title: Required
Content: Text value cannot be whitespace
Content: Text value should have minimum 5 characters
Priority: Number should be greater than (or equal to) 0
";

            Assert.Equal(expectedListReportRaw, reportRaw.ToString());

            Specification<MessageModel> messageSpecification = s => s
                .Member(m => m.Receiver, m => m
                    .AsModel(receiverSpecificaiton).WithMessage("Invalid receiver")
                )
                .Member(m => m.Cc, m => m
                    .SetOptional()
                    .AsModelsCollection(receiverSpecificaiton).WithMessage("Invalid receiver in CC")
                )
                .Member(m => m.Title, m => m
                    .MaxLength(10)
                ).WithMessage("Invalid title")
                .Member(m => m.Content, m => m
                    .NotWhiteSpace()
                    .MinLength(5)
                    .MaxLength(100)
                ).WithMessage("Invalid content")
                .Member(m => m.Priority, m => m
                    .AsNullable(p => p.GreaterOrEqualTo(0))
                ).WithMessage("Invalid priority");

            var report = ValidationContext.Factory.Create(options => options
                    .AddSpecification(messageSpecification)
                )
                .Validate(messageModel)
                .ToListReport();

            var expectedListReport = @"Receiver: Invalid receiver
Cc: Invalid receiver in CC
Title: Invalid title
Content: Invalid content
Priority: Invalid priority
";
            Assert.Equal(expectedListReport, report.ToString());
        }
    }
}