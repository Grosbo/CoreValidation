using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Options;
using CoreValidation.Specifications.Commands;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules
{
    internal static class RulesHelper
    {
        private static readonly IExecutionOptions _executionOptions = new ExecutionContextStub();

        public static IErrorsCollection CompileSingleError<TMember>(TMember member, IReadOnlyCollection<IRule> rulesCollection, IError defaultError)
        {
            var errorsCollection = new ErrorsCollection() as IErrorsCollection;

            var rule = rulesCollection.Single();

            var compilationContext = new ExecutionContextStub
            {
                DefaultError = defaultError
            };

            if (rule is ValidRule<TMember> validateRule)
            {
                validateRule.TryGetErrors(member, compilationContext, ValidationStrategy.Complete, out errorsCollection);
            }
            else if (rule is AsNullableRule validateNullableRule)
            {
                validateNullableRule.TryGetErrors(new object(), member, compilationContext, ValidationStrategy.Complete, out errorsCollection);
            }

            return errorsCollection;
        }

        public static void AssertErrorCompilation<TMember>(TMember member, IReadOnlyCollection<IRule> rulesCollection, bool expectedIsValid, string expectedErrorMessage = null)
        {
            var errorsCollection = CompileSingleError(member, rulesCollection, _executionOptions.DefaultError);

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
            var errorsCollection = CompileSingleError(member, rulesCollection, _executionOptions.DefaultError);

            Assert.False(errorsCollection.IsEmpty);

            var singleError = errorsCollection.Errors.Single();
            Assert.Equal(expectedErrorMessage, singleError.Message);
            Assert.Equal(expectedStringifiedErrorMessage, singleError.ToFormattedMessage());
        }

        public static IEnumerable<object[]> GetSetsCompilation(params IEnumerable<object[]>[] sets)
        {
            return sets.SelectMany(s => s);
        }
    }
}