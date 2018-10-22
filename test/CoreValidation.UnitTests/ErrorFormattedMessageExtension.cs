using CoreValidation.Errors;

namespace CoreValidation.UnitTests
{
    public static class ErrorFormattedMessageExtension
    {
        public static string ToFormattedMessage(this IError @this)
        {
            return MessageFormatter.Format(@this.Message, @this.Arguments);
        }
    }
}