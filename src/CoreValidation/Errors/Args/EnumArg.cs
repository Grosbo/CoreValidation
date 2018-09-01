using System;
using System.Collections.Generic;

namespace CoreValidation.Errors.Args
{
    public sealed class EnumArg<T> : IMessageArg
        where T : struct
    {
        private static readonly string _formatParameter = "format";

        private static readonly string _defaultFormat = "G";

        private readonly T _value;

        public EnumArg(string name, T value)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _value = value;
        }

        public string Name { get; }

        public IReadOnlyCollection<string> AllowedParameters { get; } = new[] {_formatParameter};

        public string ToString(IReadOnlyDictionary<string, string> parameters)
        {
            return Enum.Format(typeof(T), _value, parameters?.ContainsKey(_formatParameter) == true
                ? parameters[_formatParameter]
                : _defaultFormat);
        }
    }
}