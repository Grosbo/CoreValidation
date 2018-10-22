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

        protected CultureInfo DefaultCultureInfo { get; set; } = CultureInfo.InvariantCulture;

        protected string DefaultFormat { get; set; } = string.Empty;

        public abstract string ToString(IReadOnlyDictionary<string, string> parameters);

        public static NumberArg<int> Create(string name, int value)
        {
            return new NumberArg<int>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo));
        }

        public static NumberArg<uint> Create(string name, uint value)
        {
            return new NumberArg<uint>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo));
        }

        public static NumberArg<float> Create(string name, float value)
        {
            return new NumberArg<float>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo));
        }

        public static NumberArg<double> Create(string name, double value)
        {
            return new NumberArg<double>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo));
        }

        public static NumberArg<decimal> Create(string name, decimal value)
        {
            return new NumberArg<decimal>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo));
        }

        public static NumberArg<byte> Create(string name, byte value)
        {
            return new NumberArg<byte>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo));
        }

        public static NumberArg<sbyte> Create(string name, sbyte value)
        {
            return new NumberArg<sbyte>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo));
        }

        public static NumberArg<long> Create(string name, long value)
        {
            return new NumberArg<long>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo));
        }

        public static NumberArg<ulong> Create(string name, ulong value)
        {
            return new NumberArg<ulong>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo));
        }

        public static NumberArg<short> Create(string name, short value)
        {
            return new NumberArg<short>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo));
        }

        public static NumberArg<ushort> Create(string name, ushort value)
        {
            return new NumberArg<ushort>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo));
        }
    }
}