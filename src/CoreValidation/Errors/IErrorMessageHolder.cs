using System.Collections.Generic;

namespace CoreValidation.Errors
{
    public interface IErrorMessageHolder
    {
        string Message { get; }

        IReadOnlyCollection<IMessageArg> Arguments { get; }
    }
}