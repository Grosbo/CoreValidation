using System;

namespace CoreValidation.Exceptions
{
    public sealed class InvalidProcessedReferenceException : CoreValidationException
    {
        public InvalidProcessedReferenceException(Type type)
            : base("Reference changed")
        {
            Type = type;
        }

        public Type Type { get; }
    }
}