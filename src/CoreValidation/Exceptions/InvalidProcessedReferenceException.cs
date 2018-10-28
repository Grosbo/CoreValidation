using System;

namespace CoreValidation.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the reference of the fluent api builder is different on the output than on the input.
    /// </summary>
    public sealed class InvalidProcessedReferenceException : InvalidOperationException
    {
        public InvalidProcessedReferenceException(Type type)
            : base("Reference changed")
        {
            Type = type;
        }

        /// <summary>
        /// Type of the fluent api builder.
        /// </summary>
        public Type Type { get; }
    }
}