using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Translations;
using CoreValidation.Validators;

namespace CoreValidation.Options
{
    internal sealed class OptionsService
    {
        private static readonly OptionsUnwrapper _optionsUnwrapper = new OptionsUnwrapper();

        public IValidationContextOptions GetMerged(IValidationContextOptions baseOptions, IValidationContextOptions newOptions)
        {
            if (baseOptions == null)
            {
                throw new ArgumentNullException(nameof(baseOptions));
            }

            if (newOptions == null)
            {
                throw new ArgumentNullException(nameof(newOptions));
            }

            var compiledBase = GetVerifiedCoreValidatorOptions(baseOptions);

            var merged = new ValidationContextOptions();

            foreach (var translation in compiledBase.Translations)
            {
                merged.AddTranslation(translation.Name, translation.Dictionary);
            }

            foreach (var translation in newOptions.Translations)
            {
                merged.AddTranslation(translation.Name, translation.Dictionary);
            }

            _optionsUnwrapper.UnwrapValidators(merged, validators =>
            {
                foreach (var pair in compiledBase.Validators)
                {
                    validators.Add(pair.Key, pair.Value);
                }

                foreach (var pair in newOptions.Validators)
                {
                    if (validators.ContainsKey(pair.Key))
                    {
                        validators[pair.Key] = pair.Value;
                    }
                    else
                    {
                        validators.Add(pair.Key, pair.Value);
                    }
                }
            });

            merged.Validators = GetVerifiedValidatorsDictionary(merged.Validators);

            merged.ValidationOptions = GetVerifiedValidationOptions(newOptions.ValidationOptions);

            return GetVerifiedCoreValidatorOptions(merged);
        }

        public IValidationContextOptions GetVerifiedCoreValidatorOptions(IValidationContextOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var compiledOptions = new ValidationContextOptions
            {
                Translations = GetVerifiedTranslations(options.Translations.ToArray()),
                Validators = GetVerifiedValidatorsDictionary(options.Validators),
                ValidationOptions = GetVerifiedValidationOptions(options.ValidationOptions ?? throw new InvalidOperationException($"Null {nameof(ValidationContextOptions.ValidationOptions)}"))
            };

            if (options.ValidationOptions.TranslationName != null)
            {
                VerifyTranslationName(compiledOptions.Translations.ToArray(), options.ValidationOptions.TranslationName);
            }

            return compiledOptions;
        }

        public IValidationOptions GetVerifiedValidationOptions(IValidationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.CollectionForceKey == null)
            {
                throw new InvalidOperationException($"Null {nameof(IRulesOptions.CollectionForceKey)}");
            }

            if (options.RequiredError == null)
            {
                throw new InvalidOperationException($"Null {nameof(IRulesOptions.RequiredError)}");
            }

            if (options.MaxDepth < 0)
            {
                throw new InvalidOperationException($"{nameof(IRulesOptions.MaxDepth)} cannot be negative");
            }

            return new ValidationOptions
            {
                TranslationName = options.TranslationName,
                NullRootStrategy = options.NullRootStrategy,
                ValidationStrategy = options.ValidationStrategy,
                CollectionForceKey = options.CollectionForceKey,
                MaxDepth = options.MaxDepth,
                RequiredError = options.RequiredError,
                DefaultError = options.DefaultError
            };
        }

        public IReadOnlyCollection<Translation> GetVerifiedTranslations(IReadOnlyCollection<Translation> translations)
        {
            if (translations == null)
            {
                throw new ArgumentNullException(nameof(translations));
            }

            var compiled = new List<Translation>();

            foreach (var translation in translations)
            {
                if (translation == null)
                {
                    throw new InvalidOperationException($"Null in {translations} collection");
                }

                var compiledDictionary = compiled.FirstOrDefault(d => d.Name == translation.Name);

                if (compiledDictionary != null)
                {
                    foreach (var pair in translation.Dictionary)
                    {
                        compiledDictionary.Dictionary[pair.Key] = pair.Value;
                    }
                }
                else
                {
                    compiled.Add(translation);
                }
            }

            return compiled;
        }

        public IReadOnlyDictionary<Type, object> GetVerifiedValidatorsDictionary(IReadOnlyDictionary<Type, object> validatorsDictionary)
        {
            if (validatorsDictionary == null)
            {
                throw new ArgumentNullException(nameof(validatorsDictionary));
            }

            foreach (var pair in validatorsDictionary)
            {
                if (pair.Value == null)
                {
                    throw new InvalidOperationException($"Null validator for type {pair.Key.FullName} in the collection");
                }

                var validatorType = typeof(Validator<>).MakeGenericType(pair.Key);

                if (pair.Value.GetType() != validatorType)
                {
                    throw new InvalidValidatorTypeException(pair.Key, pair.Value);
                }
            }

            return validatorsDictionary.ToDictionary(i => i.Key, i => i.Value);
        }

        public string VerifyTranslationName(IReadOnlyCollection<Translation> translations, string translationName)
        {
            if (translations == null)
            {
                throw new ArgumentNullException(nameof(translations));
            }

            if (translationName == null)
            {
                return null;
            }

            if (!translations.Any() || translations.All(d => d.Name != translationName))
            {
                throw new TranslationNotFoundException(translationName);
            }

            return translationName;
        }
    }
}