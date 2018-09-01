using System;
using CoreValidation.Exceptions;

namespace CoreValidation.Specifications
{
    public sealed class SpecificationNotFoundException : CoreValidationException
    {
        public SpecificationNotFoundException(Type type)
            : base($"Specification for type {type.FullName} not found!")
        {
            Type = type;
        }

        public Type Type { get; }
    }
}