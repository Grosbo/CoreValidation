using System;
using System.Collections.Generic;
using CoreValidation.Specifications;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Numbers
{
    public class FloatRulesTests
    {
        private static readonly Func<int, float> _convert = Convert.ToSingle;

        public static IEnumerable<object[]> CloseTo_MemberData()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.EqualTo_Signed(_convert),
                NumberDataHelper.EqualTo_Unsigned(_convert),
                NumberDataHelper.EqualTo_Limits(float.MinValue, float.MaxValue, 0),
                new[] {new object[] {0.999999d, 0d, false}},
                new[] {new object[] {1.000001d, 0d, false}},
                new[] {new object[] {1.123456d, 1.123456d, true}}
            );
        }

        [Theory]
        [MemberData(nameof(CloseTo_MemberData))]
        public void CloseTo(float model, float value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, float>();

            builder.CloseTo(value);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.CloseTo);
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
        public void CloseTo_WithTolerance(float model, float value, float tolerance, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, float>();

            builder.CloseTo(value, tolerance);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.CloseTo);
        }

        public static IEnumerable<object[]> NotCloseTo_MemberData()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.NotEqualTo_Signed(_convert),
                NumberDataHelper.NotEqualTo_Unsigned(_convert),
                NumberDataHelper.NotEqualTo_Limits(float.MinValue, float.MaxValue, 0),
                new[] {new object[] {0.999999d, 0d, true}},
                new[] {new object[] {1.000001d, 0d, true}},
                new[] {new object[] {1.123456d, 1.123456d, false}}
            );
        }

        [Theory]
        [MemberData(nameof(NotCloseTo_MemberData))]
        public void NotCloseTo(float model, float value, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, float>();

            builder.NotCloseTo(value);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.NotCloseTo);
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
        public void NotCloseTo_WithTolerance(float model, float value, float tolerance, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, float>();

            builder.NotCloseTo(value, tolerance);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.NotCloseTo);
        }

        public static IEnumerable<object[]> GreaterThan_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.GreaterThan_Signed(_convert),
                NumberDataHelper.GreaterThan_Unsigned(_convert),
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
        public void GreaterThan_Should_CollectError(float model, float min, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, float>();

            builder.GreaterThan(min);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.GreaterThan);
        }

        public static IEnumerable<object[]> LessThan_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.LessThan_Signed(_convert),
                NumberDataHelper.LessThan_Unsigned(_convert),
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
        public void LessThan_Should_CollectError(float model, float max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, float>();

            builder.LessThan(max);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.LessThan);
        }

        public static IEnumerable<object[]> Between_Should_CollectError_Data()
        {
            return RulesHelper.GetSetsCompilation(
                NumberDataHelper.Between_Signed(_convert),
                NumberDataHelper.Between_Unsigned(_convert),
                NumberDataHelper.Between_Limits(float.MinValue, float.MaxValue, 0),
                new[] {new object[] {0.999999d, 1, 1.000001d, true}},
                new[] {new object[] {0.999999d, 0.999999d, 1.000001d, false}},
                new[] {new object[] {0.999999d, 1.000001d, 1.000001d, false}}
            );
        }

        [Theory]
        [MemberData(nameof(Between_Should_CollectError_Data))]
        public void Between_Should_CollectError(float min, float model, float max, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, float>();

            builder.Between(min, max);

            RulesHelper.AssertErrorCompilation(model, builder.Rules, expectedIsValid, Phrases.Keys.Numbers.Between);
        }

        public class MessageTests
        {
            [Fact]
            public void Between_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, float>();

                builder.Between(1, 3, "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage<float>(4, builder.Rules, "{min} {max} Overriden error message", "1 3 Overriden error message");
            }

            [Fact]
            public void BetweenOrEqualTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, float>();

                builder.Between(1, 3, "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage<float>(4, builder.Rules, "{min} {max} Overriden error message", "1 3 Overriden error message");
            }

            [Fact]
            public void CloseTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, float>();

                builder.CloseTo(0, (float)0.001, "{value} {tolerance} Overriden error message");

                RulesHelper.AssertErrorMessage<float>(4, builder.Rules, "{value} {tolerance} Overriden error message", "0 0.001 Overriden error message");
            }

            [Fact]
            public void GreaterThan()
            {
                var builder = new MemberSpecificationBuilder<object, float>();

                builder.GreaterThan(2, "{min} Overriden error message");

                RulesHelper.AssertErrorMessage<float>(1, builder.Rules, "{min} Overriden error message", "2 Overriden error message");
            }

            [Fact]
            public void LessThan_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, float>();

                builder.LessThan(1, "{max} Overriden error message");

                RulesHelper.AssertErrorMessage<float>(2, builder.Rules, "{max} Overriden error message", "1 Overriden error message");
            }

            [Fact]
            public void NotCloseTo_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, float>();

                builder.NotCloseTo(4, (float)0.001, "{value} {tolerance} Overriden error message");

                RulesHelper.AssertErrorMessage<float>(4, builder.Rules, "{value} {tolerance} Overriden error message", "4 0.001 Overriden error message");
            }
        }
    }
}