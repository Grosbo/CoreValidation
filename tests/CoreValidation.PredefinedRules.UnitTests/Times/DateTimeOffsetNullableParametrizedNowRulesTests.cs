using System;
using System.Collections.Generic;
using CoreValidation.Specifications;
using CoreValidation.Translations;
using Xunit;

namespace CoreValidation.PredefinedRules.UnitTests.Times
{
    public class DateTimeOffsetNullableParametrizedNowRulesTests
    {
        private static readonly Func<int, DateTimeOffset> _convert = ticks => new DateTimeOffset(ticks, TimeSpan.Zero);

        public static IEnumerable<object[]> ParametrizedAfterNow_Should_CollectError_MemberData()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterThan_Unsigned(_convert),
                NumberDataHelper.GreaterThan_Limits(DateTimeOffset.MinValue, DateTimeOffset.MaxValue, new DateTimeOffset(1, TimeSpan.Zero))
            );
        }

        [Theory]
        [MemberData(nameof(ParametrizedAfterNow_Should_CollectError_MemberData))]
        public void ParametrizedAfterNow_Should_CollectError(DateTimeOffset model, DateTimeOffset now, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, DateTimeOffset?>();

            builder.ParametrizedAfterNow(now);

            RulesHelper.AssertErrorCompilation<DateTimeOffset?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.AfterNow);
        }

        public static IEnumerable<object[]> ParametrizedBeforeNow_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessThan_Unsigned(_convert),
                NumberDataHelper.LessThan_Limits(DateTimeOffset.MinValue, DateTimeOffset.MaxValue, new DateTimeOffset(1, TimeSpan.Zero))
            );
        }

        [Theory]
        [MemberData(nameof(ParametrizedBeforeNow_Should_CollectError_Data))]
        public void ParametrizedBeforeNow_Should_CollectError(DateTimeOffset model, DateTimeOffset now, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, DateTimeOffset?>();

            builder.ParametrizedBeforeNow(now);

            RulesHelper.AssertErrorCompilation<DateTimeOffset?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.BeforeNow);
        }

        public static IEnumerable<object[]> ParametrizedFromNow_Should_CollectError_Data()
        {
            yield return new object[] {new DateTimeOffset(10, TimeSpan.Zero), new DateTimeOffset(10, TimeSpan.Zero), new TimeSpan(2), true};
            yield return new object[] {new DateTimeOffset(10, TimeSpan.Zero), new DateTimeOffset(11, TimeSpan.Zero), new TimeSpan(2), true};
            yield return new object[] {new DateTimeOffset(10, TimeSpan.Zero), new DateTimeOffset(12, TimeSpan.Zero), new TimeSpan(2), true};
            yield return new object[] {new DateTimeOffset(10, TimeSpan.Zero), new DateTimeOffset(13, TimeSpan.Zero), new TimeSpan(2), false};

            yield return new object[] {new DateTimeOffset(10, TimeSpan.Zero), new DateTimeOffset(10, TimeSpan.Zero), new TimeSpan(-2), true};
            yield return new object[] {new DateTimeOffset(10, TimeSpan.Zero), new DateTimeOffset(9, TimeSpan.Zero), new TimeSpan(-2), true};
            yield return new object[] {new DateTimeOffset(10, TimeSpan.Zero), new DateTimeOffset(8, TimeSpan.Zero), new TimeSpan(-2), true};
            yield return new object[] {new DateTimeOffset(10, TimeSpan.Zero), new DateTimeOffset(7, TimeSpan.Zero), new TimeSpan(-2), false};
        }

        [Theory]
        [MemberData(nameof(ParametrizedFromNow_Should_CollectError_Data))]
        public void ParametrizedFromNow_Should_CollectError(DateTimeOffset now, DateTimeOffset model, TimeSpan value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, DateTimeOffset?>();

            builder.ParametrizedFromNow(now, value);

            RulesHelper.AssertErrorCompilation<DateTimeOffset?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.FromNow);
        }

        public class MessageTests
        {
            [Fact]
            public void ParametrizedAfterNow_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, DateTimeOffset?>();

                builder.ParametrizedAfterNow(new DateTimeOffset(2020, 09, 21, 15, 0, 2, TimeSpan.Zero), message: "{now} {timeComparison} Overriden error message");

                RulesHelper.AssertErrorMessage(new DateTimeOffset(2020, 09, 21, 15, 0, 1, TimeSpan.Zero), builder.Rules, "{now} {timeComparison} Overriden error message", "2020-09-21 15:00:02 All Overriden error message");
            }

            [Fact]
            public void ParametrizedBeforeNow_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, DateTimeOffset?>();

                builder.ParametrizedBeforeNow(new DateTimeOffset(2020, 09, 21, 15, 0, 1, TimeSpan.Zero), message: "{now} {timeComparison} Overriden error message");

                RulesHelper.AssertErrorMessage(new DateTimeOffset(2020, 09, 21, 15, 0, 2, TimeSpan.Zero), builder.Rules, "{now} {timeComparison} Overriden error message", "2020-09-21 15:00:01 All Overriden error message");
            }

            [Fact]
            public void ParametrizedFromNow_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, DateTimeOffset?>();

                builder.ParametrizedFromNow(new DateTimeOffset(2020, 09, 21, 15, 0, 7, TimeSpan.Zero), TimeSpan.FromSeconds(2), "{now} {timeSpan} Overriden error message");

                RulesHelper.AssertErrorMessage(new DateTimeOffset(2020, 09, 21, 15, 0, 10, TimeSpan.Zero), builder.Rules, "{now} {timeSpan} Overriden error message", "2020-09-21 15:00:07 00:00:02 Overriden error message");
            }
        }
    }
}