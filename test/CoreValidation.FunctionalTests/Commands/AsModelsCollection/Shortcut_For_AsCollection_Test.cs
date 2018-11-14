using System.Collections.Generic;
using CoreValidation.Specifications;
using Xunit;

namespace CoreValidation.FunctionalTests.Commands.AsModelsCollection
{
    // ReSharper disable once InconsistentNaming
    public class Shortcut_For_AsCollection_Test
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
        public void Shortcut_For_AsCollection()
        {
            Specification<ToDoItem> toDoItemSpecification = s => s
                .Member(m => m.Title, m => m.NotEmpty().MaxLength(10))
                .Member(m => m.Checked);

            var toDoList = new ToDoList
            {
                Items = new[]
                {
                    new ToDoItem {Checked = false},
                    new ToDoItem {Title = "test2", Checked = false},
                    new ToDoItem {Title = "test3"},
                    new ToDoItem {Title = "long title test", Checked = false}
                }
            };

            Specification<ToDoList> fullCollectionSpec = s => s
                .Member(m => m.Items, m => m.AsCollection(i => i.AsModel(toDoItemSpecification)));

            var reportFromFull = ValidationContext.Factory.Create(options => options
                    .AddSpecification(fullCollectionSpec)
                )
                .Validate(toDoList)
                .ToListReport();

            Specification<ToDoList> shortcutCollectionSpec = s => s
                .Member(m => m.Items, m => m.AsModelsCollection(toDoItemSpecification));

            var reportFromShortcut = ValidationContext.Factory.Create(options => options
                    .AddSpecification(shortcutCollectionSpec)
                )
                .Validate(toDoList)
                .ToListReport();

            var expectedReport = @"Items.0.Title: Required
Items.2.Checked: Required
Items.3.Title: Text value should have maximum 10 characters
";

            Assert.Equal(expectedReport, reportFromFull.ToString());

            Assert.Equal(reportFromFull.ToString(), reportFromShortcut.ToString());
        }
    }
}