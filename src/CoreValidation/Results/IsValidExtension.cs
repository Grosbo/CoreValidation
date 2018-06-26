using System;
using CoreValidation.Results;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class IsValidExtension
    {
        public static bool IsValid<T>(this IValidationResult<T> @this)
            where T : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            return @this.ErrorsCollection.IsEmpty;
        }
    }
}