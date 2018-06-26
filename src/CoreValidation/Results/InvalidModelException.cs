using System;

namespace CoreValidation.Results
{
    public abstract class InvalidModelException : Exception
    {
        public InvalidModelException(Type type, object model)
        {
            Type = type;
            Model = model;
        }

        public object Model { get; }

        public Type Type { get; }
    }

    public sealed class InvalidModelException<T> : InvalidModelException
        where T : class
    {
        public InvalidModelException(IValidationResult<T> validationResult)
            : base(typeof(T), validationResult.Model)
        {
            ValidationResult = validationResult;
        }

        public IValidationResult<T> ValidationResult { get; }

        public new T Model
        {
            get => ValidationResult.Model;
        }
    }
}