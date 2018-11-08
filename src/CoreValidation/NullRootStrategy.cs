namespace CoreValidation
{
    /// <summary>
    ///     Behavior for the null reference passed to be validated
    /// </summary>
    public enum NullRootStrategy
    {
        /// <summary>
        ///     Throw <see cref="ArgumentNullException" />
        /// </summary>
        ArgumentNullException = 0,


        /// <summary>
        ///     Return Required error at the top level
        /// </summary>
        RequiredError = 1,


        /// <summary>
        ///     Return no errors
        /// </summary>
        NoErrors = 2
    }
}