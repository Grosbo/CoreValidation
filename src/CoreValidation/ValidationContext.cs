using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
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
        public ValidationContext(IValidationContextOptions options = null)
        {
            Id = Guid.NewGuid();
            Options = OptionsService.GetVerifiedCoreValidatorOptions(options ?? new ValidationContextOptions());
            ValidatorsRepository = new ValidatorsRepository(Options.Validators);
            SpecificationsRepository = new SpecificationsRepository(ValidatorsRepository);
            TranslatorsRepository = new TranslatorsRepository(Options.Translations.ToArray());
        }

        internal IValidatorsRepository ValidatorsRepository { get; }

        public static IValidationContextFactory Factory { get; } = new ValidationContextFactory();

        public ITranslatorsRepository TranslatorsRepository { get; }

        internal ISpecificationsRepository SpecificationsRepository { get; }

        public IValidationContextOptions Options { get; }

        public IValidationResult<T> Validate<T>(T model, Func<IValidationOptions, IValidationOptions> setOptions = null)
            where T : class
        {
            if (!Types.Contains(typeof(T)))
            {
                throw new ValidatorNotFoundException(typeof(T));
            }

            var validationOptions = setOptions != null
                ? OptionsService.GetVerifiedValidationOptions(setOptions(Options.ValidationOptions))
                : Options.ValidationOptions;

            var defaultTranslator = validationOptions.TranslationName != null
                ? TranslatorsRepository.Get(validationOptions.TranslationName)
                : TranslatorsRepository.GetOriginal();

            var translationProxy = new TranslationProxy(defaultTranslator, TranslatorsRepository);

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

                        report.AddError(Options.ValidationOptions.RequiredError);

                        return new ValidationResult<T>(Id, translationProxy, Options.ValidationOptions, null, report);
                    }
                }
            }

            var specification = SpecificationsRepository.GetOrInit<T>();

            var compilationContext = new RulesExecutionContext
            {
                RulesOptions = validationOptions,
                SpecificationsRepository = SpecificationsRepository,
                ValidationStrategy = validationOptions.ValidationStrategy
            };

            var errorsCollection = SpecificationRulesExecutor.ExecuteSpecificationRules(specification, model, compilationContext, 0);

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
                ValidationOptions = OptionsService.GetVerifiedValidationOptions(Options.ValidationOptions)
            };

            var options = modifyOptions != null
                ? modifyOptions(blankOptions)
                : blankOptions;

            var finalOptions = OptionsService.GetMerged(Options, options);

            return new ValidationContext(finalOptions);
        }
    }
}