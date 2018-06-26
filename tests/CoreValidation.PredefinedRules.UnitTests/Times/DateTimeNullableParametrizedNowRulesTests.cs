using System;
using System.Collections.Generic;
using CoreValidation.Specifications;
using CoreValidation.Translations;
using Xunit;

namespace CoreValidation.PredefinedRules.UnitTests.Times
{
    // ReSharper disable once InconsistentNaming
    public class DateTimeNullableParametrizedNowRulesTests
    {
        private static readonly Func<int, DateTime> _convert = ticks => new DateTime(ticks);

        public static IEnumerable<object[]> ParametrizedAfterNow_Should_CollectError_MemberData()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterThan_Unsigned(_convert),
                NumberDataHelper.GreaterThan_Limits(DateTime.MinValue, DateTime.MaxValue, new DateTime(1))
            );
        }

        [Theory]
        [MemberData(nameof(ParametrizedAfterNow_Should_CollectError_MemberData))]
        public void ParametrizedAfterNow_Should_CollectError(DateTime model, DateTime now, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, DateTime?>();

            builder.ParametrizedAfterNow(now);

            RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.AfterNow);
        }

        public static IEnumerable<object[]> ParametrizedBeforeNow_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessThan_Unsigned(_convert),
                NumberDataHelper.LessThan_Limits(DateTime.MinValue, DateTime.MaxValue, new DateTime(1))
            );
        }

        [Theory]
        [MemberData(nameof(ParametrizedBeforeNow_Should_CollectError_Data))]
        public void ParametrizedBeforeNow_Should_CollectError(DateTime model, DateTime now, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, DateTime?>();

            builder.ParametrizedBeforeNow(now);

            RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.BeforeNow);
        }

        public static IEnumerable<object[]> ParametrizedFromNow_Should_CollectError_Data()
        {
            yield return new object[] {new DateTime(10), new DateTime(10), new TimeSpan(2), true};
            yield return new object[] {new DateTime(10), new DateTime(11), new TimeSpan(2), true};
            yield return new object[] {new DateTime(10), new DateTime(12), new TimeSpan(2), true};
            yield return new object[] {new DateTime(10), new DateTime(13), new TimeSpan(2), false};

            yield return new object[] {new DateTime(10), new DateTime(10), new TimeSpan(-2), true};
            yield return new object[] {new DateTime(10), new DateTime(9), new TimeSpan(-2), true};
            yield return new object[] {new DateTime(10), new DateTime(8), new TimeSpan(-2), true};
            yield return new object[] {new DateTime(10), new DateTime(7), new TimeSpan(-2), false};
        }

        [Theory]
        [MemberData(nameof(ParametrizedFromNow_Should_CollectError_Data))]
        public void ParametrizedFromNow_Should_CollectError(DateTime now, DateTime model, TimeSpan value, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, DateTime?>();

            builder.ParametrizedFromNow(now, value);

            RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.FromNow);
        }

        public class MessageTests
        {
            [Fact]
            public void ParametrizedAfterNow_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, DateTime?>();

                builder.ParametrizedAfterNow(new DateTime(2020, 09, 21, 15, 0, 2), message: "{now} {timeComparison} Overriden error message");

                RulesHelper.AssertErrorMessage(new DateTime(2020, 09, 21, 15, 0, 1), builder.Rules, "{now} {timeComparison} Overriden error message", "2020-09-21 15:00:02 All Overriden error message");
            }

            [Fact]
            public void ParametrizedBeforeNow_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, DateTime?>();

                builder.ParametrizedBeforeNow(new DateTime(2020, 09, 21, 15, 0, 1), message: "{now} {timeComparison} Overriden error message");

                RulesHelper.AssertErrorMessage(new DateTime(2020, 09, 21, 15, 0, 2), builder.Rules, "{now} {timeComparison} Overriden error message", "2020-09-21 15:00:01 All Overriden error message");
            }

            [Fact]
            public void ParametrizedFromNow_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, DateTime?>();

                builder.ParametrizedFromNow(new DateTime(2020, 09, 21, 15, 0, 7), TimeSpan.FromSeconds(2), "{now} {timeSpan} Overriden error message");

                RulesHelper.AssertErrorMessage(new DateTime(2020, 09, 21, 15, 0, 10), builder.Rules, "{now} {timeSpan} Overriden error message", "2020-09-21 15:00:07 00:00:02 Overriden error message");
            }
        }
    }
}