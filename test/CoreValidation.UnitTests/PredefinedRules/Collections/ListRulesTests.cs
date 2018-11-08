using System;
using System.Collections.Generic;
using CoreValidation.Tests;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Collections
{
    public class ListRulesTests
    {
        private static readonly Func<int[], List<int>> _convert = array => new List<int>(array);

        public static IEnumerable<object[]> ExactCollectionSize_Should_CollectError_Data()
        {
            return CollectionDataHelper.ExactCollectionSize_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(ExactCollectionSize_Should_CollectError_Data))]
        public void ExactCollectionSize_Should_CollectError(List<int> member, int size, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.ExactCollectionSize(size),
                member,
                expectedIsValid,
                Phrases.Keys.Collections.ExactCollectionSize,
                new[]
                {
                    Arg.Number("size", size)
                });
        }

        [Theory]
        [MemberData(nameof(ExactCollectionSize_Should_CollectError_Data))]
        public void ExactCollectionSize_Should_CollectError_When_LongType(List<int> member, long size, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.ExactCollectionSize(size),
                member,
                expectedIsValid,
                Phrases.Keys.Collections.ExactCollectionSize,
                new[]
                {
                    Arg.Number("size", size)
                });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void ExactCollectionSize_Should_ThrowException_When_NegativeCollectionSize(int size)
        {
            Tester.TestMemberRuleException<List<int>>(
                s => s.ExactCollectionSize(size),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void ExactCollectionSize_Should_ThrowException_When_NegativeCollectionSize_And_LongType(long size)
        {
            Tester.TestMemberRuleException<List<int>>(
                s => s.ExactCollectionSize(size),
                typeof(ArgumentOutOfRangeException));
        }

        public static IEnumerable<object[]> NotEmptyCollection_Should_CollectError_Data()
        {
            return CollectionDataHelper.NotEmptyCollection_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(NotEmptyCollection_Should_CollectError_Data))]
        public void NotEmptyCollection_Should_CollectError(List<int> member, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.NotEmptyCollection(),
                member,
                expectedIsValid,
                Phrases.Keys.Collections.NotEmptyCollection);
        }

        public static IEnumerable<object[]> EmptyCollection_Should_CollectError_Data()
        {
            return CollectionDataHelper.EmptyCollection_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(EmptyCollection_Should_CollectError_Data))]
        public void EmptyCollection_Should_CollectError(List<int> member, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.EmptyCollection(),
                member,
                expectedIsValid,
                Phrases.Keys.Collections.EmptyCollection);
        }

        public static IEnumerable<object[]> MaxCollectionSize_Should_CollectError_Data()
        {
            return CollectionDataHelper.MaxCollectionSize_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MaxCollectionSize_Should_CollectError_Data))]
        public void MaxCollectionSize_Should_CollectError(List<int> member, int max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.MaxCollectionSize(max),
                member,
                expectedIsValid,
                Phrases.Keys.Collections.MaxCollectionSize,
                new[]
                {
                    Arg.Number("max", max)
                });
        }

        public static IEnumerable<object[]> MaxCollectionSize_Should_CollectError_When_LongCollectionSize_Data()
        {
            return CollectionDataHelper.MaxCollectionSize_Should_CollectError_When_LongCollectionSize_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MaxCollectionSize_Should_CollectError_Data))]
        [MemberData(nameof(MaxCollectionSize_Should_CollectError_When_LongCollectionSize_Data))]
        public void MaxCollectionSize_Should_CollectError_When_LongCollectionSize(List<int> member, long max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.MaxCollectionSize(max),
                member,
                expectedIsValid,
                Phrases.Keys.Collections.MaxCollectionSize,
                new[]
                {
                    Arg.Number("max", max)
                });
        }

        public static IEnumerable<object[]> MinCollectionSize_Should_CollectError_Data()
        {
            return CollectionDataHelper.MinCollectionSize_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MinCollectionSize_Should_CollectError_Data))]
        public void MinCollectionSize_Should_CollectError(List<int> member, int min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.MinCollectionSize(min),
                member,
                expectedIsValid,
                Phrases.Keys.Collections.MinCollectionSize,
                new[]
                {
                    Arg.Number("min", min)
                });
        }

        public static IEnumerable<object[]> MinCollectionSize_Should_CollectError_When_LongCollectionSize_Data()
        {
            return CollectionDataHelper.MinCollectionSize_Should_CollectError_When_LongCollectionSize_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MinCollectionSize_Should_CollectError_Data))]
        [MemberData(nameof(MinCollectionSize_Should_CollectError_When_LongCollectionSize_Data))]
        public void MinCollectionSize_Should_CollectError_When_LongCollectionSize(List<int> member, long min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.MinCollectionSize(min),
                member,
                expectedIsValid,
                Phrases.Keys.Collections.MinCollectionSize,
                new[]
                {
                    Arg.Number("min", min)
                });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void MinCollectionSize_Should_ThrowException_When_NegativeCollectionSize(int min)
        {
            Tester.TestMemberRuleException<List<int>>(
                s => s.MinCollectionSize(min),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void MinCollectionSize_Should_ThrowException_When_NegativeCollectionSize_And_LongCollectionSize(long min)
        {
            Tester.TestMemberRuleException<List<int>>(
                s => s.MinCollectionSize(min),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void MaxCollectionSize_Should_ThrowException_When_NegativeCollectionSize(int max)
        {
            Tester.TestMemberRuleException<List<int>>(
                s => s.MaxCollectionSize(max),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void MaxCollectionSize_Should_ThrowException_When_NegativeCollectionSize_And_LongCollectionSize(long max)
        {
            Tester.TestMemberRuleException<List<int>>(
                s => s.MaxCollectionSize(max),
                typeof(ArgumentOutOfRangeException));
        }

        public static IEnumerable<object[]> CollectionSizeBetween_Should_CollectError_Data()
        {
            return CollectionDataHelper.CollectionSizeBetween_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(CollectionSizeBetween_Should_CollectError_Data))]
        public void CollectionSizeBetween_Should_CollectError(List<int> member, int min, int max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.CollectionSizeBetween(min, max),
                member,
                expectedIsValid,
                Phrases.Keys.Collections.CollectionSizeBetween,
                new[]
                {
                    Arg.Number("min", min),
                    Arg.Number("max", max)
                });
        }

        public static IEnumerable<object[]> CollectionSizeBetween_Should_CollectError_When_LongCollectionSize_Data()
        {
            return CollectionDataHelper.CollectionSizeBetween_Should_CollectError_When_LongCollectionSize_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(CollectionSizeBetween_Should_CollectError_Data))]
        [MemberData(nameof(CollectionSizeBetween_Should_CollectError_When_LongCollectionSize_Data))]
        public void CollectionSizeBetween_Should_CollectError_When_LongCollectionSize(List<int> member, long min, long max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.CollectionSizeBetween(min, max),
                member,
                expectedIsValid,
                Phrases.Keys.Collections.CollectionSizeBetween,
                new[]
                {
                    Arg.Number("min", min),
                    Arg.Number("max", max)
                });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void CollectionSizeBetween_Should_ThrowException_When_MaxCollectionSizeIsNegative(int max)
        {
            Tester.TestMemberRuleException<List<int>>(
                s => s.CollectionSizeBetween(0, max),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void CollectionSizeBetween_Should_ThrowException_When_MaxCollectionSizeIsLongNegative(long max)
        {
            Tester.TestMemberRuleException<List<int>>(
                s => s.CollectionSizeBetween(0, max),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void CollectionSizeBetween_Should_ThrowException_When_MinCollectionSizeIsNegative(int min)
        {
            Tester.TestMemberRuleException<List<int>>(
                s => s.CollectionSizeBetween(min, 10),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void CollectionSizeBetween_Should_ThrowException_When_MinCollectionSizeIsLongNegative(long min)
        {
            Tester.TestMemberRuleException<List<int>>(
                s => s.CollectionSizeBetween(min, 10L),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(20, 0)]
        [InlineData(int.MaxValue, 1)]
        public void CollectionSizeBetween_Should_ThrowException_When_MinLargerThanMax(int min, int max)
        {
            Tester.TestMemberRuleException<List<int>>(
                s => s.CollectionSizeBetween(min, max),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(20, 0)]
        [InlineData(long.MaxValue, 1)]
        public void CollectionSizeBetween_Should_ThrowException_When_MinLargerThanMax_And_LongMinAndMax(long min, long max)
        {
            Tester.TestMemberRuleException<List<int>>(
                s => s.CollectionSizeBetween(min, max),
                typeof(ArgumentOutOfRangeException));
        }
    }
}