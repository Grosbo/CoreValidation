using System;
using System.Collections.Generic;
using CoreValidation.Errors.Args;
using CoreValidation.Tests;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Numbers
{
    public class UShortRulesTests
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
        public void EqualTo_Should_CollectError(ushort memberValue, ushort argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.EqualTo(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.EqualTo,
                new IMessageArg[]
                {
                    NumberArg.Create("value", argValue)
                });
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
        public void NotEqualTo_Should_CollectError(ushort memberValue, ushort argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.NotEqualTo(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.NotEqualTo,
                new IMessageArg[]
                {
                    NumberArg.Create("value", argValue)
                });
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
        public void GreaterThan_Should_CollectError(ushort memberValue, ushort min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.GreaterThan(min),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.GreaterThan,
                new IMessageArg[]
                {
                    NumberArg.Create("min", min)
                });
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
        public void GreaterOrEqualTo_Should_CollectError(ushort memberValue, ushort min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.GreaterOrEqualTo(min),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.GreaterOrEqualTo,
                new IMessageArg[]
                {
                    NumberArg.Create("min", min)
                });
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
        public void LessThan_Should_CollectError(ushort memberValue, ushort max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.LessThan(max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.LessThan,
                new IMessageArg[]
                {
                    NumberArg.Create("max", max)
                });
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
        public void LessOrEqualTo_Should_CollectError(ushort memberValue, ushort max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.LessOrEqualTo(max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.LessOrEqualTo,
                new IMessageArg[]
                {
                    NumberArg.Create("max", max)
                });
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
        public void Between_Should_CollectError(ushort min, ushort memberValue, ushort max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.Between(min, max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.Between,
                new IMessageArg[]
                {
                    NumberArg.Create("min", min),
                    NumberArg.Create("max", max)
                });
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
        public void BetweenOrEqualTo_Should_CollectError(ushort min, ushort memberValue, ushort max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.BetweenOrEqualTo(min, max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.BetweenOrEqualTo,
                new IMessageArg[]
                {
                    NumberArg.Create("min", min),
                    NumberArg.Create("max", max)
                });
        }
    }
}