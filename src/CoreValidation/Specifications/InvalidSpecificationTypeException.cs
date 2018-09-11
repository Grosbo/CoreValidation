using System;

namespace CoreValidation.Specifications
{
    public sealed class InvalidSpecificationTypeException : InvalidOperationException
    {
        public InvalidSpecificationTypeException(Type type, object specification)
            : base($"Invalid specification for type {type.FullName}: {specification.GetType().FullName}")
        {
            Type = type;
            Specification = specification;
        }

        public Type Type { get; }

        public object Specification { get; }
    }
}