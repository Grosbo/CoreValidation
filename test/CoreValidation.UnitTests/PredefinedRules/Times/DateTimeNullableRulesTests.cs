using System;
using System.Collections.Generic;
using CoreValidation.Tests;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Times
{
    public class DateTimeNullableRulesTests
    {
        private static readonly Func<int, DateTime> _convert = ticks => new DateTime(ticks);

        public static IEnumerable<object[]> EqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.EqualTo_Unsigned(_convert),
                NumberDataHelper.EqualTo_Limits(DateTime.MinValue, DateTime.MaxValue, new DateTime(1))
            );
        }

        [Theory]
        [MemberData(nameof(EqualTo_Should_CollectError_Data))]
        public void EqualTo_Should_CollectError(DateTime memberValue, DateTime value, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTime?>(
                m => m.EqualTo(value),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.EqualTo,
                new[]
                {
                    Arg.Time("value", value),
                    Arg.Enum("timeComparison", TimeComparison.All)
                });
        }

        public static IEnumerable<object[]> NotEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.NotEqualTo_Unsigned(_convert),
                NumberDataHelper.NotEqualTo_Limits(DateTime.MinValue, DateTime.MaxValue, new DateTime(1))
            );
        }

        [Theory]
        [MemberData(nameof(NotEqualTo_Should_CollectError_Data))]
        public void NotEqualTo_Should_CollectError(DateTime memberValue, DateTime value, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTime?>(
                m => m.NotEqualTo(value),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.NotEqualTo,
                new[]
                {
                    Arg.Time("value", value),
                    Arg.Enum("timeComparison", TimeComparison.All)
                });
        }

        public static IEnumerable<object[]> After_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterThan_Unsigned(_convert),
                NumberDataHelper.GreaterThan_Limits(DateTime.MinValue, DateTime.MaxValue, new DateTime(1))
            );
        }

        [Theory]
        [MemberData(nameof(After_Should_CollectError_Data))]
        public void After_Should_CollectError(DateTime memberValue, DateTime min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTime?>(
                m => m.After(min),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.After,
                new[]
                {
                    Arg.Time("min", min),
                    Arg.Enum("timeComparison", TimeComparison.All)
                });
        }

        public static IEnumerable<object[]> AfterOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterOrEqualTo_Unsigned(_convert),
                NumberDataHelper.GreaterOrEqualTo_Limits(DateTime.MinValue, DateTime.MaxValue, new DateTime(1))
            );
        }

        [Theory]
        [MemberData(nameof(AfterOrEqualTo_Should_CollectError_Data))]
        public void AfterOrEqualTo_Should_CollectError(DateTime memberValue, DateTime min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTime?>(
                m => m.AfterOrEqualTo(min),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.AfterOrEqualTo,
                new[]
                {
                    Arg.Time("min", min),
                    Arg.Enum("timeComparison", TimeComparison.All)
                });
        }

        public static IEnumerable<object[]> Before_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessThan_Unsigned(_convert),
                NumberDataHelper.LessThan_Limits(DateTime.MinValue, DateTime.MaxValue, new DateTime(1))
            );
        }

        [Theory]
        [MemberData(nameof(Before_Should_CollectError_Data))]
        public void Before_Should_CollectError(DateTime memberValue, DateTime max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTime?>(
                m => m.Before(max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.Before,
                new[]
                {
                    Arg.Time("max", max),
                    Arg.Enum("timeComparison", TimeComparison.All)
                });
        }

        public static IEnumerable<object[]> BeforeOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessOrEqualTo_Unsigned(_convert),
                NumberDataHelper.LessOrEqualTo_Limits(DateTime.MinValue, DateTime.MaxValue, new DateTime(1))
            );
        }

        [Theory]
        [MemberData(nameof(BeforeOrEqualTo_Should_CollectError_Data))]
        public void BeforeOrEqualTo_Should_CollectError(DateTime memberValue, DateTime max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTime?>(
                m => m.BeforeOrEqualTo(max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.BeforeOrEqualTo,
                new[]
                {
                    Arg.Time("max", max),
                    Arg.Enum("timeComparison", TimeComparison.All)
                });
        }

        public static IEnumerable<object[]> Between_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.Between_Unsigned(_convert),
                NumberDataHelper.Between_Limits(DateTime.MinValue, DateTime.MaxValue, new DateTime(1))
            );
        }

        [Theory]
        [MemberData(nameof(Between_Should_CollectError_Data))]
        public void Between_Should_CollectError(DateTime min, DateTime memberValue, DateTime max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTime?>(
                m => m.Between(min, max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.Between,
                new[]
                {
                    Arg.Time("min", min),
                    Arg.Time("max", max),
                    Arg.Enum("timeComparison", TimeComparison.All)
                });
        }

        public static IEnumerable<object[]> BetweenOrEqualTo_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.BetweenOrEqualTo_Unsigned(_convert),
                NumberDataHelper.BetweenOrEqualTo_Limits(DateTime.MinValue, DateTime.MaxValue, new DateTime(1))
            );
        }

        [Theory]
        [MemberData(nameof(BetweenOrEqualTo_Should_CollectError_Data))]
        public void BetweenOrEqualTo_Should_CollectError(DateTime min, DateTime memberValue, DateTime max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<DateTime?>(
                m => m.BetweenOrEqualTo(min, max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Times.BetweenOrEqualTo,
                new[]
                {
                    Arg.Time("min", min),
                    Arg.Time("max", max),
                    Arg.Enum("timeComparison", TimeComparison.All)
                });
        }

        public class ComparisonModesTests
        {
            public static IEnumerable<object[]> EqualTo_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};

                // dates equal
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 9), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 9), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 9), TimeComparison.JustTime, false};

                // times equal
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 9, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 9, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 9, 4, 3, 2, 1), TimeComparison.JustTime, true};

                // all different
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 9, 5, 4, 3, 2, 9), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 9, 5, 4, 3, 2, 9), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 9, 5, 4, 3, 2, 9), TimeComparison.JustTime, false};
            }

            [Theory]
            [MemberData(nameof(EqualTo_Should_CollectError_When_TimeComparisonSet_Data))]
            public void EqualTo_Should_CollectError_When_TimeComparisonSet(DateTime memberValue, DateTime value, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTime?>(
                    m => m.EqualTo(value, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.EqualTo,
                    new[]
                    {
                        Arg.Time("value", value),
                        Arg.Enum("timeComparison", timeComparison)
                    });
            }

            public static IEnumerable<object[]> NotEqualTo_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};

                // dates equal
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 9), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 9), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 9), TimeComparison.JustTime, true};

                // times equal
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 9, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 9, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 9, 4, 3, 2, 1), TimeComparison.JustTime, false};

                // all different
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 9, 5, 4, 3, 2, 9), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 9, 5, 4, 3, 2, 9), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 9, 5, 4, 3, 2, 9), TimeComparison.JustTime, true};
            }

            [Theory]
            [MemberData(nameof(NotEqualTo_Should_CollectError_When_TimeComparisonSet_Data))]
            public void NotEqualTo_Should_CollectError_When_TimeComparisonSet(DateTime memberValue, DateTime value, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTime?>(
                    m => m.NotEqualTo(value, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.NotEqualTo,
                    new[]
                    {
                        Arg.Time("value", value),
                        Arg.Enum("timeComparison", timeComparison)
                    });
            }

            public static IEnumerable<object[]> After_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};

                // time before
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};

                // time after
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};

                // date before
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};

                // date after
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};

                // all before
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};

                // all after
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};
            }

            [Theory]
            [MemberData(nameof(After_Should_CollectError_When_TimeComparisonSet_Data))]
            public void After_Should_CollectError_When_TimeComparisonSet(DateTime memberValue, DateTime min, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTime?>(
                    m => m.After(min, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.After,
                    new[]
                    {
                        Arg.Time("min", min),
                        Arg.Enum("timeComparison", timeComparison)
                    });
            }

            public static IEnumerable<object[]> AfterOrEqualTo_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};

                // time before
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};

                // time after
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};

                // date before
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};

                // date after
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};

                // all before
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};

                // all after
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};
            }

            [Theory]
            [MemberData(nameof(AfterOrEqualTo_Should_CollectError_When_TimeComparisonSet_Data))]
            public void AfterOrEqualTo_Should_CollectError_When_TimeComparisonSet(DateTime memberValue, DateTime min, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTime?>(
                    m => m.AfterOrEqualTo(min, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.AfterOrEqualTo,
                    new[]
                    {
                        Arg.Time("min", min),
                        Arg.Enum("timeComparison", timeComparison)
                    });
            }

            public static IEnumerable<object[]> Before_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};

                // time before
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};

                // time after
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};

                // date before
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};

                // date after
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};

                // all before
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};

                // all after
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};
            }

            [Theory]
            [MemberData(nameof(Before_Should_CollectError_When_TimeComparisonSet_Data))]
            public void Before_Should_CollectError_When_TimeComparisonSet(DateTime memberValue, DateTime max, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTime?>(
                    m => m.Before(max, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.Before,
                    new[]
                    {
                        Arg.Time("max", max),
                        Arg.Enum("timeComparison", timeComparison)
                    });
            }

            public static IEnumerable<object[]> BeforeOrEqualTo_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};

                // time before
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};

                // time after
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 5, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};

                // date before
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};

                // date after
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 1), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};

                // all before
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, true};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2007, 6, 1, 4, 3, 2, 0), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, true};

                // all after
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.All, false};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2007, 6, 9, 4, 3, 2, 9), new DateTime(2007, 6, 5, 4, 3, 2, 1), TimeComparison.JustTime, false};
            }

            [Theory]
            [MemberData(nameof(BeforeOrEqualTo_Should_CollectError_When_TimeComparisonSet_Data))]
            public void BeforeOrEqualTo_Should_CollectError_When_TimeComparisonSet(DateTime memberValue, DateTime max, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTime?>(
                    m => m.BeforeOrEqualTo(max, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.BeforeOrEqualTo,
                    new[]
                    {
                        Arg.Time("max", max),
                        Arg.Enum("timeComparison", timeComparison)
                    });
            }

            public static IEnumerable<object[]> Between_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 10, 10, 10, 10, 10), TimeComparison.All, false};
                yield return new object[] {new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 10, 10, 10, 10, 10), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 10, 10, 10, 10, 10), TimeComparison.JustTime, false};

                // all between
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, true};

                // dates equal min
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 9, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 9, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 9, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, true};

                // dates equal max
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 11, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 11, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 11, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, true};

                // time equal min
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 9), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 9), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 9), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, false};

                // time equal max
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 11), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 11), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 11), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, false};

                // dates before min
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 8, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 8, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 8, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, true};

                // dates after max
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 12, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 12, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 12, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, true};

                // time before min
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 8), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 8), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 8), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, false};

                // time after max
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 12), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 12), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 12), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, false};

                // all before min
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 8, 10, 10, 10, 8), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 8, 10, 10, 10, 8), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 8, 10, 10, 10, 8), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, false};

                // all after max
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 12, 10, 10, 10, 12), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 12, 10, 10, 10, 12), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 12, 10, 10, 10, 12), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, false};
            }

            [Theory]
            [MemberData(nameof(Between_Should_CollectError_When_TimeComparisonSet_Data))]
            public void Between_Should_CollectError_When_TimeComparisonSet(DateTime min, DateTime memberValue, DateTime max, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTime?>(
                    m => m.Between(min, max, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.Between,
                    new[]
                    {
                        Arg.Time("min", min),
                        Arg.Time("max", max),
                        Arg.Enum("timeComparison", timeComparison)
                    });
            }

            public static IEnumerable<object[]> BetweenOrEqualTo_Should_CollectError_When_TimeComparisonSet_Data()
            {
                // all equal
                yield return new object[] {new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 10, 10, 10, 10, 10), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 10, 10, 10, 10, 10), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 10, 10, 10, 10, 10), TimeComparison.JustTime, true};

                // all between
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, true};

                // dates equal min
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 9, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 9, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 9, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, true};

                // dates equal max
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 11, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 11, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 11, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, true};

                // time equal min
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 9), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 9), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 9), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, true};

                // time equal max
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 11), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 11), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 11), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, true};

                // dates before min
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 8, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 8, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 8, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, true};

                // dates after max
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 12, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 12, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 12, 10, 10, 10, 10), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, true};

                // time before min
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 8), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 8), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 8), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, false};

                // time after max
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 12), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 12), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, true};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 10, 10, 10, 10, 12), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, false};

                // all before min
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 8, 10, 10, 10, 8), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 8, 10, 10, 10, 8), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 8, 10, 10, 10, 8), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, false};

                // all after max
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 12, 10, 10, 10, 12), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.All, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 12, 10, 10, 10, 12), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustDate, false};
                yield return new object[] {new DateTime(2000, 10, 9, 10, 10, 10, 9), new DateTime(2000, 10, 12, 10, 10, 10, 12), new DateTime(2000, 10, 11, 10, 10, 10, 11), TimeComparison.JustTime, false};
            }

            [Theory]
            [MemberData(nameof(BetweenOrEqualTo_Should_CollectError_When_TimeComparisonSet_Data))]
            public void BetweenOrEqualTo_Should_CollectError_When_TimeComparisonSet(DateTime min, DateTime memberValue, DateTime max, TimeComparison timeComparison, bool expectedIsValid)
            {
                Tester.TestSingleMemberRule<DateTime?>(
                    m => m.BetweenOrEqualTo(min, max, timeComparison),
                    memberValue,
                    expectedIsValid,
                    Phrases.Keys.Times.BetweenOrEqualTo,
                    new[]
                    {
                        Arg.Time("min", min),
                        Arg.Time("max", max),
                        Arg.Enum("timeComparison", timeComparison)
                    });
            }
        }
    }
}