using System;

namespace CoreValidation.Specifications.Commands
{
    public class SetRequiredCommand : ICommand
    {
        public SetRequiredCommand(string message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public string Message { get; }
        public string Name => "SetRequired";
    }
}