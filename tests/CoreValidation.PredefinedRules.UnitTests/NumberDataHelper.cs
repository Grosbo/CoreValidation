using System;
using System.Collections.Generic;

namespace CoreValidation.PredefinedRules.UnitTests
{
    public static class NumberDataHelper
    {
        public static IEnumerable<object[]> EqualTo_Unsigned<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(0), convert(3), false};
            yield return new object[] {convert(2), convert(5), false};
            yield return new object[] {convert(1), convert(1), true};
        }

        public static IEnumerable<object[]> EqualTo_Signed<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(0), convert(-1), false};
            yield return new object[] {convert(-2), convert(-5), false};
            yield return new object[] {convert(-1), convert(-1), true};
            yield return new object[] {convert(-2), convert(2), false};
        }

        public static IEnumerable<object[]> EqualTo_Limits<T>(T min, T max, T neutral)
        {
            yield return new object[] {max, max, true};
            yield return new object[] {min, max, false};
            yield return new object[] {min, min, true};
            yield return new object[] {min, neutral, false};
            yield return new object[] {max, neutral, false};
        }

        public static IEnumerable<object[]> NotEqualTo_Unsigned<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(0), convert(3), true};
            yield return new object[] {convert(2), convert(5), true};
            yield return new object[] {convert(1), convert(1), false};
        }

        public static IEnumerable<object[]> NotEqualTo_Signed<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(0), convert(-1), true};
            yield return new object[] {convert(-2), convert(-5), true};
            yield return new object[] {convert(-1), convert(-1), false};
            yield return new object[] {convert(-2), convert(2), true};
        }

        public static IEnumerable<object[]> NotEqualTo_Limits<T>(T min, T max, T neutral)
        {
            yield return new object[] {max, max, false};
            yield return new object[] {min, max, true};
            yield return new object[] {min, min, false};
            yield return new object[] {min, neutral, true};
            yield return new object[] {max, neutral, true};
        }

        public static IEnumerable<object[]> GreaterThan_Unsigned<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(0), convert(3), false};
            yield return new object[] {convert(2), convert(1), true};
            yield return new object[] {convert(1), convert(1), false};
            yield return new object[] {convert(1), convert(0), true};
        }

        public static IEnumerable<object[]> GreaterThan_Signed<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(0), convert(-1), true};
            yield return new object[] {convert(-2), convert(-1), false};
            yield return new object[] {convert(-1), convert(-1), false};
            yield return new object[] {convert(2), convert(-2), true};
        }

        public static IEnumerable<object[]> GreaterThan_Limits<T>(T min, T max, T neutral)
        {
            yield return new object[] {max, max, false};
            yield return new object[] {min, max, false};
            yield return new object[] {max, min, true};
            yield return new object[] {min, min, false};
            yield return new object[] {min, neutral, false};
            yield return new object[] {max, neutral, true};
        }

        public static IEnumerable<object[]> GreaterOrEqualTo_Unsigned<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(0), convert(3), false};
            yield return new object[] {convert(2), convert(1), true};
            yield return new object[] {convert(1), convert(1), true};
            yield return new object[] {convert(1), convert(0), true};
        }

        public static IEnumerable<object[]> GreaterOrEqualTo_Signed<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(0), convert(-1), true};
            yield return new object[] {convert(-2), convert(-1), false};
            yield return new object[] {convert(-1), convert(-1), true};
            yield return new object[] {convert(2), convert(-2), true};
        }

        public static IEnumerable<object[]> GreaterOrEqualTo_Limits<T>(T min, T max, T neutral)
        {
            yield return new object[] {max, max, true};
            yield return new object[] {min, max, false};
            yield return new object[] {max, min, true};
            yield return new object[] {min, min, true};
            yield return new object[] {min, neutral, false};
            yield return new object[] {max, neutral, true};
        }

        public static IEnumerable<object[]> LessThan_Unsigned<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(0), convert(3), true};
            yield return new object[] {convert(2), convert(1), false};
            yield return new object[] {convert(1), convert(1), false};
            yield return new object[] {convert(1), convert(0), false};
        }

        public static IEnumerable<object[]> LessThan_Signed<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(0), convert(-1), false};
            yield return new object[] {convert(-2), convert(-1), true};
            yield return new object[] {convert(-1), convert(-1), false};
            yield return new object[] {convert(2), convert(-2), false};
        }

        public static IEnumerable<object[]> LessThan_Limits<T>(T min, T max, T neutral)
        {
            yield return new object[] {max, max, false};
            yield return new object[] {min, max, true};
            yield return new object[] {max, min, false};
            yield return new object[] {min, min, false};
            yield return new object[] {min, neutral, true};
            yield return new object[] {max, neutral, false};
        }

        public static IEnumerable<object[]> LessOrEqualTo_Unsigned<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(0), convert(3), true};
            yield return new object[] {convert(2), convert(1), false};
            yield return new object[] {convert(1), convert(1), true};
            yield return new object[] {convert(1), convert(0), false};
        }

        public static IEnumerable<object[]> LessOrEqualTo_Signed<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(0), convert(-1), false};
            yield return new object[] {convert(-2), convert(-1), true};
            yield return new object[] {convert(-1), convert(-1), true};
            yield return new object[] {convert(2), convert(-2), false};
        }

        public static IEnumerable<object[]> LessOrEqualTo_Limits<T>(T min, T max, T neutral)
        {
            yield return new object[] {max, max, true};
            yield return new object[] {min, max, true};
            yield return new object[] {max, min, false};
            yield return new object[] {min, min, true};
            yield return new object[] {min, neutral, true};
            yield return new object[] {max, neutral, false};
        }

        public static IEnumerable<object[]> Between_Unsigned<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(1), convert(1), convert(3), false};
            yield return new object[] {convert(1), convert(2), convert(3), true};
            yield return new object[] {convert(1), convert(3), convert(3), false};
            yield return new object[] {convert(1), convert(0), convert(3), false};
            yield return new object[] {convert(1), convert(4), convert(3), false};
            yield return new object[] {convert(3), convert(3), convert(3), false};
            yield return new object[] {convert(3), convert(4), convert(3), false};
        }

        public static IEnumerable<object[]> Between_Signed<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(-1), convert(0), convert(1), true};
            yield return new object[] {convert(-1), convert(-1), convert(1), false};
            yield return new object[] {convert(-1), convert(1), convert(1), false};
            yield return new object[] {convert(-1), convert(2), convert(1), false};
            yield return new object[] {convert(-1), convert(-2), convert(1), false};
            yield return new object[] {convert(1), convert(1), convert(1), false};
            yield return new object[] {convert(-3), convert(-2), convert(-3), false};
            yield return new object[] {convert(-3), convert(-2), convert(-1), true};
        }

        public static IEnumerable<object[]> Between_Limits<T>(T min, T max, T neutral)
        {
            yield return new object[] {max, neutral, max, false};
            yield return new object[] {min, neutral, max, true};
            yield return new object[] {min, max, max, false};
            yield return new object[] {min, min, max, false};
            yield return new object[] {min, min, min, false};
            yield return new object[] {max, max, max, false};
        }

        public static IEnumerable<object[]> BetweenOrEqualTo_Signed<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(-1), convert(0), convert(1), true};
            yield return new object[] {convert(-1), convert(-1), convert(1), true};
            yield return new object[] {convert(-1), convert(1), convert(1), true};
            yield return new object[] {convert(-1), convert(2), convert(1), false};
            yield return new object[] {convert(-1), convert(-2), convert(1), false};
            yield return new object[] {convert(1), convert(1), convert(1), true};
            yield return new object[] {convert(-3), convert(-2), convert(-3), false};
            yield return new object[] {convert(-3), convert(-2), convert(-1), true};
        }

        public static IEnumerable<object[]> BetweenOrEqualTo_Unsigned<T>(Func<int, T> convert)
        {
            yield return new object[] {convert(1), convert(1), convert(3), true};
            yield return new object[] {convert(1), convert(2), convert(3), true};
            yield return new object[] {convert(1), convert(3), convert(3), true};
            yield return new object[] {convert(1), convert(0), convert(3), false};
            yield return new object[] {convert(1), convert(4), convert(3), false};
            yield return new object[] {convert(3), convert(3), convert(3), true};
            yield return new object[] {convert(3), convert(4), convert(3), false};
        }

        public static IEnumerable<object[]> BetweenOrEqualTo_Limits<T>(T min, T max, T neutral)
        {
            yield return new object[] {max, neutral, max, false};
            yield return new object[] {min, neutral, max, true};
            yield return new object[] {min, max, max, true};
            yield return new object[] {min, min, max, true};
            yield return new object[] {min, min, min, true};
            yield return new object[] {max, max, max, true};
        }
    }
}