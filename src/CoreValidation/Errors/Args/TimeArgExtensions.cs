using System;
using CoreValidation.Errors.Args;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static partial class Arg
    {
        /// <summary>
        /// Creates time argument.
        /// Parameter: 'format', not used if not set, reflects the functionality of ToString(format), e.g. {name|format=g}, {name|format=yyyy-MM-dd}
        /// Parameter: 'cultureInfo', CultureInfo.InvariantCulture if not set, reflects the functionality of ToString(cultureInfo), e.g. {name|cultureInfo=en-US}
        /// </summary>
        /// <param name="name">Name of the argument.</param>
        /// <param name="value">Time value.</param>
        /// <returns></returns>
        public static IMessageArg Time(string name, DateTime value)
        {
            return new TimeArg<DateTime>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo))
            {
                DefaultFormat = DateTimeFormats.DateAndTimeFormat
            };
        }

        /// <summary>
        /// Creates time argument.
        /// Parameter: 'format', not used if not set, reflects the functionality of ToString(format), e.g. {name|format=g}, {name|format=yyyy-MM-dd}
        /// Parameter: 'cultureInfo', CultureInfo.InvariantCulture if not set, reflects the functionality of ToString(cultureInfo), e.g. {name|cultureInfo=en-US}
        /// </summary>
        /// <param name="name">Name of the argument.</param>
        /// <param name="value">Time value.</param>
        /// <returns></returns>
        public static IMessageArg Time(string name, DateTimeOffset value)
        {
            return new TimeArg<DateTimeOffset>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo))
            {
                DefaultFormat = DateTimeFormats.DateAndTimeFormat
            };
        }

        /// <summary>
        /// Creates time argument.
        /// Parameter: 'format', not used if not set, reflects the functionality of ToString(format), e.g. {name|format=g}, {name|format=yyyy-MM-dd}
        /// Parameter: 'cultureInfo', CultureInfo.InvariantCulture if not set, reflects the functionality of ToString(cultureInfo), e.g. {name|cultureInfo=en-US}
        /// </summary>
        /// <param name="name">Name of the argument.</param>
        /// <param name="value">Time value.</param>
        /// <returns></returns>
        public static IMessageArg Time(string name, TimeSpan value)
        {
            return new TimeArg<TimeSpan>(name, value, (v, format, cultureInfo) => v.ToString(format, cultureInfo))
            {
                DefaultFormat = DateTimeFormats.TimeSpanFormat
            };
        }
    }
}