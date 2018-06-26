using System;
using System.Linq;
using CoreValidation.Factory.Validators;
using CoreValidation.Options;
using CoreValidation.Validators;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ValidatorsOptionsExtension
    {
        private static readonly OptionsUnwrapper _optionsUnwrapper = new OptionsUnwrapper();

        public static IValidationContextOptions AddValidator<T>(this IValidationContextOptions options, Validator<T> validator)
            where T : class
        {
            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            return _optionsUnwrapper.UnwrapValidators(options, validators =>
            {
                if (validators.ContainsKey(typeof(T)))
                {
                    validators[typeof(T)] = validator;
                }
                else
                {
                    validators.Add(typeof(T), validator);
                }
            });
        }

        public static IValidationContextOptions AddValidatorsFromHolder(this IValidationContextOptions options, IValidatorHolder validatorsHolder)
        {
            if (validatorsHolder == null)
            {
                throw new ArgumentNullException(nameof(validatorsHolder));
            }

            var validatedTypes = ValidatorHoldersHelpers.GetValidatedTypes(validatorsHolder.GetType());

            if (!validatedTypes.Any())
            {
                throw new InvalidOperationException($"Type passed as {nameof(validatorsHolder)} should implement at least one {typeof(IValidatorHolder<>).Name} type");
            }

            foreach (var validatedType in validatedTypes)
            {
                ValidatorHoldersHelpers.InvokeAddValidator(options, validatorsHolder, validatedType);
            }

            return options;
        }
    }
}