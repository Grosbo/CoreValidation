using CoreValidation.Errors.Args;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static partial class Arg
    {
        public static IMessageArg Enum<T>(string name, T value) where T : struct
        {
            return new EnumArg<T>(name, value);
        }
    }
}