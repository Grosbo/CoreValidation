﻿using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Exceptions;
using CoreValidation.Options;
using CoreValidation.Results;
using CoreValidation.Results.List;
using CoreValidation.Translations;

// ReSharper disable once CheckNamespace
namespace CoreValidation
{
    public static class ToListReportExtension
    {
        public static ListReport ToListReport<T>(this IValidationResult<T> @this, string translationName = null)
            where T : class
        {
            var listReport = new ListReport();

            var translator = translationName == null
                ? @this.TranslationProxy.DefaultTranslator
                : @this.TranslationProxy.TranslatorsRepository.Get(translationName);

            BuildListReport(listReport, string.Empty, @this.ErrorsCollection, translator, 0, @this.RulesOptions);

            return listReport;
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void BuildListReport(ListReport listReport, string path, IErrorsCollection errorsCollection, Translator translator, int depth, IRulesOptions rulesOptions)
        {
            if (depth > rulesOptions.MaxDepth)
            {
                throw new MaxDepthExceededException(rulesOptions.MaxDepth);
            }

            if (errorsCollection.IsEmpty)
            {
                return;
            }

            if (errorsCollection.Errors.Any())
            {
                listReport.AddRange(errorsCollection.Errors.Select(m =>
                {
                    return string.IsNullOrWhiteSpace(path)
                        ? translator(m)
                        : $"{path}: {translator(m)}";

                }).ToList());
            }

            foreach (var memberPair in errorsCollection.Members)
            {
                var currentPath = string.IsNullOrEmpty(path) ? string.Empty : $"{path}.";

                BuildListReport(listReport, $"{currentPath}{memberPair.Key}", memberPair.Value, translator, depth + 1, rulesOptions);
            }
        }
    }
}