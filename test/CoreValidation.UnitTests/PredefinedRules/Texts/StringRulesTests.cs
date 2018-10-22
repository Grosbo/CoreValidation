using System;
using System.Collections.Generic;
using CoreValidation.Errors.Args;
using CoreValidation.Tests;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Texts
{
    public class StringRulesTests
    {
        [Theory]
        [InlineData("abc", "abc", true)]
        [InlineData("!@#$%^&*()_[]{};':\",/.<>?~789456123", "!@#$%^&*()_[]{};':\",/.<>?~789456123", true)]
        [InlineData("ęóąśłżźć", "ęóąśłżźć", true)]
        [InlineData("ABC", "ABC", true)]
        [InlineData("", "", true)]
        [InlineData("", "#", false)]
        [InlineData("abc", "cba", false)]
        [InlineData("abc", "abcd", false)]
        [InlineData("abc", "ABC", false)]
        [InlineData("abc", " abc ", false)]
        [InlineData("ĘÓĄŚŁŻŹĆ", "EOASLZZC", false)]
        public void EqualTo_Should_CollectError(string memberValue, string argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.EqualTo(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Texts.EqualTo,
                new IMessageArg[]
                {
                    new TextArg("value", argValue),
                    new EnumArg<StringComparison>("stringComparison", StringComparison.Ordinal)
                });
        }

        [Theory]
        [InlineData("abc", "abc", false)]
        [InlineData("!@#$%^&*()_[]{};':\",/.<>?~789456123", "!@#$%^&*()_[]{};':\",/.<>?~789456123", false)]
        [InlineData("ęóąśłżźć", "ęóąśłżźć", false)]
        [InlineData("ABC", "ABC", false)]
        [InlineData("", "", false)]
        [InlineData("", "#", true)]
        [InlineData("abc", "cba", true)]
        [InlineData("abc", "abcd", true)]
        [InlineData("abc", "ABC", true)]
        [InlineData("abc", " abc ", true)]
        [InlineData("ĘÓĄŚŁŻŹĆ", "EOASLZZC", true)]
        public void NotEqualTo_Should_CollectError(string memberValue, string argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.NotEqualTo(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Texts.NotEqualTo,
                new IMessageArg[]
                {
                    new TextArg("value", argValue),
                    new EnumArg<StringComparison>("stringComparison", StringComparison.Ordinal)
                });
        }

        [Theory]
        [InlineData("abc", "abc", true)]
        [InlineData("!@#$%^&*()_[]{};':\",/.<>?~789456123", "!@#$%^&*()_[]{};':\",/.<>?~789456123", true)]
        [InlineData("ęóąśłżźć", "ęóąśłżźć", true)]
        [InlineData("ĘÓĄŚŁŻŹĆ", "ęóąśłżźć", true)]
        [InlineData("ABC", "ABC", true)]
        [InlineData("abc", "ABC", true)]
        [InlineData("abc 123 !@# ĘÓĄŚŁŻŹĆ DEF", "ABC 123 !@# ęóąśłżźć def", true)]
        [InlineData("", "", true)]
        [InlineData("", "#", false)]
        [InlineData("abc", "cba", false)]
        [InlineData("abc", "abcd", false)]
        [InlineData("abc", " abc ", false)]
        [InlineData("ĘÓĄŚŁŻŹĆ", "EOASLZZC", false)]
        public void EqualTo_Should_CollectError_When_ComparisonIgnoreCase(string memberValue, string argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.EqualTo(argValue, StringComparison.OrdinalIgnoreCase),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Texts.EqualTo,
                new IMessageArg[]
                {
                    new TextArg("value", argValue),
                    new EnumArg<StringComparison>("stringComparison", StringComparison.OrdinalIgnoreCase)
                });
        }

        [Theory]
        [InlineData("abc", "abc", false)]
        [InlineData("!@#$%^&*()_[]{};':\",/.<>?~789456123", "!@#$%^&*()_[]{};':\",/.<>?~789456123", false)]
        [InlineData("ęóąśłżźć", "ęóąśłżźć", false)]
        [InlineData("ĘÓĄŚŁŻŹĆ", "ęóąśłżźć", false)]
        [InlineData("ABC", "ABC", false)]
        [InlineData("abc", "ABC", false)]
        [InlineData("abc 123 !@# ĘÓĄŚŁŻŹĆ DEF", "ABC 123 !@# ęóąśłżźć def", false)]
        [InlineData("", "", false)]
        [InlineData("", "#", true)]
        [InlineData("abc", "cba", true)]
        [InlineData("abc", "abcd", true)]
        [InlineData("abc", " abc ", true)]
        [InlineData("ĘÓĄŚŁŻŹĆ", "EOASLZZC", true)]
        public void NotEqualTo_Should_CollectError_When_ComparisonIgnoreCase(string memberValue, string argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.NotEqualTo(argValue, StringComparison.OrdinalIgnoreCase),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Texts.NotEqualTo,
                new IMessageArg[]
                {
                    new TextArg("value", argValue),
                    new EnumArg<StringComparison>("stringComparison", StringComparison.OrdinalIgnoreCase)
                });
        }

        public static IEnumerable<object[]> Contains_Should_CollectError_Data()
        {
            yield return new object[] {$"test{Environment.NewLine}abc", "ABC", StringComparison.Ordinal, false};
            yield return new object[] {$"test{Environment.NewLine}abc", "ABC", StringComparison.OrdinalIgnoreCase, true};
            yield return new object[] {$"test{Environment.NewLine}abc", $"{Environment.NewLine}abc", StringComparison.OrdinalIgnoreCase, true};
            yield return new object[] {$"test{Environment.NewLine}abc", $"{Environment.NewLine}ABC", StringComparison.OrdinalIgnoreCase, true};
            yield return new object[] {$"test{Environment.NewLine}abc", $"{Environment.NewLine}ABC", StringComparison.Ordinal, false};
        }

        [Theory]
        [InlineData("ruletest", "TEST", StringComparison.Ordinal, false)]
        [InlineData("ruletest", "TEST", StringComparison.OrdinalIgnoreCase, true)]
        [InlineData("ruletest", "test123", StringComparison.Ordinal, false)]
        [InlineData("ruletest", "test123", StringComparison.OrdinalIgnoreCase, false)]
        [InlineData("ruletest", "rule123", StringComparison.Ordinal, false)]
        [InlineData("ruletest", "rule123", StringComparison.OrdinalIgnoreCase, false)]
        [InlineData("abc !@# DEF", "abc !", StringComparison.Ordinal, true)]
        [InlineData("abc !@# DEF", "abc !", StringComparison.OrdinalIgnoreCase, true)]
        [InlineData("abc !@# DEF", "!@#", StringComparison.Ordinal, true)]
        [InlineData("abc !@# DEF", "!@#", StringComparison.OrdinalIgnoreCase, true)]
        [InlineData("abc !@# DEF", "# def", StringComparison.Ordinal, false)]
        [InlineData("abc !@# DEF", "# DEF", StringComparison.OrdinalIgnoreCase, true)]
        [MemberData(nameof(Contains_Should_CollectError_Data))]
        public void Contains_Should_CollectError(string memberValue, string argValue, StringComparison stringComparison, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.Contains(argValue, stringComparison),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Texts.Contains,
                new IMessageArg[]
                {
                    new TextArg("value", argValue),
                    new EnumArg<StringComparison>("stringComparison", stringComparison)
                });
        }

        public static IEnumerable<object[]> NotContains_Should_CollectError_Data()
        {
            yield return new object[] {$"test{Environment.NewLine}abc", "ABC", StringComparison.Ordinal, true};
            yield return new object[] {$"test{Environment.NewLine}abc", "ABC", StringComparison.OrdinalIgnoreCase, false};
            yield return new object[] {$"test{Environment.NewLine}abc", $"{Environment.NewLine}abc", StringComparison.OrdinalIgnoreCase, false};
            yield return new object[] {$"test{Environment.NewLine}abc", $"{Environment.NewLine}ABC", StringComparison.OrdinalIgnoreCase, false};
            yield return new object[] {$"test{Environment.NewLine}abc", $"{Environment.NewLine}ABC", StringComparison.Ordinal, true};
        }

        [Theory]
        [InlineData("ruletest", "TEST", StringComparison.Ordinal, true)]
        [InlineData("ruletest", "TEST", StringComparison.OrdinalIgnoreCase, false)]
        [InlineData("ruletest", "test123", StringComparison.Ordinal, true)]
        [InlineData("ruletest", "test123", StringComparison.OrdinalIgnoreCase, true)]
        [InlineData("ruletest", "rule123", StringComparison.Ordinal, true)]
        [InlineData("ruletest", "rule123", StringComparison.OrdinalIgnoreCase, true)]
        [InlineData("abc !@# DEF", "abc !", StringComparison.Ordinal, false)]
        [InlineData("abc !@# DEF", "abc !", StringComparison.OrdinalIgnoreCase, false)]
        [InlineData("abc !@# DEF", "!@#", StringComparison.Ordinal, false)]
        [InlineData("abc !@# DEF", "!@#", StringComparison.OrdinalIgnoreCase, false)]
        [InlineData("abc !@# DEF", "# def", StringComparison.Ordinal, true)]
        [InlineData("abc !@# DEF", "# DEF", StringComparison.OrdinalIgnoreCase, false)]
        [MemberData(nameof(NotContains_Should_CollectError_Data))]
        public void NotContains_Should_CollectError(string memberValue, string argValue, StringComparison stringComparison, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.NotContains(argValue, stringComparison),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Texts.NotContains,
                new IMessageArg[]
                {
                    new TextArg("value", argValue),
                    new EnumArg<StringComparison>("stringComparison", stringComparison)
                });
        }

        public static IEnumerable<object[]> NotEmpty_Should_CollectError_NewLines_Data()
        {
            yield return new object[] {$"{Environment.NewLine}", true};
            yield return new object[] {$"\t{Environment.NewLine}{Environment.NewLine}", true};
        }

        [Theory]
        [InlineData("abc", true)]
        [InlineData(" ", true)]
        [InlineData("", false)]
        [MemberData(nameof(NotEmpty_Should_CollectError_NewLines_Data))]
        public void NotEmpty_Should_CollectError(string memberValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.NotEmpty(),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Texts.NotEmpty);
        }

        public static IEnumerable<object[]> NotWhiteSpace_Should_CollectError_NewLines_Data()
        {
            yield return new object[] {$"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}_", true};
            yield return new object[] {$"\t{Environment.NewLine}\t\t_", true};

            yield return new object[] {$"{Environment.NewLine}", false};
            yield return new object[] {$"\t{Environment.NewLine}", false};
            yield return new object[] {$"{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}", false};
            yield return new object[] {$"\t{Environment.NewLine}\t{Environment.NewLine}{Environment.NewLine}{Environment.NewLine}", false};
        }

        [Theory]
        [InlineData("abc", true)]
        [InlineData("\t\t\t\t_\t\t\t", true)]
        [InlineData(" ", false)]
        [InlineData("\t", false)]
        [InlineData("", false)]
        [MemberData(nameof(NotWhiteSpace_Should_CollectError_NewLines_Data))]
        public void NotWhiteSpace_Should_CollectError(string memberValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.NotWhiteSpace(),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Texts.NotWhiteSpace);
        }

        public static IEnumerable<object[]> SingleLine_Should_CollectError_NewLines_Data()
        {
            yield return new object[] {$"abc{Environment.NewLine}", false};
            yield return new object[] {$"{Environment.NewLine}", false};
            yield return new object[] {$"{Environment.NewLine}{Environment.NewLine}", false};
            yield return new object[] {$"a{Environment.NewLine}b", false};
            yield return new object[] {$"\t{Environment.NewLine}{Environment.NewLine}", false};
        }

        [Theory]
        [InlineData("", true)]
        [InlineData("abc", true)]
        [MemberData(nameof(SingleLine_Should_CollectError_NewLines_Data))]
        public void SingleLine_Should_CollectError(string memberValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.SingleLine(),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Texts.SingleLine);
        }

        public static IEnumerable<object[]> ExactLength_Should_CollectError_NewLines_Data()
        {
            yield return new object[] {$"abc{Environment.NewLine}", 4, true};
            yield return new object[] {$"{Environment.NewLine}", 1, true};
            yield return new object[] {$"{Environment.NewLine}{Environment.NewLine}", 2, true};
            yield return new object[] {$"a{Environment.NewLine}b", 3, true};
            yield return new object[] {$"{Environment.NewLine}", 0, false};
        }

        [Theory]
        [InlineData("abc", 3, true)]
        [InlineData("ĘÓĄŚŁŻŹĆ", 8, true)]
        [InlineData("123545", 6, true)]
        [InlineData("ABC_CDE", 7, true)]
        [InlineData("", 0, true)]
        [InlineData("1234567890", 10, true)]
        [InlineData("abc ", 3, false)]
        [InlineData("Ę Ó Ą Ś Ł Ż Ź Ć ", 15, false)]
        [MemberData(nameof(ExactLength_Should_CollectError_NewLines_Data))]
        public void ExactLength_Should_CollectError(string memberValue, int argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.ExactLength(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Texts.ExactLength,
                new IMessageArg[]
                {
                    NumberArg.Create("length", argValue)
                });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void ExactLength_Should_ThrowException_When_NegativeLength(int argValue)
        {
            Tester.TestMemberRuleException<string>(m => m.ExactLength(argValue), typeof(ArgumentOutOfRangeException));
        }

        public static IEnumerable<object[]> MaxLength_Should_CollectError_NewLines_Data()
        {
            yield return new object[] {$"abc{Environment.NewLine}", 5, true};
            yield return new object[] {$"{Environment.NewLine}", 1, true};
            yield return new object[] {$"{Environment.NewLine}", 0, false};
            yield return new object[] {$"{Environment.NewLine}{Environment.NewLine}", 1, false};
            yield return new object[] {$"a{Environment.NewLine}b", 3, true};
            yield return new object[] {$"a{Environment.NewLine}b", 2, false};
        }

        [Theory]
        [InlineData("abc", 3, true)]
        [InlineData("abc", 2, false)]
        [InlineData("", 0, true)]
        [InlineData("", 1, true)]
        [InlineData("abc1234567890", int.MaxValue, true)]
        [InlineData("\t\t\t", 3, true)]
        [InlineData("\t\t\t_", 3, false)]
        [InlineData("X", 0, false)]
        [MemberData(nameof(MaxLength_Should_CollectError_NewLines_Data))]
        public void MaxLength_Should_CollectError(string memberValue, int argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.MaxLength(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Texts.MaxLength,
                new IMessageArg[]
                {
                    NumberArg.Create("max", argValue)
                });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void MaxLength_Should_ThrowException_When_NegativeLength(int argValue)
        {
            Tester.TestMemberRuleException<string>(m => m.MaxLength(argValue), typeof(ArgumentOutOfRangeException));
        }

        public static IEnumerable<object[]> MinLength_Should_CollectError_NewLines_Data()
        {
            yield return new object[] {$"abc{Environment.NewLine}", 5, false};
            yield return new object[] {$"{Environment.NewLine}", 0, true};
            yield return new object[] {$"{Environment.NewLine}", 1, true};
            yield return new object[] {$"{Environment.NewLine}{Environment.NewLine}", 3, false};
            yield return new object[] {$"a{Environment.NewLine}b", 3, true};
            yield return new object[] {$"a{Environment.NewLine}b", 4, false};
        }

        [Theory]
        [InlineData("abc", 3, true)]
        [InlineData("abc", 2, true)]
        [InlineData("abc", 4, false)]
        [InlineData("", 0, true)]
        [InlineData("", 1, false)]
        [InlineData("abc1234567890", int.MaxValue, false)]
        [InlineData("\t\t\t", 3, true)]
        [InlineData("\t\t\t_", 3, true)]
        [MemberData(nameof(MinLength_Should_CollectError_NewLines_Data))]
        public void MinLength_Should_CollectError(string memberValue, int argValue, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.MinLength(argValue),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Texts.MinLength,
                new IMessageArg[]
                {
                    NumberArg.Create("min", argValue)
                });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void MinLength_Should_ThrowException_When_NegativeLength(int argValue)
        {
            Tester.TestMemberRuleException<string>(m => m.MinLength(argValue), typeof(ArgumentOutOfRangeException));
        }

        public static IEnumerable<object[]> LengthBetween_Should_CollectError_NewLines_Data()
        {
            yield return new object[] {$"abc{Environment.NewLine}", 0, 3, false};
            yield return new object[] {$"abc{Environment.NewLine}", 0, 4, true};

            yield return new object[] {$"{Environment.NewLine}", 0, 1, true};
            yield return new object[] {$"{Environment.NewLine}", 1, int.MaxValue, true};

            yield return new object[] {$"{Environment.NewLine}{Environment.NewLine}", 0, 2, true};
            yield return new object[] {$"{Environment.NewLine}{Environment.NewLine}", 0, 1, false};

            yield return new object[] {$"a{Environment.NewLine}b", 2, 3, true};
            yield return new object[] {$"a{Environment.NewLine}b", 0, 2, false};
        }

        [Theory]
        [InlineData("abc", 0, 3, true)]
        [InlineData("abc", 1, 3, true)]
        [InlineData("abc", 2, 3, true)]
        [InlineData("abc", 3, 3, true)]
        [InlineData("abc", 3, 4, true)]
        [InlineData("abc", 0, 2, false)]
        [InlineData("abc", 1, 1, false)]
        [InlineData("abc", 0, 1, false)]
        [InlineData("abc", 0, int.MaxValue, true)]
        [InlineData("abc", 4, 5, false)]
        [InlineData("abc", 4, int.MaxValue, false)]
        [MemberData(nameof(LengthBetween_Should_CollectError_NewLines_Data))]
        public void LengthBetween_Should_CollectError(string memberValue, int min, int max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.LengthBetween(min, max),
                memberValue,
                expectedIsValid,
                Phrases.Keys.Texts.LengthBetween,
                new IMessageArg[]
                {
                    NumberArg.Create("min", min),
                    NumberArg.Create("max", max)
                });
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(1, -1)]
        [InlineData(int.MinValue, 1)]
        [InlineData(int.MinValue, int.MaxValue)]
        [InlineData(int.MinValue, int.MinValue)]
        public void LengthBetween_Should_ThrowException_When_NegativeLength(int min, int max)
        {
            Tester.TestMemberRuleException<string>(m => m.LengthBetween(min, max), typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(int.MaxValue, 0)]
        [InlineData(1, 0)]
        [InlineData(1000, 100)]
        public void LengthBetween_Should_ThrowException_When_MinGreaterThanMax(int min, int max)
        {
            Tester.TestMemberRuleException<string>(m => m.LengthBetween(min, max), typeof(ArgumentException));
        }

        [Fact]
        public void Contains_Should_ThrowException_When_NullValue()
        {
            Tester.TestMemberRuleException<string>(m => m.Contains(null), typeof(ArgumentNullException));
        }

        [Fact]
        public void EqualTo_Should_ThrowException_When_NullValue()
        {
            Tester.TestMemberRuleException<string>(m => m.EqualTo(null), typeof(ArgumentNullException));
        }

        [Fact]
        public void NotContains_Should_ThrowException_When_NullValue()
        {
            Tester.TestMemberRuleException<string>(m => m.NotContains(null), typeof(ArgumentNullException));
        }

        [Fact]
        public void NotEqualTo_Should_ThrowException_When_NullValuee()
        {
            Tester.TestMemberRuleException<string>(m => m.NotEqualTo(null), typeof(ArgumentNullException));
        }
    }
}