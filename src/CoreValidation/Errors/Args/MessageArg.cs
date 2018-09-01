using System;
using System.Collections.Generic;

namespace CoreValidation.Errors.Args
{
    public class MessageArg<T> : IMessageArg
    {
        private readonly string _value;

        public MessageArg(string name, T value)
        {
            _value = value?.ToString() ?? throw new ArgumentNullException(nameof(name));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public IReadOnlyCollection<string> AllowedParameters { get; } = Array.Empty<string>();

        public string ToString(IReadOnlyDictionary<string, string> parameters)
        {
            return _value;
        }
    }

    public sealed class MessageArg : MessageArg<string>
    {
        public MessageArg(string name, string value)
            : base(name, value)
        {
        }
    }
}