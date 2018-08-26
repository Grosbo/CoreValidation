using System;
using System.Collections.Generic;
using CoreValidation.Specifications;
using CoreValidation.Translations;
using Xunit;

namespace CoreValidation.PredefinedRules.UnitTests.Numbers
{
    public class UShortNullableRulesTests
    {
        private static readonly Func<int, ushort> _convert = Convert.ToUInt16;

        public static IEnumerable<object[]> EqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.EqualTo_Unsigned(_convert),
                NumberDataHelper.EqualTo_Limits(ushort.MinValue, ushort.MaxValue, 1U)
            );
        }

        [Theory]
        [MemberData(nameof(EqualTo_Should_CollectError_Data))]
        public void EqualTo_Should_CollectError(ushort model, ushort value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, ushort?>();

            builder.EqualTo(value);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.EqualTo);
        }

        public static IEnumerable<object[]> NotEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.NotEqualTo_Unsigned(_convert),
                NumberDataHelper.NotEqualTo_Limits(ushort.MinValue, ushort.MaxValue, 1U)
            );
        }

        [Theory]
        [MemberData(nameof(NotEqualTo_Should_CollectError_Data))]
        public void NotEqualTo_Should_CollectError(ushort model, ushort value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, ushort?>();

            builder.NotEqualTo(value);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.NotEqualTo);
        }

        public static IEnumerable<object[]> GreaterThan_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterThan_Unsigned(_convert),
                NumberDataHelper.GreaterThan_Limits(ushort.MinValue, ushort.MaxValue, 1U)
            );
        }

        [Theory]
        [MemberData(nameof(GreaterThan_Should_CollectError_Data))]
        public void GreaterThan_Should_CollectError(ushort model, ushort min, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, ushort?>();

            builder.GreaterThan(min);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.GreaterThan);
        }

        public static IEnumerable<object[]> GreaterOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterOrEqualTo_Unsigned(_convert),
                NumberDataHelper.GreaterOrEqualTo_Limits(ushort.MinValue, ushort.MaxValue, 1U)
            );
        }

        [Theory]
        [MemberData(nameof(GreaterOrEqualTo_Should_CollectError_Data))]
        public void GreaterOrEqualTo_Should_CollectError(ushort model, ushort min, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, ushort?>();

            builder.GreaterOrEqualTo(min);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.GreaterOrEqualTo);
        }

        public static IEnumerable<object[]> LessThan_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessThan_Unsigned(_convert),
                NumberDataHelper.LessThan_Limits(ushort.MinValue, ushort.MaxValue, 1U)
            );
        }

        [Theory]
        [MemberData(nameof(LessThan_Should_CollectError_Data))]
        public void LessThan_Should_CollectError(ushort model, ushort max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, ushort?>();

            builder.LessThan(max);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.LessThan);
        }

        public static IEnumerable<object[]> LessOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessOrEqualTo_Unsigned(_convert),
                NumberDataHelper.LessOrEqualTo_Limits(ushort.MinValue, ushort.MaxValue, 1U)
            );
        }

        [Theory]
        [MemberData(nameof(LessOrEqualTo_Should_CollectError_Data))]
        public void LessOrEqualTo_Should_CollectError(ushort model, ushort max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, ushort?>();

            builder.LessOrEqualTo(max);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.LessOrEqualTo);
        }

        public static IEnumerable<object[]> Between_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.Between_Unsigned(_convert),
                NumberDataHelper.Between_Limits(ushort.MinValue, ushort.MaxValue, 1U)
            );
        }

        [Theory]
        [MemberData(nameof(Between_Should_CollectError_Data))]
        public void Between_Should_CollectError(ushort min, ushort model, ushort max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, ushort?>();

            builder.Between(min, max);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.Between);
        }

        public static IEnumerable<object[]> BetweenOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.BetweenOrEqualTo_Unsigned(_convert),
                NumberDataHelper.BetweenOrEqualTo_Limits(ushort.MinValue, ushort.MaxValue, 1U)
            );
        }

        [Theory]
        [MemberData(nameof(BetweenOrEqualTo_Should_CollectError_Data))]
        public void BetweenOrEqualTo_Should_CollectError(ushort min, ushort model, ushort max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, ushort?>();

            builder.BetweenOrEqualTo(min, max);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.BetweenOrEqualTo);
        }

        public class MessageTests
        {
            [Fact]
            public void Between_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, ushort?>();

                builder.Between(1, 3, "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage<ushort>(4, builder.Rules, "{min} {max} Overriden error message", "1 3 Overriden error message");
            }

            [Fact]
            public void BetweenOrEqualTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, ushort?>();

                builder.BetweenOrEqualTo(1, 3, "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage<ushort>(4, builder.Rules, "{min} {max} Overriden error message", "1 3 Overriden error message");
            }

            [Fact]
            public void EqualTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, ushort?>();

                builder.EqualTo(0, "{value} Overriden error message");

                RulesHelper.AssertErrorMessage<ushort>(4, builder.Rules, "{value} Overriden error message", "0 Overriden error message");
            }

            [Fact]
            public void GreaterOrEqual_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, ushort?>();

                builder.GreaterOrEqualTo(2, "{min} Overriden error message");

                RulesHelper.AssertErrorMessage<ushort>(1, builder.Rules, "{min} Overriden error message", "2 Overriden error message");
            }

            [Fact]
            public void GreaterThan_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, ushort?>();

                builder.GreaterThan(2, "{min} Overriden error message");

                RulesHelper.AssertErrorMessage<ushort>(1, builder.Rules, "{min} Overriden error message", "2 Overriden error message");
            }

            [Fact]
            public void LessThan_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, ushort?>();

                builder.LessThan(1, "{max} Overriden error message");

                RulesHelper.AssertErrorMessage<ushort>(2, builder.Rules, "{max} Overriden error message", "1 Overriden error message");
            }

            [Fact]
            public void LessThanOrEqual_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, ushort?>();

                builder.LessOrEqualTo(1, "{max} Overriden error message");

                RulesHelper.AssertErrorMessage<ushort>(2, builder.Rules, "{max} Overriden error message", "1 Overriden error message");
            }

            [Fact]
            public void NotEqualTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, ushort?>();

                builder.NotEqualTo(4, "{value} Overriden error message");

                RulesHelper.AssertErrorMessage<ushort>(4, builder.Rules, "{value} Overriden error message", "4 Overriden error message");
            }
        }
    }
}