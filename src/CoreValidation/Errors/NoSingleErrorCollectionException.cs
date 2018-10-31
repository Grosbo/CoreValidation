using System;
using CoreValidation.Exceptions;

namespace CoreValidation.Errors
{
    public sealed class NoSingleErrorCollectionException : InvalidOperationException, ICoreValidationException
    {
        public NoSingleErrorCollectionException() : base()
        {
        }
    }
}