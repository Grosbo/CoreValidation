using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Exceptions;
using CoreValidation.Results.Model;
using CoreValidation.Translations;
using Xunit;

namespace CoreValidation.UnitTests.Results.Reports.Model
{
    public class ToModelReportExtensionTests
    {
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        internal static void ExpectMembersInReport(ModelReport modelReport, IReadOnlyCollection<string> expectedMembers)
        {
            Assert.NotNull(modelReport);

            Assert.Equal(expectedMembers.Count(), modelReport.Count);

            for (var i = 0; i < expectedMembers.Count(); ++i)
            {
                Assert.Equal(expectedMembers.ElementAt(i), modelReport.Keys.ElementAt(i));
            }
        }

        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        internal static void ExpectMessagesInList(ModelReportErrorsList modelReportErrorsList, IReadOnlyCollection<string> expectedMessages)
        {
            Assert.NotNull(modelReportErrorsList);
            Assert.Equal(expectedMessages.Count(), modelReportErrorsList.Count);

            for (var i = 0; i < expectedMessages.Count(); ++i)
            {
                Assert.Equal(expectedMessages.ElementAt(i), modelReportErrorsList.ElementAt(i));
            }
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        public void ToModelReport_Should_LimitDepth(int maxDepth, bool expectException)
        {
            var level3 = new ErrorsCollection();
            level3.AddError(new Error("error"));

            var level2 = new ErrorsCollection();
            level2.AddError("member2", level3);

            var level1 = new ErrorsCollection();
            level1.AddError("member1", level2);

            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError("member", level1);

            var result = ResultsTestHelpers.MockValidationResult(errorsCollection, new RulesOptionsStub {MaxDepth = maxDepth, CollectionForceKey = "*", RequiredError = new Error("Required")});

            if (expectException)
            {
                Assert.Throws<MaxDepthExceededException>(() => { result.ToModelReport(); });
            }
            else
            {
                result.ToModelReport();
            }
        }

        public class Translations
        {
            [Fact]
            public void ToModelReport_Should_ThrowException_When_InvalidTranslationName()
            {
                var draft = new ErrorsCollection();

                draft.AddError(new Error("foo"));
                draft.AddError(new Error("bar"));

                var innerDraft = new ErrorsCollection();
                innerDraft.AddError("inner", new Error("test123"));
                innerDraft.AddError("inner", new Error("test321"));

                draft.AddError("test", innerDraft);

                var result = ResultsTestHelpers.MockValidationResult(draft, translations: new[]
                {
                    new Translation("test", new Dictionary<string, string> {{"foo", "FOO"}, {"test123", "TEST123"}}),
                    new Translation("test1", new Dictionary<string, string> {{"bar", "BAR"}, {"test321", "TEST321"}})
                });

                Assert.Throws<TranslationNotFoundException>(() => { result.ToModelReport("test_non_existing"); });
            }

            [Fact]
            public void ToModelReport_Should_Translate()
            {
                var draft = new ErrorsCollection();

                draft.AddError(new Error("foo"));
                draft.AddError(new Error("bar"));

                var innerDraft = new ErrorsCollection();
                innerDraft.AddError("inner", new Error("test123"));
                innerDraft.AddError("inner", new Error("test321"));

                draft.AddError("test", innerDraft);

                var report = ResultsTestHelpers.MockValidationResult(draft, translations: new[]
                {
                    new Translation("test", new Dictionary<string, string> {{"foo", "FOO"}, {"test123", "TEST123"}}),
                    new Translation("test1", new Dictionary<string, string> {{"bar", "BAR"}, {"test321", "TEST321"}})
                }).ToModelReport("test") as ModelReport;

                ExpectMembersInReport(report, new[] {string.Empty, "test"});

                // ReSharper disable once PossibleNullReferenceException
                ExpectMessagesInList(report[string.Empty] as ModelReportErrorsList, new[] {"FOO", "bar"});

                ExpectMembersInReport(report["test"] as ModelReport, new[] {"inner"});

                // ReSharper disable once PossibleNullReferenceException
                ExpectMessagesInList((report["test"] as ModelReport)["inner"] as ModelReportErrorsList, new[] {"TEST123", "test321"});
            }
        }

        [Fact]
        public void ToModelReport_Should_Generate_MemberMessages()
        {
            var draft = new ErrorsCollection();

            draft.AddError("test", new Error("test123"));
            draft.AddError("test", new Error("test321"));
            draft.AddError("test", new Error("foo"));
            draft.AddError("test", new Error("bar"));

            var report = (ModelReport)ResultsTestHelpers.MockValidationResult(draft).ToModelReport();

            ExpectMembersInReport(report, new[] {"test"});

            ExpectMessagesInList((ModelReportErrorsList)report["test"], new[] {"test123", "test321", "foo", "bar"});
        }

        [Fact]
        public void ToModelReport_Should_Generate_NestedLevel()
        {
            var draft = new ErrorsCollection();

            var innerDraft = new ErrorsCollection();
            innerDraft.AddError("inner", new Error("test123"));
            innerDraft.AddError("inner", new Error("test321"));
            innerDraft.AddError("inner", new Error("foo"));
            innerDraft.AddError("inner", new Error("bar"));

            draft.AddError("test", innerDraft);

            var report = ResultsTestHelpers.MockValidationResult(draft).ToModelReport() as ModelReport;

            ExpectMembersInReport(report, new[] {"test"});

            // ReSharper disable once PossibleNullReferenceException
            ExpectMembersInReport(report["test"] as ModelReport, new[] {"inner"});

            // ReSharper disable once PossibleNullReferenceException
            ExpectMessagesInList((report["test"] as ModelReport)["inner"] as ModelReportErrorsList, new[] {"test123", "test321", "foo", "bar"});
        }

        [Fact]
        public void ToModelReport_Should_Generate_RootMessages()
        {
            var draft = new ErrorsCollection();

            draft.AddError(new Error("test123"));
            draft.AddError(new Error("test321"));
            draft.AddError(new Error("foo"));
            draft.AddError(new Error("bar"));

            var report = ResultsTestHelpers.MockValidationResult(draft).ToModelReport() as ModelReportErrorsList;

            ExpectMessagesInList(report, new[] {"test123", "test321", "foo", "bar"});
        }

        [Fact]
        public void ToModelReport_Should_Generate_RootMessages_And_MemberMessages()
        {
            var draft = new ErrorsCollection();

            draft.AddError(new Error("foo"));
            draft.AddError(new Error("bar"));

            var innerDraft = new ErrorsCollection();
            innerDraft.AddError("inner", new Error("test123"));
            innerDraft.AddError("inner", new Error("test321"));

            draft.AddError("test", innerDraft);

            var report = ResultsTestHelpers.MockValidationResult(draft).ToModelReport() as ModelReport;

            ExpectMembersInReport(report, new[] {string.Empty, "test"});

            // ReSharper disable once PossibleNullReferenceException
            ExpectMessagesInList(report[string.Empty] as ModelReportErrorsList, new[] {"foo", "bar"});

            ExpectMembersInReport(report["test"] as ModelReport, new[] {"inner"});

            // ReSharper disable once PossibleNullReferenceException
            ExpectMessagesInList((report["test"] as ModelReport)["inner"] as ModelReportErrorsList, new[] {"test123", "test321"});
        }
    }
}