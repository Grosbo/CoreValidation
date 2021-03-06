﻿using System;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Exceptions;
using CoreValidation.Options;
using CoreValidation.Results;
using CoreValidation.Results.Model;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ToModelReportExtension
    {
        /// <summary>
        ///     Creates model report. Error messages are structured as the original validated model. Perfect for JSON
        ///     serialization.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="translationName">
        ///     Name of the translation to be used in the report. If null, using the default one set in
        ///     the validation context.
        /// </param>
        /// <typeparam name="T">Type of the validated model.</typeparam>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="this" /> is null.</exception>
        public static IModelReport ToModelReport<T>(this IValidationResult<T> @this, string translationName = null)
            where T : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            var translator = translationName == null
                ? @this.TranslationProxy.DefaultTranslator
                : @this.TranslationProxy.TranslatorsRepository.Get(translationName);

            return BuildModelReport(@this.ErrorsCollection, translator, 0, @this.ExecutionOptions);
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static IModelReport BuildModelReport(IErrorsCollection errorsCollection, Translator translator, int depth, IExecutionOptions executionOptions)
        {
            if (depth > executionOptions.MaxDepth)
            {
                throw new MaxDepthExceededException(executionOptions.MaxDepth);
            }

            if (errorsCollection.IsEmpty)
            {
                return ModelReport.Empty;
            }

            var errorsList = new ModelReportErrorsList();

            if (errorsCollection.Errors.Any())
            {
                var errors = errorsCollection.Errors
                    .Select(e => translator(e))
                    .Distinct()
                    .ToList();

                errorsList.AddRange(errors);
            }

            if (!errorsCollection.Members.Any())
            {
                return errorsList;
            }

            var report = errorsList.Any()
                ? new ModelReport {{string.Empty, errorsList}}
                : new ModelReport();

            foreach (var memberPair in errorsCollection.Members)
            {
                var memberReport = BuildModelReport(memberPair.Value, translator, depth + 1, executionOptions);

                if (memberReport != null)
                {
                    report.Add(memberPair.Key, memberReport);
                }
            }

            return report;
        }
    }
}