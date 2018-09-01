using System;
using CoreValidation.Errors;
using CoreValidation.Options;
using CoreValidation.Translations;

namespace CoreValidation.Results
{
    public interface IValidationResult<out T>
        where T : class
    {
        T Model { get; }

        IErrorsCollection ErrorsCollection { get; }

        Guid CoreValidatorId { get; }

        DateTime ValidationDate { get; }

        bool ContainsMergedErrors { get; }

        IExecutionOptions ExecutionOptions { get; }

        ITranslationProxy TranslationProxy { get; }

        IValidationResult<T> Merge(params IErrorsCollection[] errorsCollections);
    }
}