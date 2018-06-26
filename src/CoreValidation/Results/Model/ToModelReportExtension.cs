using System;
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

            return BuildModelReport(@this.ErrorsCollection, translator, 0, @this.RulesOptions);
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static IModelReport BuildModelReport(IErrorsCollection errorsCollection, Translator translator, int depth, IRulesOptions rulesOptions)
        {
            if (depth > rulesOptions.MaxDepth)
            {
                throw new MaxDepthExceededException(rulesOptions.MaxDepth);
            }

            if (errorsCollection.IsEmpty)
            {
                return new ModelReport();
            }

            var errorsList = new ModelReportErrorsList();

            if (errorsCollection.Errors.Any())
            {
                var errors = errorsCollection.Errors.Select(e => translator(e));

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
                var memberReport = BuildModelReport(memberPair.Value, translator, depth + 1, rulesOptions);

                if (memberReport != null)
                {
                    report.Add(memberPair.Key, memberReport);
                }
            }

            return report;
        }
    }
}