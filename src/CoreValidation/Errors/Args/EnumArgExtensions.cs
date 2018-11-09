using CoreValidation.Errors.Args;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static partial class Arg
    {
        /// <summary>
        /// Creates Enum argument.
        /// Parameter: 'format' with default value 'G', reflects the functionality of ToString(format), e.g. {name|format=G}
        /// </summary>
        /// <param name="name">Name of the argument.</param>
        /// <param name="value">Enum value.</param>
        /// <typeparam name="T">Type of the enum.</typeparam>
        /// <returns></returns>
        public static IMessageArg Enum<T>(string name, T value) where T : struct
        {
            return new EnumArg<T>(name, value);
        }
    }
}