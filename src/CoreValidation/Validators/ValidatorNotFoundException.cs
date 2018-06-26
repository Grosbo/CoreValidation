using System;

using CoreValidation.Exceptions;

namespace CoreValidation.Validators
{
    public sealed class ValidatorNotFoundException : CoreValidationException
    {
        public ValidatorNotFoundException(Type type)
            : base($"Validator for type {type.FullName} not found!")
        {
            Type = type;
        }

        public Type Type { get; }
    }
}