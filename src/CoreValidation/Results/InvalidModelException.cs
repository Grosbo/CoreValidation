using System;

namespace CoreValidation.Results
{
    public abstract class InvalidModelException : Exception
    {
        public InvalidModelException(Type type, string message)
            : base(message)
        {
            Type = type;
        }

        public Type Type { get; }
    }

    public class InvalidModelException<T> : InvalidModelException
    {
        public InvalidModelException(string message) : base(typeof(T), message)
        {
        }
    }
}