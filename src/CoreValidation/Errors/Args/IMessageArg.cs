using System.Collections.Generic;

namespace CoreValidation.Errors.Args
{
    public interface IMessageArg
    {
        /// <summary>
        ///     Name of the argument.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Names of the parameters allowed in this argument.
        /// </summary>
        IReadOnlyCollection<string> AllowedParameters { get; }

        /// <summary>
        ///     Formats the argument value according to the provided parameters.
        /// </summary>
        /// <param name="parameters">
        ///     Dictionary with parameters. Only keys that are in <see cref="AllowedParameters" /> will be
        ///     used.
        /// </param>
        string ToString(IReadOnlyDictionary<string, string> parameters);
    }

    public interface IMessageArg<out T> : IMessageArg
    {
        /// <summary>
        ///     Value of the argument.
        /// </summary>
        T Value { get; }
    }
}