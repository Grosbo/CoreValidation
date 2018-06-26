using System;
using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Translations;

namespace CoreValidation.Options
{
    public sealed class ValidationContextOptions : IValidationContextOptions
    {
        public IReadOnlyCollection<Translation> Translations { get; set; } = new List<Translation>();

        public IReadOnlyDictionary<Type, object> Validators { get; set; } = new Dictionary<Type, object>();

        public IValidationOptions ValidationOptions { get; set; } = new ValidationOptions
        {
            NullRootStrategy = NullRootStrategy.RequiredError,
            ValidationStrategy = ValidationStrategy.Complete,
            TranslationName = null,
            CollectionForceKey = "*",
            MaxDepth = 10,
            RequiredError = new Error("Required")
        };
    }
}