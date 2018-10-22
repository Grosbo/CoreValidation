using System;

namespace CoreValidation.Specifications.Commands
{
    internal class WithMessageCommand : ICommand
    {
        public WithMessageCommand(string message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public string Message { get; }
        public string Name => "WithMessage";
    }
}