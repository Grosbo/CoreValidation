using System;
using System.Collections.Generic;

namespace CoreValidation.Errors.Args
{
    public sealed class EnumArg<T> : IMessageArg<T>
        where T : struct
    {
        private static readonly string _formatParameter = "format";

        private static readonly string _defaultFormat = "G";


        public EnumArg(string name, T value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
        }

        public string Name { get; }

        public IReadOnlyCollection<string> AllowedParameters { get; } = new[] {_formatParameter};

        public string ToString(IReadOnlyDictionary<string, string> parameters)
        {
            return Enum.Format(typeof(T), Value, parameters?.ContainsKey(_formatParameter) == true
                ? parameters[_formatParameter]
                : _defaultFormat);
        }

        public T Value { get; }
    }
}