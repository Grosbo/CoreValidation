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

        public static ErrorsCollection CompileSingleError<TMember>(TMember member, IReadOnlyCollection<IRule> rulesCollection, Error defaultError)
        {
            var errorsCollection = new ErrorsCollection();

            var rule = rulesCollection.Single();

            if (rule is ValidRule<TMember> validateRule)
            {
                var error = validateRule.CompileError(member, ValidationStrategy.Complete, defaultError);

                if (error != null)
                {
                    errorsCollection.AddError(error);
                }
            }
            else if (rule is ValidNullableRule)
            {
                errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    member,
                    ValidationStrategy.Complete,
                    _rulesOptions.DefaultError
                });
            }

            return errorsCollection;
        }

        public static void AssertErrorCompilation<TMember>(TMember member, IReadOnlyCollection<IRule> rulesCollection, bool expectedIsValid, string expectedErrorMessage = null)
        {
            var errorsCollection = CompileSingleError(member, rulesCollection, _rulesOptions.DefaultError);

            Assert.Equal(expectedIsValid, errorsCollection.IsEmpty);

            if (!errorsCollection.IsEmpty)
            {
                var error = errorsCollection.Errors.Single();
                Assert.Equal(expectedErrorMessage, error.Message);
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