using System;
using System.Collections.Generic;
using System.Globalization;

namespace CoreValidation.Errors.Args
{
    public sealed class NumberArg<T> : NumberArg, IMessageArg<T>
    {
        private readonly Func<T, string, CultureInfo, string> _stringify;

        public NumberArg(string name, T value, Func<T, string, CultureInfo, string> stringify)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
            _stringify = stringify ?? throw new ArgumentNullException(nameof(stringify));
        }

        public T Value { get; }

        public string Name { get; }

        public IReadOnlyCollection<string> AllowedParameters { get; } = new[] {FormatParameter, CultureParameter};

        public override string ToString(IReadOnlyDictionary<string, string> parameters)
        {
            var format = parameters?.ContainsKey(FormatParameter) == true
                ? parameters[FormatParameter]
                : null;

            var culture = parameters?.ContainsKey(CultureParameter) == true
                ? CultureInfo.GetCultureInfo(parameters[CultureParameter])
                : null;

            if ((format == null) && (culture == null))
            {
                format = DefaultFormat;
                culture = DefaultCultureInfo;
            }
            else if ((format != null) && (culture == null))
            {
                culture = DefaultCultureInfo;
            }

            return _stringify(Value, format, culture);
        }
    }

    public abstract class NumberArg
    {
        protected static string FormatParameter => "format";

        protected static string CultureParameter => "culture";

        protected CultureInfo DefaultCultureInfo { get; } = CultureInfo.InvariantCulture;

        protected string DefaultFormat { get; } = string.Empty;

        public abstract string ToString(IReadOnlyDictionary<string, string> parameters);
    }
}