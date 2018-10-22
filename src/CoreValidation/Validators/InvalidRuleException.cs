using System;

namespace CoreValidation.Validators
{
    public sealed class InvalidRuleException : InvalidOperationException
    {
        public InvalidRuleException(string message)
            : base(message)
        {
        }
    }
}