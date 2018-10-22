using System;

namespace CoreValidation.Validators
{
    public class CommandException : InvalidOperationException
    {
        public CommandException(string message, int order, string name)
            : base(message)
        {
            Order = order;
            Name = name;
        }

        public int Order { get; }

        public string Name { get; }
    }
}