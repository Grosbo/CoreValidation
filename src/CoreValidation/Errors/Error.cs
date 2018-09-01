using System;
using System.Collections.Generic;
using CoreValidation.Errors.Args;

namespace CoreValidation.Errors
{
    public sealed class Error
    {
        public Error(string message, IReadOnlyCollection<IMessageArg> args = null)
            : this(message, args, MessageFormatter.Format(message, args))
        {
        }


        private Error(string message, IReadOnlyCollection<IMessageArg> args, string formattedMessage)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Arguments = args;
            FormattedMessage = formattedMessage;
        }

        public string FormattedMessage { get; }

        public string Message { get; }

        public IReadOnlyCollection<IMessageArg> Arguments { get; }

        public bool EqualContent(Error other)
        {
            return string.Equals(FormattedMessage, other?.FormattedMessage, StringComparison.Ordinal);
        }

        public static Error CreateValidOrNull(string message, IReadOnlyCollection<IMessageArg> args)
        {
            if ((message == null) && (args == null))
            {
                return null;
            }

            if ((message == null) && (args != null))
            {
                throw new ArgumentException($"Defining {nameof(args)} not allowed if {nameof(message)} is null");
            }

            return new Error(message, args);
        }
    }
}