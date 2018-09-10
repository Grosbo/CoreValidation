using System;
using System.Collections.Generic;
using CoreValidation.Specifications;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Numbers
{
    public class DoubleNullableRulesTests
    {
        private static readonly Func<int, double> _convert = Convert.ToDouble;

        public static IEnumerable<object[]> CloseTo_MemberData()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.EqualTo_Signed(_convert),
                NumberDataHelper.EqualTo_Unsigned(_convert),
                NumberDataHelper.EqualTo_Limits(double.MinValue, double.MaxValue, 0),
                new[] {new object[] {0.999999d, 0d, false}},
                new[] {new object[] {1.000001d, 0d, false}},
                new[] {new object[] {1.123456d, 1.123456d, true}}
            );
        }

        [Theory]
        [MemberData(nameof(CloseTo_MemberData))]
        public void CloseTo(double model, double value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, double?>();

            builder.CloseTo(value);

            RulesHelper.AssertErrorCompilation<double?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.CloseTo);
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
        public void CloseTo_WithTolerance(double model, double value, double tolerance, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, double?>();

            builder.CloseTo(value, tolerance);

            RulesHelper.AssertErrorCompilation<double?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.CloseTo);
        }

        public static IEnumerable<object[]> NotCloseTo_MemberData()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.NotEqualTo_Signed(_convert),
                NumberDataHelper.NotEqualTo_Unsigned(_convert),
                NumberDataHelper.NotEqualTo_Limits(double.MinValue, double.MaxValue, 0),
                new[] {new object[] {0.999999d, 0d, true}},
                new[] {new object[] {1.000001d, 0d, true}},
                new[] {new object[] {1.123456d, 1.123456d, false}}
            );
        }

        [Theory]
        [MemberData(nameof(NotCloseTo_MemberData))]
        public void NotCloseTo(double model, double value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, double?>();

            builder.NotCloseTo(value);

            RulesHelper.AssertErrorCompilation<double?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.NotCloseTo);
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
        public void NotCloseTo_WithTolerance(double model, double value, double tolerance, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, double?>();

            builder.NotCloseTo(value, tolerance);

            RulesHelper.AssertErrorCompilation<double?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.NotCloseTo);
        }

        public static IEnumerable<object[]> GreaterThan_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterThan_Signed(_convert),
                NumberDataHelper.GreaterThan_Unsigned(_convert),
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
        public void GreaterThan_Should_CollectError(double model, double min, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, double?>();

            builder.GreaterThan(min);

            RulesHelper.AssertErrorCompilation<double?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.GreaterThan);
        }

        public static IEnumerable<object[]> LessThan_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessThan_Signed(_convert),
                NumberDataHelper.LessThan_Unsigned(_convert),
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
        public void LessThan_Should_CollectError(double model, double max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, double?>();

            builder.LessThan(max);

            RulesHelper.AssertErrorCompilation<double?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.LessThan);
        }

        public static IEnumerable<object[]> Between_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.Between_Signed(_convert),
                NumberDataHelper.Between_Unsigned(_convert),
                NumberDataHelper.Between_Limits(double.MinValue, double.MaxValue, 0),
                new[] {new object[] {0.999999d, 1, 1.000001d, true}},
                new[] {new object[] {0.999999d, 0.999999d, 1.000001d, false}},
                new[] {new object[] {0.999999d, 1.000001d, 1.000001d, false}}
            );
        }

        [Theory]
        [MemberData(nameof(Between_Should_CollectError_Data))]
        public void Between_Should_CollectError(double min, double model, double max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, double?>();

            builder.Between(min, max);

            RulesHelper.AssertErrorCompilation<double?>(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.Between);
        }

        public class MessageTests
        {
            [Fact]
            public void Between_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, double?>();

                builder.Between(1, 3, "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage<double?>(4, builder.Rules, "{min} {max} Overriden error message", "1 3 Overriden error message");
            }

            [Fact]
            public void BetweenOrEqualTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, double?>();

                builder.Between(1, 3, "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage<double?>(4, builder.Rules, "{min} {max} Overriden error message", "1 3 Overriden error message");
            }

            [Fact]
            public void CloseTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, double?>();

                builder.CloseTo(0, 0.001, "{value} {tolerance} Overriden error message");

                RulesHelper.AssertErrorMessage<double?>(4, builder.Rules, "{value} {tolerance} Overriden error message", "0 0.001 Overriden error message");
            }

            [Fact]
            public void GreaterThan()
            {
                var builder = new MemberSpecificationBuilder<object, double?>();

                builder.GreaterThan(2, "{min} Overriden error message");

                RulesHelper.AssertErrorMessage<double?>(1, builder.Rules, "{min} Overriden error message", "2 Overriden error message");
            }

            [Fact]
            public void LessThan_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, double?>();

                builder.LessThan(1, "{max} Overriden error message");

                RulesHelper.AssertErrorMessage<double?>(2, builder.Rules, "{max} Overriden error message", "1 Overriden error message");
            }

            [Fact]
            public void NotCloseTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, double?>();

                builder.NotCloseTo(4, 0.001, "{value} {tolerance} Overriden error message");

                RulesHelper.AssertErrorMessage<double?>(4, builder.Rules, "{value} {tolerance} Overriden error message", "4 0.001 Overriden error message");
            }
        }
    }
}