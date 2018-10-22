using System.Collections.Generic;

namespace CoreValidation.Errors.Args
{
    public interface IMessageArg
    {
        string Name { get; }
        IReadOnlyCollection<string> AllowedParameters { get; }
        string ToString(IReadOnlyDictionary<string, string> parameters);
    }

    public interface IMessageArg<out T> : IMessageArg
    {
        T Value { get; }
    }
}