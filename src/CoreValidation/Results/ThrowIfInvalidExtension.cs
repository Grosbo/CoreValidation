﻿using CoreValidation.Results;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ThrowIfInvalidExtension
    {
        public static void ThrowIfInvalid<T>(this IValidationResult<T> @this)
            where T : class
        {
            if (!@this.IsValid())
            {
                throw new InvalidModelException<T>(@this);
            }
        }
    }
}