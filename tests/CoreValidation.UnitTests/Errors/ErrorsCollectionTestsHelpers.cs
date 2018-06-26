using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using Xunit;

namespace CoreValidation.UnitTests.Errors
{
    internal static class ErrorsCollectionTestsHelpers
    {
        public static void ExpectMembers(IErrorsCollection errorsCollection, IReadOnlyCollection<string> membersNames)
        {
            Assert.NotNull(errorsCollection.Members);
            Assert.Equal(membersNames.Count(), errorsCollection.Members.Count);

            for (var i = 0; i < membersNames.Count(); ++i)
            {
                Assert.Equal(membersNames.ElementAt(i), errorsCollection.Members.Keys.ElementAt(i));
                Assert.NotNull(errorsCollection.Members[membersNames.ElementAt(i)]);
            }
        }

        public static void ExpectErrors(IErrorsCollection errorsCollection, IReadOnlyCollection<string> errors)
        {
            Assert.NotNull(errorsCollection.Errors);
            Assert.Equal(errors.Count(), errorsCollection.Errors.Count());

            for (var i = 0; i < errors.Count(); ++i)
            {
                Assert.Equal(errors.ElementAt(i), errorsCollection.Errors.ElementAt(i).StringifiedMessage);
            }
        }
    }
}