using System;
using CoreValidation.Exceptions;

namespace CoreValidation.Specifications
{
    public sealed class InvalidSpecificationTypeException : CoreValidationException
    {
        public InvalidSpecificationTypeException(Type type, object validator)
            : base($"Invalid specification for type {type?.FullName}: {validator?.GetType().FullName}")
        {
            Type = type;
            Validator = validator;
        }

        public Type Type { get; }

        public object Validator { get; }
    }
}