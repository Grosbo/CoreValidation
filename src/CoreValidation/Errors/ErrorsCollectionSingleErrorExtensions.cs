using System;
using System.Linq;

namespace CoreValidation.Errors
{
    internal static class ErrorsCollectionSingleErrorExtensions
    {
        public static bool ContainsSingleError(this IErrorsCollection @this)
        {
            return (@this.Errors.Count == 1) && !@this.Members.Any();
        }

        public static IError GetSingleError(this IErrorsCollection @this)
        {
            if (!@this.ContainsSingleError())
            {
                throw new InvalidOperationException("Collection doesn't hold a single error");
            }

            return @this.Errors.Single();
        }
    }
}