using System;
using System.Collections.Generic;
using CoreValidation.Specifications;
using CoreValidation.Translations;
using Xunit;

namespace CoreValidation.PredefinedRules.UnitTests.Times
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
        public void EqualTo_Should_CollectError(DateTime model, DateTime value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, DateTime?>();

            builder.EqualTo(value);

            RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.EqualTo);
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
        public void NotEqualTo_Should_CollectError(DateTime model, DateTime value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, DateTime?>();

            builder.NotEqualTo(value);

            RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.NotEqualTo);
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
        public void After_Should_CollectError(DateTime model, DateTime value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, DateTime?>();

            builder.After(value);

            RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.After);
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
        public void AfterOrEqualTo_Should_CollectError(DateTime model, DateTime value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, DateTime?>();

            builder.AfterOrEqualTo(value);

            RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.AfterOrEqualTo);
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
        public void Before_Should_CollectError(DateTime model, DateTime max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, DateTime?>();

            builder.Before(max);

            RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.Before);
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
        public void BeforeOrEqualTo_Should_CollectError(DateTime model, DateTime max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, DateTime?>();

            builder.BeforeOrEqualTo(max);

            RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.BeforeOrEqualTo);
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
        public void Between_Should_CollectError(DateTime min, DateTime model, DateTime max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, DateTime?>();

            builder.Between(min, max);

            RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.Between);
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
        public void BetweenOrEqualTo_Should_CollectError(DateTime min, DateTime model, DateTime max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, DateTime?>();

            builder.BetweenOrEqualTo(min, max);

            RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.BetweenOrEqualTo);
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
            public void EqualTo_Should_CollectError_When_TimeComparisonSet(DateTime model, DateTime value, TimeComparison timeComparison, bool expectedIsValid)
            {
                var builder = new MemberSpecificationBuilder<object, DateTime?>();

                builder.EqualTo(value, timeComparison);

                RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.EqualTo);
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
            public void NotEqualTo_Should_CollectError_When_TimeComparisonSet(DateTime model, DateTime value, TimeComparison timeComparison, bool expectedIsValid)
            {
                var builder = new MemberSpecificationBuilder<object, DateTime?>();

                builder.NotEqualTo(value, timeComparison);

                RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.NotEqualTo);
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
            public void After_Should_CollectError_When_TimeComparisonSet(DateTime model, DateTime value, TimeComparison timeComparison, bool expectedIsValid)
            {
                var builder = new MemberSpecificationBuilder<object, DateTime?>();

                builder.After(value, timeComparison);

                RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.After);
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
            public void AfterOrEqualTo_Should_CollectError_When_TimeComparisonSet(DateTime model, DateTime value, TimeComparison timeComparison, bool expectedIsValid)
            {
                var builder = new MemberSpecificationBuilder<object, DateTime?>();

                builder.AfterOrEqualTo(value, timeComparison);

                RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.AfterOrEqualTo);
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
            public void Before_Should_CollectError_When_TimeComparisonSet(DateTime model, DateTime max, TimeComparison timeComparison, bool expectedIsValid)
            {
                var builder = new MemberSpecificationBuilder<object, DateTime?>();

                builder.Before(max, timeComparison);

                RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.Before);
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
            public void BeforeOrEqualTo_Should_CollectError_When_TimeComparisonSet(DateTime model, DateTime max, TimeComparison timeComparison, bool expectedIsValid)
            {
                var builder = new MemberSpecificationBuilder<object, DateTime?>();

                builder.BeforeOrEqualTo(max, timeComparison);

                RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.BeforeOrEqualTo);
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
            public void Between_Should_CollectError_When_TimeComparisonSet(DateTime min, DateTime model, DateTime max, TimeComparison timeComparison, bool expectedIsValid)
            {
                var builder = new MemberSpecificationBuilder<object, DateTime?>();

                builder.Between(min, max, timeComparison);

                RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.Between);
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
            public void BetweenOrEqualTo_Should_CollectError_When_TimeComparisonSet(DateTime min, DateTime model, DateTime max, TimeComparison timeComparison, bool expectedIsValid)
            {
                var builder = new MemberSpecificationBuilder<object, DateTime?>();

                builder.BetweenOrEqualTo(min, max, timeComparison);

                RulesHelper.AssertErrorCompilation<DateTime?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Times.BetweenOrEqualTo);
            }

            public class MessageTests
            {
                [Fact]
                public void Between_Should_SetCustomMessage()
                {
                    var builder = new MemberSpecificationBuilder<object, DateTime?>();

                    builder.Between(new DateTime(2020, 09, 21, 15, 0, 1), new DateTime(2020, 09, 21, 15, 0, 3), message: "{min} {max} {timeComparison} Overriden error message");

                    RulesHelper.AssertErrorMessage<DateTime?>(new DateTime(2020, 09, 21, 15, 0, 4), builder.Rules, "{min} {max} {timeComparison} Overriden error message", "2020-09-21 15:00:01 2020-09-21 15:00:03 All Overriden error message");
                }

                [Fact]
                public void BetweenOrEqualTo_Should_SetCustomMessage()
                {
                    var builder = new MemberSpecificationBuilder<object, DateTime?>();

                    builder.BetweenOrEqualTo(new DateTime(2020, 09, 21, 15, 0, 1), new DateTime(2020, 09, 21, 15, 0, 3), message: "{min} {max} {timeComparison} Overriden error message");

                    RulesHelper.AssertErrorMessage<DateTime?>(new DateTime(2020, 09, 21, 15, 0, 4), builder.Rules, "{min} {max} {timeComparison} Overriden error message", "2020-09-21 15:00:01 2020-09-21 15:00:03 All Overriden error message");
                }

                [Fact]
                public void EqualTo_Should_SetCustomMessage()
                {
                    var builder = new MemberSpecificationBuilder<object, DateTime?>();

                    builder.EqualTo(new DateTime(2020, 09, 21, 15, 0, 0), message: "{value} {timeComparison} Overriden error message");

                    RulesHelper.AssertErrorMessage<DateTime?>(new DateTime(2020, 09, 21, 15, 0, 4), builder.Rules, "{value} {timeComparison} Overriden error message", "2020-09-21 15:00:00 All Overriden error message");
                }

                [Fact]
                public void GreaterOrEqual_Should_SetCustomMessage()
                {
                    var builder = new MemberSpecificationBuilder<object, DateTime?>();

                    builder.AfterOrEqualTo(new DateTime(2020, 09, 21, 15, 0, 2), message: "{min} {timeComparison} Overriden error message");

                    RulesHelper.AssertErrorMessage<DateTime?>(new DateTime(2020, 09, 21, 15, 0, 1), builder.Rules, "{min} {timeComparison} Overriden error message", "2020-09-21 15:00:02 All Overriden error message");
                }

                [Fact]
                public void GreaterThan_Should_SetCustomMessage()
                {
                    var builder = new MemberSpecificationBuilder<object, DateTime?>();

                    builder.After(new DateTime(2020, 09, 21, 15, 0, 2), message: "{min} {timeComparison} Overriden error message");

                    RulesHelper.AssertErrorMessage<DateTime?>(new DateTime(2020, 09, 21, 15, 0, 1), builder.Rules, "{min} {timeComparison} Overriden error message", "2020-09-21 15:00:02 All Overriden error message");
                }

                [Fact]
                public void LessThan_Should_SetCustomMessage()
                {
                    var builder = new MemberSpecificationBuilder<object, DateTime?>();

                    builder.Before(new DateTime(2020, 09, 21, 15, 0, 1), message: "{max} {timeComparison} Overriden error message");

                    RulesHelper.AssertErrorMessage<DateTime?>(new DateTime(2020, 09, 21, 15, 0, 2), builder.Rules, "{max} {timeComparison} Overriden error message", "2020-09-21 15:00:01 All Overriden error message");
                }

                [Fact]
                public void LessThanOrEqual_Should_SetCustomMessage()
                {
                    var builder = new MemberSpecificationBuilder<object, DateTime?>();

                    builder.BeforeOrEqualTo(new DateTime(2020, 09, 21, 15, 0, 1), message: "{max} {timeComparison} Overriden error message");

                    RulesHelper.AssertErrorMessage<DateTime?>(new DateTime(2020, 09, 21, 15, 0, 2), builder.Rules, "{max} {timeComparison} Overriden error message", "2020-09-21 15:00:01 All Overriden error message");
                }

                [Fact]
                public void NotEqualTo_Should_SetCustomMessage()
                {
                    var builder = new MemberSpecificationBuilder<object, DateTime?>();

                    builder.NotEqualTo(new DateTime(2020, 09, 21, 15, 0, 4), message: "{value} {timeComparison} Overriden error message");

                    RulesHelper.AssertErrorMessage<DateTime?>(new DateTime(2020, 09, 21, 15, 0, 4), builder.Rules, "{value} {timeComparison} Overriden error message", "2020-09-21 15:00:04 All Overriden error message");
                }
            }
        }
    }
}