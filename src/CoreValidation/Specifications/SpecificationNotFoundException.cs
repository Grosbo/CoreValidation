using System;

namespace CoreValidation.Specifications
{
    public sealed class SpecificationNotFoundException : ArgumentException
    {
        public SpecificationNotFoundException(Type type)
            : base($"Specification for type {type.FullName} not found!")
        {
            Type = type;
        }

        public Type Type { get; }
    }
}