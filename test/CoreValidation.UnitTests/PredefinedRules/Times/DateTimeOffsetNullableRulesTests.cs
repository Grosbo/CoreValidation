using System;
using System.Collections.Generic;
using CoreValidation.Errors.Args;
using CoreValidation.Tests;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Times
{
    public class DateTimeOffsetNullableRulesTests
    {
        private static readonly Func<int, DateTimeOffset> _convert = ticks => new DateTimeOffset(ticks, TimeSpan.Zero);

        public static IEnumerable<object[]> EqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.EqualTo_Unsigned(_convert),
                NumberDataHelper.EqualTo_Limits(DateTimeOffset.MinValue, DateTimeOffset.MaxValue, new DateTimeOffset(1, TimeSpan.Zero))
            );
        }

        [Theory]
        [MemberData(nameof(EqualTo_Should_CollectError_Data))]
        public void EqualTo_Should_CollectError(DateTimeOffset memberValue, DateTimeOffset value, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTimeOffset?>(
                m => m.EqualTo(value),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.EqualTo,
                new IMessageArg[]
                {
                    TimeArg.Create("value", value),
                    new EnumArg<TimeComparison>("timeComparison", TimeComparison.All)
                });
        }

        public static IEnumerable<object[]> NotEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.NotEqualTo_Unsigned(_convert),
                NumberDataHelper.NotEqualTo_Limits(DateTimeOffset.MinValue, DateTimeOffset.MaxValue, new DateTimeOffset(1, TimeSpan.Zero))
            );
        }

        [Theory]
        [MemberData(nameof(NotEqualTo_Should_CollectError_Data))]
        public void NotEqualTo_Should_CollectError(DateTimeOffset memberValue, DateTimeOffset value, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTimeOffset?>(
                m => m.NotEqualTo(value),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.NotEqualTo,
                new IMessageArg[]
                {
                    TimeArg.Create("value", value),
                    new EnumArg<TimeComparison>("timeComparison", TimeComparison.All)
                });
        }

        public static IEnumerable<object[]> After_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterThan_Unsigned(_convert),
                NumberDataHelper.GreaterThan_Limits(DateTimeOffset.MinValue, DateTimeOffset.MaxValue, new DateTimeOffset(1, TimeSpan.Zero))
            );
        }

        [Theory]
        [MemberData(nameof(After_Should_CollectError_Data))]
        public void After_Should_CollectError(DateTimeOffset memberValue, DateTimeOffset min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTimeOffset?>(
                m => m.After(min),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.After,
                new IMessageArg[]
                {
                    TimeArg.Create("min", min),
                    new EnumArg<TimeComparison>("timeComparison", TimeComparison.All)
                });
        }

        public static IEnumerable<object[]> AfterOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterOrEqualTo_Unsigned(_convert),
                NumberDataHelper.GreaterOrEqualTo_Limits(DateTimeOffset.MinValue, DateTimeOffset.MaxValue, new DateTimeOffset(1, TimeSpan.Zero))
            );
        }

        [Theory]
        [MemberData(nameof(AfterOrEqualTo_Should_CollectError_Data))]
        public void AfterOrEqualTo_Should_CollectError(DateTimeOffset memberValue, DateTimeOffset min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTimeOffset?>(
                m => m.AfterOrEqualTo(min),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.AfterOrEqualTo,
                new IMessageArg[]
                {
                    TimeArg.Create("min", min),
                    new EnumArg<TimeComparison>("timeComparison", TimeComparison.All)
                });
        }

        public static IEnumerable<object[]> Before_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessThan_Unsigned(_convert),
                NumberDataHelper.LessThan_Limits(DateTimeOffset.MinValue, DateTimeOffset.MaxValue, new DateTimeOffset(1, TimeSpan.Zero))
            );
        }

        [Theory]
        [MemberData(nameof(Before_Should_CollectError_Data))]
        public void Before_Should_CollectError(DateTimeOffset memberValue, DateTimeOffset max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTimeOffset?>(
                m => m.Before(max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.Before,
                new IMessageArg[]
                {
                    TimeArg.Create("max", max),
                    new EnumArg<TimeComparison>("timeComparison", TimeComparison.All)
                });
        }

        public static IEnumerable<object[]> BeforeOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessOrEqualTo_Unsigned(_convert),
                NumberDataHelper.LessOrEqualTo_Limits(DateTimeOffset.MinValue, DateTimeOffset.MaxValue, new DateTimeOffset(1, TimeSpan.Zero))
            );
        }

        [Theory]
        [MemberData(nameof(BeforeOrEqualTo_Should_CollectError_Data))]
        public void BeforeOrEqualTo_Should_CollectError(DateTimeOffset memberValue, DateTimeOffset max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTimeOffset?>(
                m => m.BeforeOrEqualTo(max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.BeforeOrEqualTo,
                new IMessageArg[]
                {
                    TimeArg.Create("max", max),
                    new EnumArg<TimeComparison>("timeComparison", TimeComparison.All)
                });
        }

        public static IEnumerable<object[]> Between_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.Between_Unsigned(_convert),
                NumberDataHelper.Between_Limits(DateTimeOffset.MinValue, DateTimeOffset.MaxValue, new DateTimeOffset(1, TimeSpan.Zero))
            );
        }

        [Theory]
        [MemberData(nameof(Between_Should_CollectError_Data))]
        public void Between_Should_CollectError(DateTimeOffset min, DateTimeOffset memberValue, DateTimeOffset max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTimeOffset?>(
                m => m.Between(min, max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.Between,
                new IMessageArg[]
                {
                    TimeArg.Create("min", min),
                    TimeArg.Create("max", max),
                    new EnumArg<TimeComparison>("timeComparison", TimeComparison.All)
                });
        }

        public static IEnumerable<object[]> BetweenOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.BetweenOrEqualTo_Unsigned(_convert),
                NumberDataHelper.BetweenOrEqualTo_Limits(DateTimeOffset.MinValue, DateTimeOffset.MaxValue, new DateTimeOffset(1, TimeSpan.Zero))
            );
        }

        [Theory]
        [MemberData(nameof(BetweenOrEqualTo_Should_CollectError_Data))]
        public void BetweenOrEqualTo_Should_CollectError(DateTimeOffset min, DateTimeOffset memberValue, DateTimeOffset max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTimeOffset?>(
                m => m.BetweenOrEqualTo(min, max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.BetweenOrEqualTo,
                new IMessageArg[]
                {
                    TimeArg.Create("min", min),
                    TimeArg.Create("max", max),
                    new EnumArg<TimeComparison>("timeComparison", TimeComparison.All)
                });
        }

        public class ComparisonModesTests
        {
            public static IEnumerable<object[]> EqualTo_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};

                // dates equal
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), TimeComparison.JustTime, false};

                // times equal
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};

                // all different
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 9, 5, 4, 3, 2, 9, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 9, 5, 4, 3, 2, 9, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 9, 5, 4, 3, 2, 9, TimeSpan.Zero), TimeComparison.JustTime, false};
            }

            [Theory]
            [MemberData(nameof(EqualTo_Should_CollectError_When_TimeComparisonSet_Data))]
            public void EqualTo_Should_CollectError_When_TimeComparisonSet(DateTimeOffset memberValue, DateTimeOffset value, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTimeOffset?>(
                    m => m.EqualTo(value, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.EqualTo,
                    new IMessageArg[]
                    {
                        TimeArg.Create("value", value),
                        new EnumArg<TimeComparison>("timeComparison", timeComparison)
                    });
            }

            public static IEnumerable<object[]> NotEqualTo_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};

                // dates equal
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), TimeComparison.JustTime, true};

                // times equal
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};

                // all different
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 9, 5, 4, 3, 2, 9, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 9, 5, 4, 3, 2, 9, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 9, 5, 4, 3, 2, 9, TimeSpan.Zero), TimeComparison.JustTime, true};
            }

            [Theory]
            [MemberData(nameof(NotEqualTo_Should_CollectError_When_TimeComparisonSet_Data))]
            public void NotEqualTo_Should_CollectError_When_TimeComparisonSet(DateTimeOffset memberValue, DateTimeOffset value, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTimeOffset?>(
                    m => m.NotEqualTo(value, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.NotEqualTo,
                    new IMessageArg[]
                    {
                        TimeArg.Create("value", value),
                        new EnumArg<TimeComparison>("timeComparison", timeComparison)
                    });
            }

            public static IEnumerable<object[]> After_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};

                // time before
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};

                // time after
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};

                // date before
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};

                // date after
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};

                // all before
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};

                // all after
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};
            }

            [Theory]
            [MemberData(nameof(After_Should_CollectError_When_TimeComparisonSet_Data))]
            public void After_Should_CollectError_When_TimeComparisonSet(DateTimeOffset memberValue, DateTimeOffset min, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTimeOffset?>(
                    m => m.After(min, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.After,
                    new IMessageArg[]
                    {
                        TimeArg.Create("min", min),
                        new EnumArg<TimeComparison>("timeComparison", timeComparison)
                    });
            }

            public static IEnumerable<object[]> AfterOrEqualTo_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};

                // time before
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};

                // time after
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};

                // date before
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};

                // date after
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};

                // all before
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};

                // all after
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};
            }

            [Theory]
            [MemberData(nameof(AfterOrEqualTo_Should_CollectError_When_TimeComparisonSet_Data))]
            public void AfterOrEqualTo_Should_CollectError_When_TimeComparisonSet(DateTimeOffset memberValue, DateTimeOffset min, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTimeOffset?>(
                    m => m.AfterOrEqualTo(min, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.AfterOrEqualTo,
                    new IMessageArg[]
                    {
                        TimeArg.Create("min", min),
                        new EnumArg<TimeComparison>("timeComparison", timeComparison)
                    });
            }

            public static IEnumerable<object[]> Before_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};

                // time before
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};

                // time after
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};

                // date before
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};

                // date after
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};

                // all before
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};

                // all after
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};
            }

            [Theory]
            [MemberData(nameof(Before_Should_CollectError_When_TimeComparisonSet_Data))]
            public void Before_Should_CollectError_When_TimeComparisonSet(DateTimeOffset memberValue, DateTimeOffset max, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTimeOffset?>(
                    m => m.Before(max, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.Before,
                    new IMessageArg[]
                    {
                        TimeArg.Create("max", max),
                        new EnumArg<TimeComparison>("timeComparison", timeComparison)
                    });
            }

            public static IEnumerable<object[]> BeforeOrEqualTo_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};

                // time before
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};

                // time after
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 5, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};

                // date before
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};

                // date after
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 1, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};

                // all before
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2007, 6, 1, 4, 3, 2, 0, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, true};

                // all after
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2007, 6, 9, 4, 3, 2, 9, TimeSpan.Zero), new DateTimeOffset(2007, 6, 5, 4, 3, 2, 1, TimeSpan.Zero), TimeComparison.JustTime, false};
            }

            [Theory]
            [MemberData(nameof(BeforeOrEqualTo_Should_CollectError_When_TimeComparisonSet_Data))]
            public void BeforeOrEqualTo_Should_CollectError_When_TimeComparisonSet(DateTimeOffset memberValue, DateTimeOffset max, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTimeOffset?>(
                    m => m.BeforeOrEqualTo(max, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.BeforeOrEqualTo,
                    new IMessageArg[]
                    {
                        TimeArg.Create("max", max),
                        new EnumArg<TimeComparison>("timeComparison", timeComparison)
                    });
            }

            public static IEnumerable<object[]> Between_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), TimeComparison.JustTime, false};

                // all between
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, true};

                // dates equal min
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 9, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 9, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 9, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, true};

                // dates equal max
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, true};

                // time equal min
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, false};

                // time equal max
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 11, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 11, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 11, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, false};

                // dates before min
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 8, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 8, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 8, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, true};

                // dates after max
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 12, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 12, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 12, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, true};

                // time before min
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 8, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 8, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 8, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, false};

                // time after max
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 12, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 12, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 12, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, false};

                // all before min
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 8, 10, 10, 10, 8, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 8, 10, 10, 10, 8, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 8, 10, 10, 10, 8, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, false};

                // all after max
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 12, 10, 10, 10, 12, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 12, 10, 10, 10, 12, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 12, 10, 10, 10, 12, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, false};
            }

            [Theory]
            [MemberData(nameof(Between_Should_CollectError_When_TimeComparisonSet_Data))]
            public void Between_Should_CollectError_When_TimeComparisonSet(DateTimeOffset min, DateTimeOffset memberValue, DateTimeOffset max, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTimeOffset?>(
                    m => m.Between(min, max, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.Between,
                    new IMessageArg[]
                    {
                        TimeArg.Create("min", min),
                        TimeArg.Create("max", max),
                        new EnumArg<TimeComparison>("timeComparison", timeComparison)
                    });
            }

            public static IEnumerable<object[]> BetweenOrEqualTo_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), TimeComparison.JustTime, true};

                // all between
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, true};

                // dates equal min
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 9, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 9, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 9, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, true};

                // dates equal max
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, true};

                // time equal min
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, true};

                // time equal max
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 11, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 11, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 11, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, true};

                // dates before min
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 8, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 8, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 8, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, true};

                // dates after max
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 12, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 12, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 12, 10, 10, 10, 10, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, true};

                // time before min
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 8, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 8, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 8, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, false};

                // time after max
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 12, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 12, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, true};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 10, 10, 10, 10, 12, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, false};

                // all before min
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 8, 10, 10, 10, 8, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 8, 10, 10, 10, 8, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 8, 10, 10, 10, 8, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, false};

                // all after max
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 12, 10, 10, 10, 12, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.All, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 12, 10, 10, 10, 12, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustDate, false};
                yield return new object[] {new DateTimeOffset(2000, 10, 9, 10, 10, 10, 9, TimeSpan.Zero), new DateTimeOffset(2000, 10, 12, 10, 10, 10, 12, TimeSpan.Zero), new DateTimeOffset(2000, 10, 11, 10, 10, 10, 11, TimeSpan.Zero), TimeComparison.JustTime, false};
            }

            [Theory]
            [MemberData(nameof(BetweenOrEqualTo_Should_CollectError_When_TimeComparisonSet_Data))]
            public void BetweenOrEqualTo_Should_CollectError_When_TimeComparisonSet(DateTimeOffset min, DateTimeOffset memberValue, DateTimeOffset max, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTimeOffset?>(
                    m => m.BetweenOrEqualTo(min, max, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.BetweenOrEqualTo,
                    new IMessageArg[]
                    {
                        TimeArg.Create("min", min),
                        TimeArg.Create("max", max),
                        new EnumArg<TimeComparison>("timeComparison", timeComparison)
                    });
            }
        }
    }
}