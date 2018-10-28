using System;

namespace CoreValidation.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the maximum level of depth in the validated model has been exceeded.
    /// </summary>
    public sealed class MaxDepthExceededException : InvalidOperationException
    {
        /// <summary>
        /// Maximum depth allowed.
        /// </summary>
        public int MaxDepth { get; }

        public MaxDepthExceededException(int maxDepth)
            : base($"Recursion depth of {maxDepth.ToString()} exceeded")
        {
            MaxDepth = maxDepth;
        }
    }
}