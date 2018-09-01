using System;
using CoreValidation.Errors;
using CoreValidation.Results;
using CoreValidation.Translations;
using Moq;

namespace CoreValidation.UnitTests.Results
{
    public static class ResultsTestHelpers
    {
        public static IValidationResult<object> MockValidationResult(IErrorsCollection errorsCollection, ExecutionOptionsStub executionOptions = null, Translation[] translations = null)
        {
            var resultMock = new Mock<IValidationResult<object>>();

            resultMock.Setup(v => v.ErrorsCollection).Returns(errorsCollection);
            resultMock.Setup(v => v.TranslationProxy).Returns(new TranslationProxy(e => e.FormattedMessage, new TranslatorsRepository(translations ?? Array.Empty<Translation>())));
            resultMock.Setup(v => v.ExecutionOptions).Returns(executionOptions ?? new ExecutionOptionsStub {MaxDepth = 10, CollectionForceKey = "*", RequiredError = new Error("Required")});

            return resultMock.Object;
        }
    }
}