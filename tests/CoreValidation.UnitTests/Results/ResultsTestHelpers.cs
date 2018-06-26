using System;
using CoreValidation.Errors;
using CoreValidation.Results;
using CoreValidation.Translations;
using Moq;

namespace CoreValidation.UnitTests.Results
{
    public static class ResultsTestHelpers
    {
        public static IValidationResult<object> MockValidationResult(IErrorsCollection errorsCollection, RulesOptionsStub rulesOptions = null, Translation[] translations = null)
        {
            var resultMock = new Mock<IValidationResult<object>>();

            resultMock.Setup(v => v.ErrorsCollection).Returns(errorsCollection);
            resultMock.Setup(v => v.TranslationProxy).Returns(new TranslationProxy(e => e.StringifiedMessage, new TranslatorsRepository(translations ?? Array.Empty<Translation>())));
            resultMock.Setup(v => v.RulesOptions).Returns(rulesOptions ?? new RulesOptionsStub {MaxDepth = 10, CollectionForceKey = "*", RequiredError = new Error("Required")});

            return resultMock.Object;
        }
    }
}