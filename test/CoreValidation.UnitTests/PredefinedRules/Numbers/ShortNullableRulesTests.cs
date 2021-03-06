using System;
using System.Collections.Generic;
using CoreValidation.Tests;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Numbers
{
    public class ShortNullableRulesTests
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
        public void EqualTo_Should_CollectError(short memberValue, short argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<short?>(
                m => m.EqualTo(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.EqualTo,
                new[]
                {
                    Arg.Number("value", argValue)
                });
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
        public void NotEqualTo_Should_CollectError(short memberValue, short argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<short?>(
                m => m.NotEqualTo(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.NotEqualTo,
                new[]
                {
                    Arg.Number("value", argValue)
                });
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
        public void GreaterThan_Should_CollectError(short memberValue, short min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<short?>(
                m => m.GreaterThan(min),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.GreaterThan,
                new[]
                {
                    Arg.Number("min", min)
                });
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
        public void GreaterOrEqualTo_Should_CollectError(short memberValue, short min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<short?>(
                m => m.GreaterOrEqualTo(min),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.GreaterOrEqualTo,
                new[]
                {
                    Arg.Number("min", min)
                });
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
        public void LessThan_Should_CollectError(short memberValue, short max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<short?>(
                m => m.LessThan(max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.LessThan,
                new[]
                {
                    Arg.Number("max", max)
                });
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
        public void LessOrEqualTo_Should_CollectError(short memberValue, short max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<short?>(
                m => m.LessOrEqualTo(max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.LessOrEqualTo,
                new[]
                {
                    Arg.Number("max", max)
                });
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
        public void Between_Should_CollectError(short min, short memberValue, short max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<short?>(
                m => m.Between(min, max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.Between,
                new[]
                {
                    Arg.Number("min", min),
                    Arg.Number("max", max)
                });
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
        public void BetweenOrEqualTo_Should_CollectError(short min, short memberValue, short max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<short?>(
                m => m.BetweenOrEqualTo(min, max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.BetweenOrEqualTo,
                new[]
                {
                    Arg.Number("min", min),
                    Arg.Number("max", max)
                });
        }
    }
}