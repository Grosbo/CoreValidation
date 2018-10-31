using System;
using CoreValidation.Exceptions;

namespace CoreValidation.Translations
{
    public sealed class DuplicateTranslationException : InvalidOperationException, ICoreValidationException
    {
        public DuplicateTranslationException(string message) : base(message)
        {
        }
    }
}