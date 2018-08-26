using System;
using System.Collections.Generic;
using CoreValidation.Specifications;
using CoreValidation.Translations;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules
{
    public class TimeSpanRulesTests
    {
        private static readonly Func<int, TimeSpan> _convert = c => new TimeSpan(c);

        public static IEnumerable<object[]> EqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.EqualTo_Unsigned(_convert),
                NumberDataHelper.EqualTo_Signed(_convert),
                NumberDataHelper.EqualTo_Limits(TimeSpan.MinValue, TimeSpan.MaxValue, TimeSpan.Zero)
            );
        }

        [Theory]
        [MemberData(nameof(EqualTo_Should_CollectError_Data))]
        public void EqualTo_Should_CollectError(TimeSpan model, TimeSpan value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, TimeSpan>();

            builder.EqualTo(value);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.TimeSpan.EqualTo);
        }

        public static IEnumerable<object[]> NotEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.NotEqualTo_Unsigned(_convert),
                NumberDataHelper.NotEqualTo_Signed(_convert),
                NumberDataHelper.NotEqualTo_Limits(TimeSpan.MinValue, TimeSpan.MaxValue, TimeSpan.Zero)
            );
        }

        [Theory]
        [MemberData(nameof(NotEqualTo_Should_CollectError_Data))]
        public void NotEqualTo_Should_CollectError(TimeSpan model, TimeSpan value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, TimeSpan>();

            builder.NotEqualTo(value);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.TimeSpan.NotEqualTo);
        }

        public static IEnumerable<object[]> GreaterThan_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterThan_Unsigned(_convert),
                NumberDataHelper.GreaterThan_Signed(_convert),
                NumberDataHelper.GreaterThan_Limits(TimeSpan.MinValue, TimeSpan.MaxValue, TimeSpan.Zero)
            );
        }

        [Theory]
        [MemberData(nameof(GreaterThan_Should_CollectError_Data))]
        public void GreaterThan_Should_CollectError(TimeSpan model, TimeSpan min, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, TimeSpan>();

            builder.GreaterThan(min);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.TimeSpan.GreaterThan);
        }

        public static IEnumerable<object[]> GreaterOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterOrEqualTo_Unsigned(_convert),
                NumberDataHelper.GreaterOrEqualTo_Signed(_convert),
                NumberDataHelper.GreaterOrEqualTo_Limits(TimeSpan.MinValue, TimeSpan.MaxValue, TimeSpan.Zero)
            );
        }

        [Theory]
        [MemberData(nameof(GreaterOrEqualTo_Should_CollectError_Data))]
        public void GreaterOrEqualTo_Should_CollectError(TimeSpan model, TimeSpan min, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, TimeSpan>();

            builder.GreaterOrEqualTo(min);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.TimeSpan.GreaterOrEqualTo);
        }

        public static IEnumerable<object[]> LessThan_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessThan_Unsigned(_convert),
                NumberDataHelper.LessThan_Signed(_convert),
                NumberDataHelper.LessThan_Limits(TimeSpan.MinValue, TimeSpan.MaxValue, TimeSpan.Zero)
            );
        }

        [Theory]
        [MemberData(nameof(LessThan_Should_CollectError_Data))]
        public void LessThan_Should_CollectError(TimeSpan model, TimeSpan max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, TimeSpan>();

            builder.LessThan(max);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.TimeSpan.LessThan);
        }

        public static IEnumerable<object[]> LessOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessOrEqualTo_Unsigned(_convert),
                NumberDataHelper.LessOrEqualTo_Signed(_convert),
                NumberDataHelper.LessOrEqualTo_Limits(TimeSpan.MinValue, TimeSpan.MaxValue, TimeSpan.Zero)
            );
        }

        [Theory]
        [MemberData(nameof(LessOrEqualTo_Should_CollectError_Data))]
        public void LessOrEqualTo_Should_CollectError(TimeSpan model, TimeSpan max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, TimeSpan>();

            builder.LessOrEqualTo(max);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.TimeSpan.LessOrEqualTo);
        }

        public static IEnumerable<object[]> Between_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.Between_Unsigned(_convert),
                NumberDataHelper.Between_Signed(_convert),
                NumberDataHelper.Between_Limits(TimeSpan.MinValue, TimeSpan.MaxValue, TimeSpan.Zero)
            );
        }

        [Theory]
        [MemberData(nameof(Between_Should_CollectError_Data))]
        public void Between_Should_CollectError(TimeSpan min, TimeSpan model, TimeSpan max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, TimeSpan>();

            builder.Between(min, max);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.TimeSpan.Between);
        }

        public static IEnumerable<object[]> BetweenOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.BetweenOrEqualTo_Unsigned(_convert),
                NumberDataHelper.BetweenOrEqualTo_Signed(_convert),
                NumberDataHelper.BetweenOrEqualTo_Limits(TimeSpan.MinValue, TimeSpan.MaxValue, TimeSpan.Zero)
            );
        }

        [Theory]
        [MemberData(nameof(BetweenOrEqualTo_Should_CollectError_Data))]
        public void BetweenOrEqualTo_Should_CollectError(TimeSpan min, TimeSpan model, TimeSpan max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, TimeSpan>();

            builder.BetweenOrEqualTo(min, max);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.TimeSpan.BetweenOrEqualTo);
        }

        public class MessageTests
        {
            [Fact]
            public void Between_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, TimeSpan>();

                builder.Between(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(3), "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage(TimeSpan.FromMinutes(4), builder.Rules, "{min} {max} Overriden error message", "00:01:00 00:03:00 Overriden error message");
            }

            [Fact]
            public void BetweenOrEqualTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, TimeSpan>();

                builder.BetweenOrEqualTo(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(3), "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage(TimeSpan.FromMinutes(4), builder.Rules, "{min} {max} Overriden error message", "00:01:00 00:03:00 Overriden error message");
            }

            [Fact]
            public void EqualTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, TimeSpan>();

                builder.EqualTo(TimeSpan.FromMinutes(0), "{value} Overriden error message");

                RulesHelper.AssertErrorMessage(TimeSpan.FromMinutes(4), builder.Rules, "{value} Overriden error message", "00:00:00 Overriden error message");
            }

            [Fact]
            public void GreaterOrEqual_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, TimeSpan>();

                builder.GreaterOrEqualTo(TimeSpan.FromMinutes(2), "{min} Overriden error message");

                RulesHelper.AssertErrorMessage(TimeSpan.FromMinutes(1), builder.Rules, "{min} Overriden error message", "00:02:00 Overriden error message");
            }

            [Fact]
            public void GreaterThan_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, TimeSpan>();

                builder.GreaterThan(TimeSpan.FromMinutes(2), "{min} Overriden error message");

                RulesHelper.AssertErrorMessage(TimeSpan.FromMinutes(1), builder.Rules, "{min} Overriden error message", "00:02:00 Overriden error message");
            }

            [Fact]
            public void LessThan_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, TimeSpan>();

                builder.LessThan(TimeSpan.FromMinutes(1), "{max} Overriden error message");

                RulesHelper.AssertErrorMessage(TimeSpan.FromMinutes(2), builder.Rules, "{max} Overriden error message", "00:01:00 Overriden error message");
            }

            [Fact]
            public void LessThanOrEqual_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, TimeSpan>();

                builder.LessOrEqualTo(TimeSpan.FromMinutes(1), "{max} Overriden error message");

                RulesHelper.AssertErrorMessage(TimeSpan.FromMinutes(2), builder.Rules, "{max} Overriden error message", "00:01:00 Overriden error message");
            }

            [Fact]
            public void NotEqualTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, TimeSpan>();

                builder.NotEqualTo(TimeSpan.FromMinutes(4), "{value} Overriden error message");

                RulesHelper.AssertErrorMessage(TimeSpan.FromMinutes(4), builder.Rules, "{value} Overriden error message", "00:04:00 Overriden error message");
            }
        }
    }
}