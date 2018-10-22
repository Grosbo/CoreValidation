using System;
using System.Collections.Generic;
using CoreValidation.Errors.Args;
using CoreValidation.Tests;
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
        public void EqualTo_Should_CollectError(TimeSpan memberValue, TimeSpan argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.EqualTo(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.TimeSpan.EqualTo,
                new IMessageArg[]
                {
                    TimeArg.Create("value", argValue)
                });
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
        public void NotEqualTo_Should_CollectError(TimeSpan memberValue, TimeSpan argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.NotEqualTo(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.TimeSpan.NotEqualTo,
                new IMessageArg[]
                {
                    TimeArg.Create("value", argValue)
                });
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
        public void GreaterThan_Should_CollectError(TimeSpan memberValue, TimeSpan min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.GreaterThan(min),
                memberValue,
                expectedIsValid,
                Phrases.Keys.TimeSpan.GreaterThan,
                new IMessageArg[]
                {
                    TimeArg.Create("min", min)
                });
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
        public void GreaterOrEqualTo_Should_CollectError(TimeSpan memberValue, TimeSpan min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.GreaterOrEqualTo(min),
                memberValue,
                expectedIsValid,
                Phrases.Keys.TimeSpan.GreaterOrEqualTo,
                new IMessageArg[]
                {
                    TimeArg.Create("min", min)
                });
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
        public void LessThan_Should_CollectError(TimeSpan memberValue, TimeSpan max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.LessThan(max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.TimeSpan.LessThan,
                new IMessageArg[]
                {
                    TimeArg.Create("max", max)
                });
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
        public void LessOrEqualTo_Should_CollectError(TimeSpan memberValue, TimeSpan max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.LessOrEqualTo(max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.TimeSpan.LessOrEqualTo,
                new IMessageArg[]
                {
                    TimeArg.Create("max", max)
                });
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
        public void Between_Should_CollectError(TimeSpan min, TimeSpan memberValue, TimeSpan max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.Between(min, max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.TimeSpan.Between,
                new IMessageArg[]
                {
                    TimeArg.Create("min", min),
                    TimeArg.Create("max", max)
                });
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
        public void BetweenOrEqualTo_Should_CollectError(TimeSpan min, TimeSpan memberValue, TimeSpan max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.BetweenOrEqualTo(min, max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.TimeSpan.BetweenOrEqualTo,
                new IMessageArg[]
                {
                    TimeArg.Create("min", min),
                    TimeArg.Create("max", max)
                });
        }
    }
}