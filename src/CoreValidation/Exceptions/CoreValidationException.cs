using System;

namespace CoreValidation.Exceptions
{
    public abstract class CoreValidationException : Exception
    {
        protected CoreValidationException()
        {
        }

        protected CoreValidationException(string message)
            : base(message)
        {
        }

        protected CoreValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}