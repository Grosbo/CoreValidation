﻿using System;
using System.Collections.Generic;
using CoreValidation.Translations;

namespace CoreValidation.Options
{
    public interface IValidationContextOptions
    {
        IReadOnlyCollection<Translation> Translations { get; }

        IReadOnlyDictionary<Type, object> Validators { get; }

        IValidationOptions ValidationOptions { get; }
    }
}