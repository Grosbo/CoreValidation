namespace CoreValidation.Validators
{
    public sealed class InvalidCommandOrderException : CommandException
    {
        public InvalidCommandOrderException(string message, int order, string name)
            : base(message, order, name)
        {
        }
    }
}