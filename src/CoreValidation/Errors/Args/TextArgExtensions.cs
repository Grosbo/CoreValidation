using CoreValidation.Errors.Args;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static partial class Arg
    {
        public static IMessageArg Text(string name, string value)
        {
            return new TextArg(name, value);
        }

        public static IMessageArg Text(string name, char value)
        {
            return new TextArg(name, value);
        }
    }
}