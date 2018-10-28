using System;
using CoreValidation.Results;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ThrowResultIfInvalidExtension
    {
        /// <summary>
        /// Throws <see cref="ValidationResultException{T}"/> if the result contains errors.
        /// </summary>
        /// <param name="this"></param>
        /// <typeparam name="T">Type of the validated model.</typeparam>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="this"/> is null.</exception>
        /// <exception cref="ValidationResultException{T}">Thrown if the result contains errors.</exception>
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