using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Exceptions;
using CoreValidation.Factory;
using CoreValidation.Options;
using CoreValidation.Results;
using CoreValidation.Specifications;
using CoreValidation.Translations;
using CoreValidation.Validators;

namespace CoreValidation
{
    public sealed class ValidationContext : IValidationContext
    {
        private static readonly OptionsService _optionsService = new OptionsService();

        public ValidationContext(IValidationContextOptions options = null)
        {
            Id = Guid.NewGuid();
            Options = _optionsService.GetVerifiedCoreValidatorOptions(options ?? new ValidationContextOptions());
            ValidatorsRepository = new ValidatorsRepository(Options.Validators);
            TranslatorsRepository = new TranslatorsRepository(Options.Translations.ToArray());
        }

        internal IValidatorsRepository ValidatorsRepository { get; }

        public static IValidationContextFactory Factory { get; } = new ValidationContextFactory();

        public ITranslatorsRepository TranslatorsRepository { get; }

        public IValidationContextOptions Options { get; }

        public IValidationResult<T> Validate<T>(T model, Func<IValidationOptions, IValidationOptions> setOptions = null)
            where T : class
        {
            if (!Types.Contains(typeof(T)))
            {
                throw new ValidatorNotFoundException(typeof(T));
            }

            var validationOptions = setOptions != null
                ? _optionsService.GetVerifiedValidationOptions(setOptions(Options.ValidationOptions))
                : Options.ValidationOptions;

            var defaultTranslator = validationOptions.TranslationName != null
                ? TranslatorsRepository.Get(validationOptions.TranslationName)
                : TranslatorsRepository.GetOriginal();

            var translationProxy = new TranslationProxy(defaultTranslator, TranslatorsRepository);

            var validator = ValidatorsRepository.Get<T>();

            if (model == null)
            {
                switch (validationOptions.NullRootStrategy)
                {
                    case NullRootStrategy.ArgumentNullException:
                    {
                        throw new ArgumentNullException(nameof(model));
                    }

                    case NullRootStrategy.NoErrors:
                    {
                        return new ValidationResult<T>(Id, translationProxy, Options.ValidationOptions);
                    }

                    default:
                    {
                        var report = new ErrorsCollection();

                        report.AddError(Options.ValidationOptions.RequiredError.Clone());

                        return new ValidationResult<T>(Id, translationProxy, Options.ValidationOptions, null, report);
                    }
                }
            }

            var rawModelRules = new Specification<T>(model, ValidatorsRepository, validationOptions.ValidationStrategy, 0, Options.ValidationOptions);

            ISpecification<T> specification;

            try
            {
                specification = validator(rawModelRules);
            }
            catch (Exception ex)
            {
                throw new ValidationException(typeof(T), model, ex);
            }

            if (!ReferenceEquals(rawModelRules, specification) || !(specification is Specification<T>))
            {
                throw new InvalidProcessedReferenceException(typeof(Specification<T>));
            }

            var errorsCollection = (specification as Specification<T>).GetErrors();

            return new ValidationResult<T>(Id, translationProxy, Options.ValidationOptions, model, errorsCollection);
        }

        public Guid Id { get; }

        public IReadOnlyCollection<Type> Types => ValidatorsRepository.Types;

        public IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> Translations => TranslatorsRepository.Translations;

        public IValidationOptions ValidationOptions => Options.ValidationOptions;

        public IValidationContext Clone(Func<IValidationContextOptions, IValidationContextOptions> modifyOptions = null)
        {
            var blankOptions = new ValidationContextOptions
            {
                ValidationOptions = _optionsService.GetVerifiedValidationOptions(Options.ValidationOptions)
            };

            var options = modifyOptions != null
                ? modifyOptions(blankOptions)
                : blankOptions;

            var finalOptions = _optionsService.GetMerged(Options, options);

            return new ValidationContext(finalOptions);
        }
    }
}