using System;
using System.Collections.Generic;
using CoreValidation.Translations;

namespace CoreValidation.Options
{
    internal sealed class ValidationContextOptions : IValidationContextOptions
    {
        public IReadOnlyCollection<Translation> Translations { get; set; } = new List<Translation>();

        public IReadOnlyDictionary<Type, object> Specifications { get; set; } = new Dictionary<Type, object>();

        public IValidationOptions ValidationOptions { get; set; } = Options.ValidationOptions.CreateDefault();
    }
}