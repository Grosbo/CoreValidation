using System;

namespace CoreValidation.Exceptions
{
    public sealed class MaxDepthExceededException : InvalidOperationException
    {
        public MaxDepthExceededException(int maxDepth)
            : base($"Recursion depth of {maxDepth.ToString()} exceeded")
        {
        }
    }
}