using System;
using System.Linq;
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
        /// <summary>
        /// Creates List Report. Report is a list of error messages. ToString() returns them in a single string.
        /// </summary>
        /// <param name="this"></param>
        /// <param name="translationName">Name of the translation to be used in the report. If null, using the default one set in the validation context.</param>
        /// <typeparam name="T">Type of the validated model.</typeparam>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="this"/> is null.</exception>
        public static ListReport ToListReport<T>(this IValidationResult<T> @this, string translationName = null)
            where T : class
        {
            if (@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }

            var listReport = new ListReport();

            var translator = translationName == null
                ? @this.TranslationProxy.DefaultTranslator
                : @this.TranslationProxy.TranslatorsRepository.Get(translationName);

            BuildListReport(listReport, string.Empty, @this.ErrorsCollection, translator, 0, @this.ExecutionOptions);

            return listReport;
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void BuildListReport(ListReport listReport, string path, IErrorsCollection errorsCollection, Translator translator, int depth, IExecutionOptions executionOptions)
        {
            if (depth > executionOptions.MaxDepth)
            {
                throw new MaxDepthExceededException(executionOptions.MaxDepth);
            }

            if (errorsCollection.IsEmpty)
            {
                return;
            }

            if (errorsCollection.Errors.Any())
            {
                listReport.AddRange(errorsCollection.Errors
                    .Select(m => string.IsNullOrWhiteSpace(path)
                        ? translator(m)
                        : $"{path}: {translator(m)}")
                    .Distinct()
                    .ToList()
                );
            }

            foreach (var memberPair in errorsCollection.Members)
            {
                var currentPath = string.IsNullOrEmpty(path) ? string.Empty : $"{path}.";

                BuildListReport(listReport, $"{currentPath}{memberPair.Key}", memberPair.Value, translator, depth + 1, executionOptions);
            }
        }
    }
}