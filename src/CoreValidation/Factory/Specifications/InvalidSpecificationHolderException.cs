using System;
using CoreValidation.Exceptions;

namespace CoreValidation.Factory.Specifications
{
    public sealed class InvalidSpecificationHolderException : InvalidOperationException, ICoreValidationException
    {
        public InvalidSpecificationHolderException(string message) : base(message)
        {
        }
    }
}