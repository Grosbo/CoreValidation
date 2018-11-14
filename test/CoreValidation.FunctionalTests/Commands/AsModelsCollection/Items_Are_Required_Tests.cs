using System.Collections.Generic;
using CoreValidation.Specifications;
using Xunit;

namespace CoreValidation.FunctionalTests.Commands.AsModelsCollection
{
    // ReSharper disable once InconsistentNaming
    public class Items_Required_Tests
    {
        private class ToDoItem
        {
            public string Title { get; set; }
            public bool? Checked { get; set; }
        }

        private class ToDoList
        {
            public IEnumerable<ToDoItem> Items { get; set; }
        }

        [Fact]
        public void Items_Are_Required()
        {
            Specification<ToDoItem> toDoItemSpecification = s => s
                .Member(m => m.Title, m => m.NotEmpty().MaxLength(10))
                .Member(m => m.Checked);

            var toDoList = new ToDoList
            {
                Items = new[]
                {
                    new ToDoItem {Checked = false},
                    null,
                    new ToDoItem {Title = "test3"},
                    null
                }
            };

            Specification<ToDoList> specification = s => s
                .Member(m => m.Items, m => m.AsCollection(i => i.AsModel(toDoItemSpecification)));

            var listReport = ValidationContext.Factory.Create(options => options
                    .AddSpecification(specification)
                )
                .Validate(toDoList)
                .ToListReport();

            var expectedReport = @"Items.0.Title: Required
Items.1: Required
Items.2.Checked: Required
Items.3: Required
";

            Assert.Equal(expectedReport, listReport.ToString());
        }
    }
}