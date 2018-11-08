using System;
using System.Collections.Generic;
using CoreValidation.Translations;

namespace CoreValidation.Options
{
    /// <summary>
    ///     Options of the <see cref="ValidationContext" />
    /// </summary>
    public interface IValidationContextOptions
    {
        /// <summary>
        ///     Translations available in the validation context.
        /// </summary>
        IReadOnlyCollection<Translation> Translations { get; }

        /// <summary>
        ///     Specifications available in the validation context.
        /// </summary>
        IReadOnlyDictionary<Type, object> Specifications { get; }

        /// <summary>
        ///     Default options of the validation process.
        /// </summary>
        IValidationOptions ValidationOptions { get; }
    }
}