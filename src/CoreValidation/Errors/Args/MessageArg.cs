using System;
using System.Collections.Generic;

namespace CoreValidation.Errors.Args
{
    public class MessageArg<T> : IMessageArg<T>
    {
        public MessageArg(string name, T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public string Name { get; }

        public IReadOnlyCollection<string> AllowedParameters { get; } = Array.Empty<string>();

        public string ToString(IReadOnlyDictionary<string, string> parameters)
        {
            return Value.ToString();
        }

        public T Value { get; }
    }

    public sealed class MessageArg : MessageArg<string>
    {
        public MessageArg(string name, string value)
            : base(name, value)
        {
        }
    }
}