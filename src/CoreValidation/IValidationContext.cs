using System;
using System.Collections.Generic;
using CoreValidation.Options;
using CoreValidation.Results;

namespace CoreValidation
{
    public interface IValidationContext
    {
        Guid Id { get; }

        IReadOnlyCollection<Type> Types { get; }

        IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Translations { get; }

        IValidationOptions ValidationOptions { get; }

        IValidationResult<T> Validate<T>(T model, Func<IValidationOptions, IValidationOptions> modifyOptions = null)
            where T : class;

        IValidationContext Clone(Func<IValidationContextOptions, IValidationContextOptions> modifyOptions = null);
    }
}