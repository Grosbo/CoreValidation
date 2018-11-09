using CoreValidation.Errors.Args;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static partial class Arg
    {
        /// <summary>
        /// Creates text argument.
        /// Parameter: 'case', not used if not set, possible values: 'upper', 'lower', e.g. {name|case=upper}
        /// </summary>
        /// <param name="name">Name of the argument.</param>
        /// <param name="value">Text value.</param>
        /// <returns></returns>
        public static IMessageArg Text(string name, string value)
        {
            return new TextArg(name, value);
        }

        /// <summary>
        /// Creates text argument.
        /// Parameter: 'case', not used if not set, possible values: 'upper', 'lower', e.g. {name|case=upper}
        /// </summary>
        /// <param name="name">Name of the argument.</param>
        /// <param name="value">Text value.</param>
        /// <returns></returns>
        public static IMessageArg Text(string name, char value)
        {
            return new TextArg(name, value);
        }
    }
}