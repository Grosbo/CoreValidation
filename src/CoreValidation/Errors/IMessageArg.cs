using System.Collections.Generic;

namespace CoreValidation.Errors
{
    public interface IMessageArg
    {
        string Name { get; }
        IReadOnlyCollection<string> AllowedParameters { get; }
        string ToString(IReadOnlyDictionary<string, string> parameters);
    }
}