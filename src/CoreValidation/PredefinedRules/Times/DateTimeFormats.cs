// ReSharper disable once CheckNamespace

namespace CoreValidation
{
    public static class DateTimeFormats
    {
        public static string JustDateFormat { get; } = "yyyy-MM-dd";
        public static string JustTimeFormat { get; } = "HH:mm:ss.FFFFFFF";
        public static string AllFormat { get; } = "yyyy-MM-dd HH:mm:ss.FFFFFFF";
        public static string TimeSpanFormat { get; } = "c";

        public static string GetFormat(TimeComparison timeComparison)
        {
            switch (timeComparison)
            {
                case TimeComparison.JustDate:

                    return JustDateFormat;

                case TimeComparison.JustTime:

                    return JustTimeFormat;

                default:

                    return AllFormat;
            }
        }
    }
}