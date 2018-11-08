using System.Collections.Generic;
using CoreValidation.Errors.Args;

namespace CoreValidation.Errors
{
    public interface IError
    {
        /// <summary>
        ///     Error message.
        /// </summary>
        string Message { get; }

        /// <summary>
        ///     Error message arguments.
        ///     They can be used in the placeholders within <see cref="Message" />.
        /// </summary>
        IReadOnlyCollection<IMessageArg> Arguments { get; }
    }
}