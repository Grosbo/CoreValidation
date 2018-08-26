using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Options;
using CoreValidation.Specifications;
using Xunit;

namespace CoreValidation.PredefinedRules.UnitTests
{
    internal static class RulesHelper
    {
        private static readonly IRulesOptions _rulesOptions = new RulesOptionsStub();

        public static IErrorsCollection CompileSingleError<TMember>(TMember member, IReadOnlyCollection<IRule> rulesCollection, Error defaultError)
        {
            var errorsCollection = new ErrorsCollection() as IErrorsCollection;

            var rule = rulesCollection.Single();

            var compilationContext = new RulesExecutionContext
            {
                RulesOptions = new RulesOptionsStub
                {
                    DefaultError = defaultError
                },
                ValidationStrategy = ValidationStrategy.Complete
            };

            if (rule is ValidRule<TMember> validateRule)
            {
                validateRule.TryGetErrors(member, compilationContext, out errorsCollection);
            }
            else if (rule is ValidNullableRule validateNullableRule)
            {
                validateNullableRule.TryGetErrors(new object(), member, compilationContext, out errorsCollection);
            }

            return errorsCollection;
        }

        public static void AssertErrorCompilation<TMember>(TMember member, IReadOnlyCollection<IRule> rulesCollection, bool expectedIsValid, string expectedErrorMessage = null)
        {
            var errorsCollection = CompileSingleError(member, rulesCollection, _rulesOptions.DefaultError);

            if (!expectedIsValid)
            {
                Assert.False(errorsCollection.IsEmpty);
                var error = errorsCollection.Errors.Single();
                Assert.Equal(expectedErrorMessage, error.Message);
            }
            else
            {
                Assert.True((errorsCollection == null) || errorsCollection.IsEmpty);
            }
        }

        public static void AssertErrorMessage<TMember>(TMember member, IReadOnlyCollection<IRule> rulesCollection, string expectedErrorMessage, string expectedStringifiedErrorMessage)
        {
            var errorsCollection = CompileSingleError(member, rulesCollection, _rulesOptions.DefaultError);

            Assert.False(errorsCollection.IsEmpty);

            var singleError = errorsCollection.Errors.Single();
            Assert.Equal(expectedErrorMessage, singleError.Message);
            Assert.Equal(expectedStringifiedErrorMessage, singleError.StringifiedMessage);
        }

        public static IEnumerable<object[]> GetSetsCompilation(params IEnumerable<object[]>[] sets)
        {
            return sets.SelectMany(s => s);
        }
    }
}