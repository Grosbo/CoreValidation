using System;
using System.Collections.Generic;
using CoreValidation.Tests;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Numbers
{
    public class DoubleNullableRulesTests
    {
        private static readonly Func<int, double> _convert = Convert.ToDouble;

        public static IEnumerable<object[]> CloseTo_MemberData()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.EqualTo_Unsigned(_convert),
                NumberDataHelper.EqualTo_Signed(_convert),
                NumberDataHelper.EqualTo_Limits(double.MinValue, double.MaxValue, 0),
                new[] {new object[] {0.999999d, 0d, false}},
                new[] {new object[] {1.000001d, 0d, false}},
                new[] {new object[] {1.123456d, 1.123456d, true}}
            );
        }

        [Theory]
        [MemberData(nameof(CloseTo_MemberData))]
        public void CloseTo(double memberValue, double argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<double?>(
                m => m.CloseTo(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.CloseTo,
                new[]
                {
                    Arg.Number("value", argValue),
                    Arg.Number("tolerance", 0.0000001d)
                });
        }

        public static IEnumerable<object[]> CloseTo_WithTolerance_MemberData()
        {
            return RulesHelper.GetSetsCompilation(
                new[] {new object[] {1.000100d, 1.000199d, 0.0000001d, false}},
                new[] {new object[] {1.000100d, 1.000199d, 0.000001d, false}},
                new[] {new object[] {1.000100d, 1.000199d, 0.00001d, false}},
                new[] {new object[] {1.000100d, 1.000199d, 0.0001d, true}},
                new[] {new object[] {1.000100d, 1.000199d, 0.001d, true}},
                new[] {new object[] {1.000100d, 1.000199d, 0.01d, true}},
                new[] {new object[] {1.000100d, 1.000199d, 0.1d, true}},
                new[] {new object[] {1.000100d, 1.000199d, 1d, true}}
            );
        }

        [Theory]
        [MemberData(nameof(CloseTo_WithTolerance_MemberData))]
        public void CloseTo_WithTolerance(double memberValue, double argValue, double tolerance, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<double?>(
                m => m.CloseTo(argValue, tolerance),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.CloseTo,
                new[]
                {
                    Arg.Number("value", argValue),
                    Arg.Number("tolerance", tolerance)
                });
        }

        public static IEnumerable<object[]> NotCloseTo_MemberData()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.NotEqualTo_Unsigned(_convert),
                NumberDataHelper.NotEqualTo_Signed(_convert),
                NumberDataHelper.NotEqualTo_Limits(double.MinValue, double.MaxValue, 0),
                new[] {new object[] {0.999999d, 0d, true}},
                new[] {new object[] {1.000001d, 0d, true}},
                new[] {new object[] {1.123456d, 1.123456d, false}}
            );
        }

        [Theory]
        [MemberData(nameof(NotCloseTo_MemberData))]
        public void NotCloseTo(double memberValue, double argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<double?>(
                m => m.NotCloseTo(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.NotCloseTo,
                new[]
                {
                    Arg.Number("value", argValue),
                    Arg.Number("tolerance", 0.0000001d)
                });
        }

        public static IEnumerable<object[]> NotCloseTo_WithTolerance_MemberData()
        {
            return RulesHelper.GetSetsCompilation(
                new[] {new object[] {1.000100d, 1.000199d, 0.0000001d, true}},
                new[] {new object[] {1.000100d, 1.000199d, 0.000001d, true}},
                new[] {new object[] {1.000100d, 1.000199d, 0.00001d, true}},
                new[] {new object[] {1.000100d, 1.000199d, 0.0001d, false}},
                new[] {new object[] {1.000100d, 1.000199d, 0.001d, false}},
                new[] {new object[] {1.000100d, 1.000199d, 0.01d, false}},
                new[] {new object[] {1.000100d, 1.000199d, 0.1d, false}},
                new[] {new object[] {1.000100d, 1.000199d, 1d, false}}
            );
        }

        [Theory]
        [MemberData(nameof(NotCloseTo_WithTolerance_MemberData))]
        public void NotCloseTo_WithTolerance(double memberValue, double argValue, double tolerance, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<double?>(
                m => m.NotCloseTo(argValue, tolerance),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.NotCloseTo,
                new[]
                {
                    Arg.Number("value", argValue),
                    Arg.Number("tolerance", tolerance)
                });
        }

        public static IEnumerable<object[]> GreaterThan_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterThan_Unsigned(_convert),
                NumberDataHelper.GreaterThan_Signed(_convert),
                NumberDataHelper.GreaterThan_Limits(double.MinValue, double.MaxValue, 0),
                new[] {new object[] {0.999999d, 1d, false}},
                new[] {new object[] {1.000001d, 1d, true}},
                new[] {new object[] {0.999999d, 0.999999d, false}},
                new[] {new object[] {1d, 1.000001d, false}},
                new[] {new object[] {1.000001d, 1.000001d, false}}
            );
        }

        [Theory]
        [MemberData(nameof(GreaterThan_Should_CollectError_Data))]
        public void GreaterThan_Should_CollectError(double memberValue, double min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<double?>(
                m => m.GreaterThan(min),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.GreaterThan,
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
                NumberDataHelper.LessThan_Limits(double.MinValue, double.MaxValue, 0),
                new[] {new object[] {0.999999d, 1d, true}},
                new[] {new object[] {1.000001d, 1d, false}},
                new[] {new object[] {0.999999d, 0.999999d, false}},
                new[] {new object[] {1d, 1.000001d, true}},
                new[] {new object[] {1.000001d, 1.000001d, false}}
            );
        }

        [Theory]
        [MemberData(nameof(LessThan_Should_CollectError_Data))]
        public void LessThan_Should_CollectError(double memberValue, double max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<double?>(
                m => m.LessThan(max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.LessThan,
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
                NumberDataHelper.Between_Limits(double.MinValue, double.MaxValue, 0),
                new[] {new object[] {0.999999d, 1, 1.000001d, true}},
                new[] {new object[] {0.999999d, 0.999999d, 1.000001d, false}},
                new[] {new object[] {0.999999d, 1.000001d, 1.000001d, false}}
            );
        }

        [Theory]
        [MemberData(nameof(Between_Should_CollectError_Data))]
        public void Between_Should_CollectError(double min, double memberValue, double max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<double?>(
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
    }
}