using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Exceptions;
using CoreValidation.Results.List;
using CoreValidation.Translations;
using Xunit;

namespace CoreValidation.UnitTests.Results.List
{
    public class ToListReportExtensionTests
    {
        internal static void ExpectMessagesInReport(ListReport messagesReport, string path, IReadOnlyCollection<string> expectedMessages)
        {
            Assert.NotNull(messagesReport);

            var messages = string.IsNullOrEmpty(path)
                ? messagesReport
                    .Where(item => !item.Contains(':'))
                    .ToArray()
                : messagesReport
                    .Where(item => item.StartsWith($"{path}:"))
                    .Select(item => item.Split(": ", StringSplitOptions.RemoveEmptyEntries).ElementAtOrDefault(1))
                    .ToArray();

            Assert.Equal(expectedMessages.Count(), messages.Count());

            for (var i = 0; i < expectedMessages.Count(); ++i)
            {
                Assert.Equal(expectedMessages.ElementAt(i), messages.ElementAt(i));
            }
        }

        internal static void ExpectMembersInReport(ListReport messagesReport, string path, IReadOnlyCollection<string> expectedMembers)
        {
            Assert.NotNull(messagesReport);

            var pathParts = path == string.Empty ? Enumerable.Empty<string>() : path.Split('.');

            var reportLines = messagesReport.Where(item => (path == string.Empty) || (item.StartsWith(path) && item.Contains(':'))).ToArray();

            reportLines = reportLines
                .Where(item => item.Contains(':'))
                .Select(item => item.Substring(0, item.IndexOf(':')))
                .ToArray();

            var groups = reportLines.Select(item => item.Split('.')).ToArray();

            groups = groups.Where(g => g.Count() >= (pathParts.Count() + 1)).ToArray();

            var members = groups.Select(g => g.ElementAt(pathParts.Count())).Distinct().ToArray();

            Assert.Equal(expectedMembers.Count(), members.Length);

            for (var i = 0; i < expectedMembers.Count(); ++i)
            {
                Assert.Equal(expectedMembers.ElementAt(i), members.ElementAt(i));
            }
        }


        [Theory]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        public void ToListReport_Should_LimitDepth(int maxDepth, bool expectException)
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
                Assert.Throws<MaxDepthExceededException>(() => { result.ToListReport(); });
            }
            else
            {
                result.ToListReport();
            }
        }


        public class Translations
        {
            [Fact]
            public void ToListReport_Should_ThrowException_When_InvalidTranslationName()
            {
                var errorsCollection = new ErrorsCollection();

                var nestedErrorsCollection = new ErrorsCollection();

                nestedErrorsCollection.AddError(new Error("test123"));
                nestedErrorsCollection.AddError(new Error("test321"));
                nestedErrorsCollection.AddError("inner", new Error("foo"));
                nestedErrorsCollection.AddError("inner", new Error("bar"));

                errorsCollection.AddError("test", nestedErrorsCollection);

                var report = ResultsTestHelpers.MockValidationResult(errorsCollection, translations: new[]
                {
                    new Translation("test", new Dictionary<string, string> {{"foo", "FOO"}, {"test123", "TEST123"}}),
                    new Translation("test1", new Dictionary<string, string> {{"bar", "BAR"}, {"test321", "TEST321"}})
                });

                Assert.Throws<TranslationNotFoundException>(() => { report.ToListReport("test_non_existing"); });
            }

            [Fact]
            public void ToListReport_Should_Translate()
            {
                var errorsCollection = new ErrorsCollection();

                var nestedErrorsCollection = new ErrorsCollection();

                nestedErrorsCollection.AddError(new Error("test123"));
                nestedErrorsCollection.AddError(new Error("test321"));
                nestedErrorsCollection.AddError("inner", new Error("foo"));
                nestedErrorsCollection.AddError("inner", new Error("bar"));

                errorsCollection.AddError("test", nestedErrorsCollection);

                var report = ResultsTestHelpers.MockValidationResult(errorsCollection, translations: new[]
                {
                    new Translation("test", new Dictionary<string, string> {{"foo", "FOO"}, {"test123", "TEST123"}}),
                    new Translation("test1", new Dictionary<string, string> {{"bar", "BAR"}, {"test321", "TEST321"}})
                }).ToListReport("test");

                ExpectMembersInReport(report, "", new[] {"test"});

                ExpectMessagesInReport(report, "test", new[] {"TEST123", "test321"});

                ExpectMembersInReport(report, "test", new[] {"inner"});

                ExpectMessagesInReport(report, "test.inner", new[] {"FOO", "bar"});
            }
        }

        [Fact]
        public void ToListReport_Should_Generate_MemberMessages()
        {
            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError("test", new Error("test123"));
            errorsCollection.AddError("test", new Error("test321"));
            errorsCollection.AddError("test", new Error("foo"));
            errorsCollection.AddError("test", new Error("bar"));

            var report = ResultsTestHelpers.MockValidationResult(errorsCollection).ToListReport();

            ExpectMembersInReport(report, "", new[] {"test"});

            ExpectMessagesInReport(report, "test", new[] {"test123", "test321", "foo", "bar"});
        }

        [Fact]
        public void ToListReport_Should_Generate_NestedLevel()
        {
            var errorsCollection = new ErrorsCollection();

            var nestedErrorsCollection = new ErrorsCollection();

            nestedErrorsCollection.AddError(new Error("test123"));
            nestedErrorsCollection.AddError(new Error("test321"));
            nestedErrorsCollection.AddError("inner", new Error("foo"));
            nestedErrorsCollection.AddError("inner", new Error("bar"));

            errorsCollection.AddError("test", nestedErrorsCollection);

            var report = ResultsTestHelpers.MockValidationResult(errorsCollection).ToListReport();

            ExpectMembersInReport(report, "", new[] {"test"});

            ExpectMessagesInReport(report, "test", new[] {"test123", "test321"});

            ExpectMembersInReport(report, "test", new[] {"inner"});

            ExpectMessagesInReport(report, "test.inner", new[] {"foo", "bar"});
        }

        [Fact]
        public void ToListReport_Should_Generate_RootMessages()
        {
            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError(new Error("test123"));
            errorsCollection.AddError(new Error("test321"));
            errorsCollection.AddError(new Error("foo"));
            errorsCollection.AddError(new Error("bar"));

            var report = ResultsTestHelpers.MockValidationResult(errorsCollection).ToListReport();

            ExpectMessagesInReport(report, "", new[] {"test123", "test321", "foo", "bar"});
        }

        [Fact]
        public void ToListReport_Should_Generate_RootMessages_And_MemberMessages()
        {
            var errorsCollection = new ErrorsCollection();

            errorsCollection.AddError("test", new Error("test123"));
            errorsCollection.AddError("test", new Error("test321"));
            errorsCollection.AddError(new Error("foo"));
            errorsCollection.AddError(new Error("bar"));

            var report = ResultsTestHelpers.MockValidationResult(errorsCollection).ToListReport();

            ExpectMembersInReport(report, "", new[] {"test"});
            ExpectMessagesInReport(report, "test", new[] {"test123", "test321"});

            ExpectMessagesInReport(report, "", new[] {"foo", "bar"});
        }
    }
}