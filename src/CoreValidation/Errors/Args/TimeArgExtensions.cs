using System;
using CoreValidation.Errors.Args;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static partial class Arg
    {
        public static IMessageArg Time(string name, DateTime value)
        {
            return new TimeArg<DateTime>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo))
            {
                DefaultFormat = DateTimeFormats.DateAndTimeFormat
            };
        }

        public static IMessageArg Time(string name, DateTimeOffset value)
        {
            return new TimeArg<DateTimeOffset>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo))
            {
                DefaultFormat = DateTimeFormats.DateAndTimeFormat
            };
        }

        public static IMessageArg Time(string name, TimeSpan value)
        {
            return new TimeArg<TimeSpan>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo))
            {
                DefaultFormat = DateTimeFormats.TimeSpanFormat
            };
        }
    }
}