using System;
using CoreValidation.Exceptions;

namespace CoreValidation.Tests
{
    public sealed class UntestableException : InvalidOperationException, ICoreValidationException
    {
        public UntestableException(string message) : base(message)
        {
        }
    }
}