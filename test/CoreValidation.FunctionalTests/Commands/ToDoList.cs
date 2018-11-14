using System.Collections.Generic;
using System.Linq;
using CoreValidation.Specifications;
using Xunit;

namespace CoreValidation.FunctionalTests.Commands
{
    public class ToDoItems
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
        public void Should_PassScenario_ToDoItems_WithShourtcut()
        {
            Specification<ToDoItem> toDoItemSpecification = s => s
                .Member(m => m.Title, m => m.NotEmpty().MaxLength(10))
                .Member(m => m.Checked);

            var toDoList = new ToDoList()
            {
                Items = new[]
                {
                    new ToDoItem() {Checked = false},
                    new ToDoItem() {Title = "test2", Checked = false},
                    new ToDoItem() {Title = "test3"},
                    new ToDoItem() {Title = "long title test", Checked = false},
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


            Assert.Equal(reportFromFull.ToString(), reportFromShortcut.ToString());



        }
    }
}