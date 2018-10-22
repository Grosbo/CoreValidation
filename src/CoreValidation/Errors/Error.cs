using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors.Args;

namespace CoreValidation.Errors
{
    internal sealed class Error : IError
    {
        private string _message;

        public Error(string message, IReadOnlyCollection<IMessageArg> args = null)
        {
            if ((args != null) && args.Contains(null))
            {
                throw new ArgumentException($"Null argument in {nameof(args)}");
            }

            Arguments = args;
            Message = message;
        }

        public string Message
        {
            get => _message;
            set => _message = value ?? throw new ArgumentNullException(nameof(value));
        }

        public IReadOnlyCollection<IMessageArg> Arguments { get; }

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