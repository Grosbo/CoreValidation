using System.Collections.Generic;
using CoreValidation.Errors.Args;

namespace CoreValidation.Errors
{
    public interface IError
    {
        string Message { get; }
        IReadOnlyCollection<IMessageArg> Arguments { get; }
    }
}