using System;

namespace CoreValidation.Specifications.Commands
{
    public class SetSingleErrorCommand : ICommand
    {
        public SetSingleErrorCommand(string message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public string Message { get; }
        public string Name => "SetSingleError";
    }
}