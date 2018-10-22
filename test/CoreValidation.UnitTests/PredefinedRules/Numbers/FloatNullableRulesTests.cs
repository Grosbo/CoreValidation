using System;
using System.Collections.Generic;
using CoreValidation.Errors.Args;
using CoreValidation.Tests;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Numbers
{
    public class FloatNullableRulesTests
    {
        private static readonly Func<int, float> _convert = Convert.ToSingle;

        public static IEnumerable<object[]> CloseTo_MemberData()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.EqualTo_Unsigned(_convert),
                NumberDataHelper.EqualTo_Signed(_convert),
                NumberDataHelper.EqualTo_Limits(float.MinValue, float.MaxValue, 0),
                new[] {new object[] {0.999999d, 0d, false}},
                new[] {new object[] {1.000001d, 0d, false}},
                new[] {new object[] {1.123456d, 1.123456d, true}}
            );
        }

        [Theory]
        [MemberData(nameof(CloseTo_MemberData))]
        public void CloseTo(float memberValue, float argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<float?>(
                m => m.CloseTo(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.CloseTo,
                new IMessageArg[]
                {
                    NumberArg.Create("value", argValue),
                    NumberArg.Create("tolerance", 0.0000001f)
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
        public void CloseTo_WithTolerance(float memberValue, float argValue, float tolerance, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<float?>(
                m => m.CloseTo(argValue, tolerance),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.CloseTo,
                new IMessageArg[]
                {
                    NumberArg.Create("value", argValue),
                    NumberArg.Create("tolerance", tolerance)
                });
        }

        public static IEnumerable<object[]> NotCloseTo_MemberData()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.NotEqualTo_Unsigned(_convert),
                NumberDataHelper.NotEqualTo_Signed(_convert),
                NumberDataHelper.NotEqualTo_Limits(float.MinValue, float.MaxValue, 0),
                new[] {new object[] {0.999999d, 0d, true}},
                new[] {new object[] {1.000001d, 0d, true}},
                new[] {new object[] {1.123456d, 1.123456d, false}}
            );
        }

        [Theory]
        [MemberData(nameof(NotCloseTo_MemberData))]
        public void NotCloseTo(float memberValue, float argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<float?>(
                m => m.NotCloseTo(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.NotCloseTo,
                new IMessageArg[]
                {
                    NumberArg.Create("value", argValue),
                    NumberArg.Create("tolerance", 0.0000001f)
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
        public void NotCloseTo_WithTolerance(float memberValue, float argValue, float tolerance, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<float?>(
                m => m.NotCloseTo(argValue, tolerance),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.NotCloseTo,
                new IMessageArg[]
                {
                    NumberArg.Create("value", argValue),
                    NumberArg.Create("tolerance", tolerance)
                });
        }

        public static IEnumerable<object[]> GreaterThan_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterThan_Unsigned(_convert),
                NumberDataHelper.GreaterThan_Signed(_convert),
                NumberDataHelper.GreaterThan_Limits(float.MinValue, float.MaxValue, 0),
                new[] {new object[] {0.999999d, 1d, false}},
                new[] {new object[] {1.000001d, 1d, true}},
                new[] {new object[] {0.999999d, 0.999999d, false}},
                new[] {new object[] {1d, 1.000001d, false}},
                new[] {new object[] {1.000001d, 1.000001d, false}}
            );
        }

        [Theory]
        [MemberData(nameof(GreaterThan_Should_CollectError_Data))]
        public void GreaterThan_Should_CollectError(float memberValue, float min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<float?>(
                m => m.GreaterThan(min),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.GreaterThan,
                new IMessageArg[]
                {
                    NumberArg.Create("min", min)
                });
        }

        public static IEnumerable<object[]> LessThan_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessThan_Unsigned(_convert),
                NumberDataHelper.LessThan_Signed(_convert),
                NumberDataHelper.LessThan_Limits(float.MinValue, float.MaxValue, 0),
                new[] {new object[] {0.999999d, 1d, true}},
                new[] {new object[] {1.000001d, 1d, false}},
                new[] {new object[] {0.999999d, 0.999999d, false}},
                new[] {new object[] {1d, 1.000001d, true}},
                new[] {new object[] {1.000001d, 1.000001d, false}}
            );
        }

        [Theory]
        [MemberData(nameof(LessThan_Should_CollectError_Data))]
        public void LessThan_Should_CollectError(float memberValue, float max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<float?>(
                m => m.LessThan(max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Numbers.LessThan,
                new IMessageArg[]
                {
                    NumberArg.Create("max", max)
                });
        }

        public static IEnumerable<object[]> Between_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.Between_Unsigned(_convert),
                NumberDataHelper.Between_Signed(_convert),
                NumberDataHelper.Between_Limits(float.MinValue, float.MaxValue, 0),
                new[] {new object[] {0.999999d, 1, 1.000001d, true}},
                new[] {new object[] {0.999999d, 0.999999d, 1.000001d, false}},
                new[] {new object[] {0.999999d, 1.000001d, 1.000001d, false}}
            );
        }

        [Theory]
        [MemberData(nameof(Between_Should_CollectError_Data))]
        public void Between_Should_CollectError(float min, float memberValue, float max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule<float?>(
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
    }
}