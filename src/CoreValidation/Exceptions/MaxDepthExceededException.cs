namespace CoreValidation.Exceptions
{
    public sealed class MaxDepthExceededException : CoreValidationException
    {
        public MaxDepthExceededException(int maxDepth)
            : base($"Recursion depth of {maxDepth.ToString()} exceeded")
        {
        }
    }
}