using System;

using CoreValidation.Exceptions;

namespace CoreValidation.Validators
{
    public sealed class InvalidValidatorTypeException : CoreValidationException
    {
        public InvalidValidatorTypeException(Type type, object validator)
            : base($"Invalid validator for type {type?.FullName}: {validator?.GetType().FullName}")
        {
            Type = type;
            Validator = validator;
        }

        public Type Type { get; }

        public object Validator { get; }
    }
}