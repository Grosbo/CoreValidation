using System;
using CoreValidation.Exceptions;

namespace CoreValidation
{
    public sealed class ValidationException : CoreValidationException
    {
        public ValidationException(Type type, object model, Exception innerException)
            : base($"Exception occured while validating {type.FullName}", innerException)
        {
            Type = type;
            Model = model;
        }

        public Type Type { get; }

        public object Model { get; }
    }
}