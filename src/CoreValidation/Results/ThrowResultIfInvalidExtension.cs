using System;
using CoreValidation.Results;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ThrowResultIfInvalidExtension
    {
        public static void ThrowResultIfInvalid<T>(this IValidationResult<T> @this)
            where T : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            if (!@this.IsValid)
            {
                throw new ValidationResultException<T>(@this);
            }
        }
    }
}