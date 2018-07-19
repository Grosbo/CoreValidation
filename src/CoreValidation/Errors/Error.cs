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

        public static void AssertValidMessageAndArgs(string message, IReadOnlyCollection<IMessageArg> args)
        {
            if ((message == null) && (args != null))
            {
                throw new ArgumentException($"Defining {nameof(args)} not allowed if {nameof(message)} is null");
            }
        }

        public static Error CreateOrDefault(string message, IReadOnlyCollection<IMessageArg> args, Error defaultError)
        {
            AssertValidMessageAndArgs(message, args);

            if ((message == null) && (defaultError == null))
            {
                throw new ArgumentException($"Either {nameof(message)} or {nameof(defaultError)} must not be null.");
            }

            return message == null ? new Error(defaultError.Message, defaultError.Arguments) : new Error(message, args);
        }
    }
}