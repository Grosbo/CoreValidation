using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Specifications;
using CoreValidation.Translations;

namespace CoreValidation.Options
{
    internal static class OptionsService
    {
        public static IValidationContextOptions GetMerged(IValidationContextOptions baseOptions, IValidationContextOptions newOptions)
        {
            if (baseOptions == null)
            {
                throw new ArgumentNullException(nameof(baseOptions));
            }

            if (newOptions == null)
            {
                throw new ArgumentNullException(nameof(newOptions));
            }

            var compiledBase = GetVerifiedValidationContextOptions(baseOptions);

            var merged = new ValidationContextOptions();

            foreach (var translation in compiledBase.Translations)
            {
                merged.AddTranslation(translation.Name, translation.Dictionary);
            }

            foreach (var translation in newOptions.Translations)
            {
                merged.AddTranslation(translation.Name, translation.Dictionary);
            }

            OptionsUnwrapper.UnwrapSpecifications(merged, specifications =>
            {
                foreach (var pair in compiledBase.Specifications)
                {
                    specifications.Add(pair.Key, pair.Value);
                }

                foreach (var pair in newOptions.Specifications)
                {
                    if (specifications.ContainsKey(pair.Key))
                    {
                        specifications[pair.Key] = pair.Value;
                    }
                    else
                    {
                        specifications.Add(pair.Key, pair.Value);
                    }
                }
            });

            merged.Specifications = GetVerifiedSpecificationsDictionary(merged.Specifications);

            merged.ValidationOptions = GetVerifiedValidationOptions(newOptions.ValidationOptions);

            return GetVerifiedValidationContextOptions(merged);
        }

        public static IValidationContextOptions GetVerifiedValidationContextOptions(IValidationContextOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var compiledOptions = new ValidationContextOptions
            {
                Translations = GetVerifiedTranslations(options.Translations.ToArray()),
                Specifications = GetVerifiedSpecificationsDictionary(options.Specifications),
                ValidationOptions = GetVerifiedValidationOptions(options.ValidationOptions ?? throw new InvalidOperationException($"Null {nameof(ValidationContextOptions.ValidationOptions)}"))
            };

            if (options.ValidationOptions.TranslationName != null)
            {
                VerifyTranslationName(compiledOptions.Translations.ToArray(), options.ValidationOptions.TranslationName);
            }

            return compiledOptions;
        }

        public static IValidationOptions GetVerifiedValidationOptions(IValidationOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.CollectionForceKey == null)
            {
                throw new InvalidOperationException($"Null {nameof(IExecutionOptions.CollectionForceKey)}");
            }

            if (options.RequiredError == null)
            {
                throw new InvalidOperationException($"Null {nameof(IExecutionOptions.RequiredError)}");
            }

            if (options.MaxDepth < 0)
            {
                throw new InvalidOperationException($"{nameof(IExecutionOptions.MaxDepth)} cannot be negative");
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

        public static IReadOnlyCollection<Translation> GetVerifiedTranslations(IReadOnlyCollection<Translation> translations)
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
                    throw new InvalidOperationException($"Null in {nameof(translations)} collection");
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

        public static IReadOnlyDictionary<Type, object> GetVerifiedSpecificationsDictionary(IReadOnlyDictionary<Type, object> specificationsDictionary)
        {
            if (specificationsDictionary == null)
            {
                throw new ArgumentNullException(nameof(specificationsDictionary));
            }

            foreach (var pair in specificationsDictionary)
            {
                if (pair.Value == null)
                {
                    throw new InvalidOperationException($"Null specification for type {pair.Key.FullName} in the collection");
                }

                var specificationType = typeof(Specification<>).MakeGenericType(pair.Key);

                if (pair.Value.GetType() != specificationType)
                {
                    throw new InvalidSpecificationTypeException(pair.Key, pair.Value);
                }
            }

            return specificationsDictionary.ToDictionary(i => i.Key, i => i.Value);
        }

        public static string VerifyTranslationName(IReadOnlyCollection<Translation> translations, string translationName)
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