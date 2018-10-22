using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Specifications.Commands;

namespace CoreValidation.Validators
{
    internal static class CommandsHelper
    {
        public static T GetClosestCommand<T>(IReadOnlyCollection<ICommand> commands, int currentIndex, IReadOnlyCollection<Type> skipTypes = null)
            where T : class, ICommand
        {
            var currentCommand = commands.ElementAt(currentIndex);

            for (var i = currentIndex - 1; i >= 0; --i)
            {
                var previousCommand = commands.ElementAt(i);

                if (previousCommand is T variable)
                {
                    return variable;
                }

                if ((skipTypes == null) || !skipTypes.Contains(previousCommand.GetType()))
                {
                    throw new InvalidCommandOrderException($"Command {currentCommand.Name} is invalid after command {previousCommand.Name}", currentIndex, currentCommand.Name);
                }
            }

            throw new InvalidCommandOrderException($"Command {currentCommand.Name} not found", -1, currentCommand.Name);
        }
    }
}