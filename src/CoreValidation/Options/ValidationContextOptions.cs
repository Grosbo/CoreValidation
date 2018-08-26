using System;
using System.Collections.Generic;
using CoreValidation.Translations;

namespace CoreValidation.Options
{
    public sealed class ValidationContextOptions : IValidationContextOptions
    {
        public IReadOnlyCollection<Translation> Translations { get; set; } = new List<Translation>();

        public IReadOnlyDictionary<Type, object> Validators { get; set; } = new Dictionary<Type, object>();

        // todo: why when applying static immutable default options, it breaks when working parallely ?
        public IValidationOptions ValidationOptions { get; set; } = Options.ValidationOptions.CreateDefault();
    }
}