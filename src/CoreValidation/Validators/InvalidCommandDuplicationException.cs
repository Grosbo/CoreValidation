namespace CoreValidation.Validators
{
    public class InvalidCommandDuplicationException : CommandException
    {
        public InvalidCommandDuplicationException(int order, string name)
            : base($"{name} can be called only once inside a scope", order, name)
        {
        }
    }
}