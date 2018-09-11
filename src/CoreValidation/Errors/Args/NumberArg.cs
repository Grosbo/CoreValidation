using System;
using System.Collections.Generic;
using System.Globalization;

namespace CoreValidation.Errors.Args
{
    public class NumberArg : IMessageArg
    {
        private static readonly string _formatParameter = "format";

        private static readonly string _cultureParameter = "culture";

        private readonly CultureInfo _defaultCultureInfo = CultureInfo.InvariantCulture;

        private readonly string _defaultFormat = string.Empty;

        private readonly Func<string, CultureInfo, string> _stringify;

        private NumberArg(string name, Func<string, CultureInfo, string> stringify)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _stringify = stringify ?? throw new ArgumentNullException(nameof(stringify));
        }

        public NumberArg(string name, int value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
        }

        public NumberArg(string name, uint value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
        }

        public NumberArg(string name, float value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
        }

        public NumberArg(string name, double value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
        }

        public NumberArg(string name, decimal value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
        }

        public NumberArg(string name, byte value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
        }

        public NumberArg(string name, sbyte value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
        }

        public NumberArg(string name, long value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
        }

        public NumberArg(string name, ulong value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
        }

        public NumberArg(string name, short value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
        }

        public NumberArg(string name, ushort value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
        }

        public NumberArg(string name, Guid value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
        }

        public NumberArg(string name, DateTime value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
            _defaultFormat = DateTimeFormats.DateAndTimeFormat;
        }

        public NumberArg(string name, DateTimeOffset value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
            _defaultFormat = DateTimeFormats.DateAndTimeFormat;
        }

        public NumberArg(string name, TimeSpan value)
            : this(name, (format, cultureInfo) => value.ToString(format, cultureInfo))
        {
            _defaultFormat = DateTimeFormats.TimeSpanFormat;
        }

        public string Name { get; }

        public IReadOnlyCollection<string> AllowedParameters { get; } = new[] {_formatParameter, _cultureParameter};

        public string ToString(IReadOnlyDictionary<string, string> parameters)
        {
            var format = parameters?.ContainsKey(_formatParameter) == true
                ? parameters[_formatParameter]
                : null;

            var culture = parameters?.ContainsKey(_cultureParameter) == true
                ? CultureInfo.GetCultureInfo(parameters[_cultureParameter])
                : null;

            if ((format == null) && (culture == null))
            {
                format = _defaultFormat;
                culture = _defaultCultureInfo;
            }
            else if ((format != null) && (culture == null))
            {
                culture = _defaultCultureInfo;
            }

            return _stringify(format, culture);
        }
    }
}