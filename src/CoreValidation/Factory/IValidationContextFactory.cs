using System;
using CoreValidation.Options;

namespace CoreValidation.Factory
{
    public interface IValidationContextFactory
    {
        IValidationContext Create(Func<IValidationContextOptions, IValidationContextOptions> options);
    }
}