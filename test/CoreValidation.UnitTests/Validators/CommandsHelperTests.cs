using System.Linq;
using CoreValidation.Specifications.Commands;
using CoreValidation.Validators;
using Xunit;

namespace CoreValidation.UnitTests.Validators
{
    public class CommandsHelperTests
    {
        public class CommandA : ICommand
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public string Name { get; }
        }

        public class CommandB : ICommand
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public string Name { get; }
        }

        public class CommandNeutral : ICommand
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public string Name { get; }
        }

        public class CommandNeutral2 : ICommand
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public string Name { get; }
        }

        [Fact]
        public void Should_GetClosestCommand()
        {
            var commands = new ICommand[]
            {
                new CommandA(),
                new CommandB()
            };

            var closest = CommandsHelper.GetClosestCommand<CommandA>(commands, 1);

            Assert.Same(commands.ElementAt(0), closest);
        }

        [Fact]
        public void Should_GetClosestCommand_ThrowException_When_DifferentSkipTypes()
        {
            var commands = new ICommand[]
            {
                new CommandA(),
                new CommandNeutral(),
                new CommandNeutral(),
                new CommandNeutral(),
                new CommandB()
            };

            Assert.Throws<InvalidCommandOrderException>(() =>
            {
                CommandsHelper.GetClosestCommand<CommandA>(commands, 4, new[]
                {
                    typeof(CommandNeutral2)
                });
            });
        }

        [Fact]
        public void Should_GetClosestCommand_ThrowException_When_WithoutSkipTypes()
        {
            var commands = new ICommand[]
            {
                new CommandA(),
                new CommandNeutral(),
                new CommandNeutral(),
                new CommandNeutral(),
                new CommandB()
            };

            Assert.Throws<InvalidCommandOrderException>(() => { CommandsHelper.GetClosestCommand<CommandA>(commands, 4); });
        }

        [Fact]
        public void Should_GetClosestCommand_ThrowException_WhenNoTargetCommand()
        {
            var commands = new ICommand[]
            {
                new CommandNeutral(),
                new CommandNeutral(),
                new CommandNeutral(),
                new CommandB()
            };

            Assert.Throws<InvalidCommandOrderException>(() => { CommandsHelper.GetClosestCommand<CommandA>(commands, 3, new[] {typeof(CommandNeutral)}); });
        }

        [Fact]
        public void Should_GetClosestCommand_WithSkipTypes()
        {
            var commands = new ICommand[]
            {
                new CommandA(),
                new CommandNeutral(),
                new CommandNeutral(),
                new CommandNeutral(),
                new CommandB()
            };

            var closest = CommandsHelper.GetClosestCommand<CommandA>(commands, 4, new[] {typeof(CommandNeutral)});

            Assert.Same(commands.ElementAt(0), closest);
        }
    }
}