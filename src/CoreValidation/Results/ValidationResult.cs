using System;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Options;
using CoreValidation.Translations;

namespace CoreValidation.Results
{
    public sealed class ValidationResult<T> : IValidationResult<T>
        where T : class
    {
        internal ValidationResult(Guid coreValidatorId, ITranslationProxy translationProxy, IRulesOptions rulesOptions, T model = null, ErrorsCollection errorsCollection = null)
        {
            TranslationProxy = translationProxy ?? throw new ArgumentNullException(nameof(translationProxy));
            ErrorsCollection = errorsCollection ?? new ErrorsCollection();
            RulesOptions = rulesOptions ?? throw new ArgumentNullException(nameof(rulesOptions));

            ValidationDate = DateTime.UtcNow;
            CoreValidatorId = coreValidatorId;

            Model = model;
        }

        public ITranslationProxy TranslationProxy { get; }

        public IRulesOptions RulesOptions { get; }

        public IValidationResult<T> Merge(params IErrorsCollection[] errorsCollections)
        {
            if (errorsCollections == null)
            {
                throw new ArgumentNullException(nameof(errorsCollections));
            }

            if (errorsCollections.Contains(null))
            {
                throw new ArgumentException("Null in collection", nameof(errorsCollections));
            }

            var mergedErrorsCollection = new ErrorsCollection();

            mergedErrorsCollection.Include(ErrorsCollection);

            foreach (var errorsCollection in errorsCollections)
            {
                mergedErrorsCollection.Include(errorsCollection);
            }

            return new ValidationResult<T>(CoreValidatorId, TranslationProxy, RulesOptions, Model, mergedErrorsCollection)
            {
                ContainsMergedErrors = true
            };
        }

        public T Model { get; }

        public Guid CoreValidatorId { get; }

        public DateTime ValidationDate { get; }

        public bool ContainsMergedErrors { get; set; }

        public IErrorsCollection ErrorsCollection { get; }
    }
}