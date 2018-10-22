using System;
using System.Collections.Generic;
using System.Globalization;

namespace CoreValidation.Errors.Args
{
    public sealed class TimeArg<T> : TimeArg, IMessageArg
    {
        private readonly Func<T, string, CultureInfo, string> _stringify;

        public TimeArg(string name, T value, Func<T, string, CultureInfo, string> stringify)
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

    public abstract class TimeArg
    {
        protected static string FormatParameter => "format";

        protected static string CultureParameter => "culture";

        protected CultureInfo DefaultCultureInfo { get; set; } = CultureInfo.InvariantCulture;

        protected string DefaultFormat { get; set; } = string.Empty;

        public abstract string ToString(IReadOnlyDictionary<string, string> parameters);

        public static TimeArg<DateTime> Create(string name, DateTime value)
        {
            return new TimeArg<DateTime>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo))
            {
                DefaultFormat = DateTimeFormats.DateAndTimeFormat
            };
        }

        public static TimeArg<DateTimeOffset> Create(string name, DateTimeOffset value)
        {
            return new TimeArg<DateTimeOffset>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo))
            {
                DefaultFormat = DateTimeFormats.DateAndTimeFormat
            };
        }

        public static TimeArg<TimeSpan> Create(string name, TimeSpan value)
        {
            return new TimeArg<TimeSpan>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo))
            {
                DefaultFormat = DateTimeFormats.TimeSpanFormat
            };
        }
    }
}