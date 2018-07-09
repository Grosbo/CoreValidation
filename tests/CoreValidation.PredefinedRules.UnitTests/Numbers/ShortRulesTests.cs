﻿using System;
using System.Collections.Generic;
using CoreValidation.Specifications;
using CoreValidation.Translations;
using Xunit;

namespace CoreValidation.PredefinedRules.UnitTests.Numbers
{
    public class ShortRulesTests
    {
        private static readonly Func<int, short> _convert = Convert.ToInt16;

        public static IEnumerable<object[]> EqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.EqualTo_Unsigned(_convert),
                NumberDataHelper.EqualTo_Signed(_convert),
                NumberDataHelper.EqualTo_Limits(short.MinValue, short.MaxValue, 1)
            );
        }

        [Theory]
        [MemberData(nameof(EqualTo_Should_CollectError_Data))]
        public void EqualTo_Should_CollectError(short model, short value, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, short>();

            builder.EqualTo(value);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.EqualTo);
        }

        public static IEnumerable<object[]> NotEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.NotEqualTo_Unsigned(_convert),
                NumberDataHelper.NotEqualTo_Signed(_convert),
                NumberDataHelper.NotEqualTo_Limits(short.MinValue, short.MaxValue, 1)
            );
        }

        [Theory]
        [MemberData(nameof(NotEqualTo_Should_CollectError_Data))]
        public void NotEqualTo_Should_CollectError(short model, short value, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, short>();

            builder.NotEqualTo(value);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.NotEqualTo);
        }

        public static IEnumerable<object[]> GreaterThan_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterThan_Unsigned(_convert),
                NumberDataHelper.GreaterThan_Signed(_convert),
                NumberDataHelper.GreaterThan_Limits(short.MinValue, short.MaxValue, 1)
            );
        }

        [Theory]
        [MemberData(nameof(GreaterThan_Should_CollectError_Data))]
        public void GreaterThan_Should_CollectError(short model, short min, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, short>();

            builder.GreaterThan(min);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.GreaterThan);
        }

        public static IEnumerable<object[]> GreaterOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterOrEqualTo_Unsigned(_convert),
                NumberDataHelper.GreaterOrEqualTo_Signed(_convert),
                NumberDataHelper.GreaterOrEqualTo_Limits(short.MinValue, short.MaxValue, 1)
            );
        }

        [Theory]
        [MemberData(nameof(GreaterOrEqualTo_Should_CollectError_Data))]
        public void GreaterOrEqualTo_Should_CollectError(short model, short min, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, short>();

            builder.GreaterOrEqualTo(min);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.GreaterOrEqualTo);
        }

        public static IEnumerable<object[]> LessThan_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessThan_Unsigned(_convert),
                NumberDataHelper.LessThan_Signed(_convert),
                NumberDataHelper.LessThan_Limits(short.MinValue, short.MaxValue, 1)
            );
        }

        [Theory]
        [MemberData(nameof(LessThan_Should_CollectError_Data))]
        public void LessThan_Should_CollectError(short model, short max, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, short>();

            builder.LessThan(max);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.LessThan);
        }

        public static IEnumerable<object[]> LessOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessOrEqualTo_Unsigned(_convert),
                NumberDataHelper.LessOrEqualTo_Signed(_convert),
                NumberDataHelper.LessOrEqualTo_Limits(short.MinValue, short.MaxValue, 1)
            );
        }

        [Theory]
        [MemberData(nameof(LessOrEqualTo_Should_CollectError_Data))]
        public void LessOrEqualTo_Should_CollectError(short model, short max, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, short>();

            builder.LessOrEqualTo(max);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.LessOrEqualTo);
        }

        public static IEnumerable<object[]> Between_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.Between_Unsigned(_convert),
                NumberDataHelper.Between_Signed(_convert),
                NumberDataHelper.Between_Limits(short.MinValue, short.MaxValue, 1)
            );
        }

        [Theory]
        [MemberData(nameof(Between_Should_CollectError_Data))]
        public void Between_Should_CollectError(short min, short model, short max, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, short>();

            builder.Between(min, max);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.Between);
        }

        public static IEnumerable<object[]> BetweenOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.BetweenOrEqualTo_Unsigned(_convert),
                NumberDataHelper.BetweenOrEqualTo_Signed(_convert),
                NumberDataHelper.BetweenOrEqualTo_Limits(short.MinValue, short.MaxValue, 1)
            );
        }

        [Theory]
        [MemberData(nameof(BetweenOrEqualTo_Should_CollectError_Data))]
        public void BetweenOrEqualTo_Should_CollectError(short min, short model, short max, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, short>();

            builder.BetweenOrEqualTo(min, max);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.BetweenOrEqualTo);
        }

        public class MessageTests
        {
            [Fact]
            public void Between_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, short>();

                builder.Between(1, 3, "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage<short>(4, builder.Rules, "{min} {max} Overriden error message", "1 3 Overriden error message");
            }

            [Fact]
            public void BetweenOrEqualTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, short>();

                builder.BetweenOrEqualTo(1, 3, "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage<short>(4, builder.Rules, "{min} {max} Overriden error message", "1 3 Overriden error message");
            }

            [Fact]
            public void EqualTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, short>();

                builder.EqualTo(0, "{value} Overriden error message");

                RulesHelper.AssertErrorMessage<short>(4, builder.Rules, "{value} Overriden error message", "0 Overriden error message");
            }

            [Fact]
            public void GreaterOrEqual_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, short>();

                builder.GreaterOrEqualTo(2, "{min} Overriden error message");

                RulesHelper.AssertErrorMessage<short>(1, builder.Rules, "{min} Overriden error message", "2 Overriden error message");
            }

            [Fact]
            public void GreaterThan_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, short>();

                builder.GreaterThan(2, "{min} Overriden error message");

                RulesHelper.AssertErrorMessage<short>(1, builder.Rules, "{min} Overriden error message", "2 Overriden error message");
            }

            [Fact]
            public void LessThan_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, short>();

                builder.LessThan(1, "{max} Overriden error message");

                RulesHelper.AssertErrorMessage<short>(2, builder.Rules, "{max} Overriden error message", "1 Overriden error message");
            }

            [Fact]
            public void LessThanOrEqual_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, short>();

                builder.LessOrEqualTo(1, "{max} Overriden error message");

                RulesHelper.AssertErrorMessage<short>(2, builder.Rules, "{max} Overriden error message", "1 Overriden error message");
            }

            [Fact]
            public void NotEqualTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, short>();

                builder.NotEqualTo(4, "{value} Overriden error message");

                RulesHelper.AssertErrorMessage<short>(4, builder.Rules, "{value} Overriden error message", "4 Overriden error message");
            }
        }
    }
}