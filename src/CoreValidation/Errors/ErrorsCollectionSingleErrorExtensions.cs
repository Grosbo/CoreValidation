using System;
using System.Linq;
using CoreValidation.Exceptions;

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
                throw new NoSingleErrorCollectionException();
            }

            return @this.Errors.Single();
        }
    }
}