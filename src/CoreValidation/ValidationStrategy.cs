namespace CoreValidation
{
    /// <summary>
    ///     Strategy of the validation process
    /// </summary>
    public enum ValidationStrategy
    {
        /// <summary>
        ///     Validate the model and return all existing errors
        /// </summary>
        Complete = 0,


        /// <summary>
        ///     Validate the model until the first error
        /// </summary>
        FailFast = 1,


        /// <summary>
        ///     Don't validate model - just return all theoretically possible errors
        /// </summary>
        Force = 2
    }
}