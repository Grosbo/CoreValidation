using System;

namespace CoreValidation.Exceptions
{
    public sealed class InvalidCommandException : InvalidOperationException, ICoreValidationException
    {
        public InvalidCommandException(string message) : base(message)
        {
        }
    }
}