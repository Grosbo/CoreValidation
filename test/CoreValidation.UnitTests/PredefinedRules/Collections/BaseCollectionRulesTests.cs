using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Tests;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Collections
{
    public class BaseCollectionRulesTests
    {
        private static readonly Func<int[], CustomCollection> _convert = array => new CustomCollection(array);

        public static IEnumerable<object[]> ExactCollectionSize_Should_CollectError_Data()
        {
            return CollectionDataHelper.ExactCollectionSize_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(ExactCollectionSize_Should_CollectError_Data))]
        public void ExactCollectionSize_Should_CollectError(CustomCollection member, int size, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.ExactCollectionSize<object, CustomCollection, int>(size),
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
        public void ExactCollectionSize_Should_CollectError_When_LongType(CustomCollection member, long size, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.ExactCollectionSize<object, CustomCollection, int>(size),
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
            Tester.TestMemberRuleException<CustomCollection>(
                s => s.ExactCollectionSize<object, CustomCollection, int>(size),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void ExactCollectionSize_Should_ThrowException_When_NegativeCollectionSize_And_LongType(long size)
        {
            Tester.TestMemberRuleException<CustomCollection>(
                s => s.ExactCollectionSize<object, CustomCollection, int>(size),
                typeof(ArgumentOutOfRangeException));
        }

        public static IEnumerable<object[]> NotEmptyCollection_Should_CollectError_Data()
        {
            return CollectionDataHelper.NotEmptyCollection_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(NotEmptyCollection_Should_CollectError_Data))]
        public void NotEmptyCollection_Should_CollectError(CustomCollection member, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.NotEmptyCollection<object, CustomCollection, int>(),
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
        public void EmptyCollection_Should_CollectError(CustomCollection member, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.EmptyCollection<object, CustomCollection, int>(),
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
        public void MaxCollectionSize_Should_CollectError(CustomCollection member, int max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.MaxCollectionSize<object, CustomCollection, int>(max),
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
        public void MaxCollectionSize_Should_CollectError_When_LongCollectionSize(CustomCollection member, long max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.MaxCollectionSize<object, CustomCollection, int>(max),
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
        public void MinCollectionSize_Should_CollectError(CustomCollection member, int min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.MinCollectionSize<object, CustomCollection, int>(min),
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
        public void MinCollectionSize_Should_CollectError_When_LongCollectionSize(CustomCollection member, long min, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.MinCollectionSize<object, CustomCollection, int>(min),
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
            Tester.TestMemberRuleException<CustomCollection>(
                s => s.MinCollectionSize<object, CustomCollection, int>(min),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void MinCollectionSize_Should_ThrowException_When_NegativeCollectionSize_And_LongCollectionSize(long min)
        {
            Tester.TestMemberRuleException<CustomCollection>(
                s => s.MinCollectionSize<object, CustomCollection, int>(min),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void MaxCollectionSize_Should_ThrowException_When_NegativeCollectionSize(int max)
        {
            Tester.TestMemberRuleException<CustomCollection>(
                s => s.MaxCollectionSize<object, CustomCollection, int>(max),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void MaxCollectionSize_Should_ThrowException_When_NegativeCollectionSize_And_LongCollectionSize(long max)
        {
            Tester.TestMemberRuleException<CustomCollection>(
                s => s.MaxCollectionSize<object, CustomCollection, int>(max),
                typeof(ArgumentOutOfRangeException));
        }

        public static IEnumerable<object[]> CollectionSizeBetween_Should_CollectError_Data()
        {
            return CollectionDataHelper.CollectionSizeBetween_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(CollectionSizeBetween_Should_CollectError_Data))]
        public void CollectionSizeBetween_Should_CollectError(CustomCollection member, int min, int max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.CollectionSizeBetween<object, CustomCollection, int>(min, max),
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
        public void CollectionSizeBetween_Should_CollectError_When_LongCollectionSize(CustomCollection member, long min, long max, bool expectedIsValid)
        {
            Tester.TestSingleMemberRule(
                m => m.CollectionSizeBetween<object, CustomCollection, int>(min, max),
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
            Tester.TestMemberRuleException<CustomCollection>(
                s => s.CollectionSizeBetween<object, CustomCollection, int>(0, max),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void CollectionSizeBetween_Should_ThrowException_When_MaxCollectionSizeIsLongNegative(long max)
        {
            Tester.TestMemberRuleException<CustomCollection>(
                s => s.CollectionSizeBetween<object, CustomCollection, int>(0, max),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void CollectionSizeBetween_Should_ThrowException_When_MinCollectionSizeIsNegative(int min)
        {
            Tester.TestMemberRuleException<CustomCollection>(
                s => s.CollectionSizeBetween<object, CustomCollection, int>(min, 10),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void CollectionSizeBetween_Should_ThrowException_When_MinCollectionSizeIsLongNegative(long min)
        {
            Tester.TestMemberRuleException<CustomCollection>(
                s => s.CollectionSizeBetween<object, CustomCollection, int>(min, 10L),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(20, 0)]
        [InlineData(int.MaxValue, 1)]
        public void CollectionSizeBetween_Should_ThrowException_When_MinLargerThanMax(int min, int max)
        {
            Tester.TestMemberRuleException<CustomCollection>(
                s => s.CollectionSizeBetween<object, CustomCollection, int>(min, max),
                typeof(ArgumentOutOfRangeException));
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(20, 0)]
        [InlineData(long.MaxValue, 1)]
        public void CollectionSizeBetween_Should_ThrowException_When_MinLargerThanMax_And_LongMinAndMax(long min, long max)
        {
            Tester.TestMemberRuleException<CustomCollection>(
                s => s.CollectionSizeBetween<object, CustomCollection, int>(min, max),
                typeof(ArgumentOutOfRangeException));
        }

        public class CustomCollection : IEnumerable<int>
        {
            private readonly List<int> _source;

            public CustomCollection(IEnumerable<int> source)
            {
                _source = source.ToList();
            }

            public IEnumerator<int> GetEnumerator()
            {
                return _source.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}