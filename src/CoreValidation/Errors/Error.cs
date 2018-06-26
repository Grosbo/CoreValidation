using System;
using System.Collections.Generic;

namespace CoreValidation.Errors
{
    public sealed class Error : IErrorMessageHolder
    {
        private static readonly MessageStringifier _messageStringifier = new MessageStringifier();

        public Error(string message, IReadOnlyCollection<IMessageArg> args = null)
            : this(message, args, _messageStringifier.Stringify(message, args))
        {
        }

        private Error(string message, IReadOnlyCollection<IMessageArg> args, string stringifiedMessage)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Arguments = args;
            StringifiedMessage = stringifiedMessage;
        }

        public string StringifiedMessage { get; }

        public string Message { get; }

        public IReadOnlyCollection<IMessageArg> Arguments { get; }

        public bool EqualContent(Error other)
        {
            return string.Equals(StringifiedMessage, other?.StringifiedMessage, StringComparison.Ordinal);
        }

        public Error Clone()
        {
            return new Error(Message, Arguments, StringifiedMessage);
        }
    }
}