using System;
using CoreValidation.Exceptions;
using CoreValidation.Options;

namespace CoreValidation.Factory
{
    public sealed class ValidationContextFactory : IValidationContextFactory
    {
        internal ValidationContextFactory()
        {
        }

        public IValidationContext Create(Func<IValidationContextOptions, IValidationContextOptions> options = null)
        {
            var newOptions = new ValidationContextOptions();

            var setOptionsFunction = options ?? (o => o);

            var finalOptions = setOptionsFunction(newOptions);

            if (!ReferenceEquals(newOptions, finalOptions))
            {
                throw new InvalidProcessedReferenceException(typeof(IValidationContextOptions));
            }

            return new ValidationContext(finalOptions);
        }
    }
}