using System;

namespace CoreValidation.Results
{
    public abstract class InvalidModelException<T> : InvalidModelException
    {
        protected InvalidModelException() : base(typeof(T), $"Invalid model of type {typeof(T).FullName}.")
        {
        }

        protected InvalidModelException(string message) : base(typeof(T), message)
        {
        }
    }

    public abstract class InvalidModelException : Exception
    {
        public InvalidModelException(Type type)
        {
            Type = type;
        }

        public InvalidModelException(Type type, string message)
            : base(message)
        {
            Type = type;
        }

        public Type Type { get; }
    }
}