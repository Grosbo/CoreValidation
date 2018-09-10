using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CoreValidation.Specifications;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Collections
{
    public class CollectionRulesTests
    {
        private static readonly Func<int[], Collection<int>> _convert = array => new Collection<int>(array);

        public static IEnumerable<object[]> ExactCollectionSize_Should_CollectError_Data()
        {
            return CollectionDataHelper.ExactCollectionSize_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(ExactCollectionSize_Should_CollectError_Data))]
        public void ExactCollectionSize_Should_CollectError(Collection<int> items, int expectedCollectionSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.ExactCollectionSize(expectedCollectionSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.ExactCollectionSize);
        }

        [Theory]
        [MemberData(nameof(ExactCollectionSize_Should_CollectError_Data))]
        public void ExactCollectionSize_Should_CollectError_When_LongType(Collection<int> items, long expectedCollectionSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.ExactCollectionSize(expectedCollectionSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.ExactCollectionSize);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void ExactCollectionSize_Should_ThrowException_When_NegativeCollectionSize(int expectedCollectionSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.ExactCollectionSize(expectedCollectionSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void ExactCollectionSize_Should_ThrowException_When_NegativeCollectionSize_And_LongType(long expectedCollectionSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.ExactCollectionSize(expectedCollectionSize); });
        }

        public static IEnumerable<object[]> NotEmptyCollection_Should_CollectError_Data()
        {
            return CollectionDataHelper.NotEmptyCollection_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(NotEmptyCollection_Should_CollectError_Data))]
        public void NotEmptyCollection_Should_CollectError(Collection<int> items, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.NotEmptyCollection();

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.NotEmptyCollection);
        }

        public static IEnumerable<object[]> EmptyCollection_Should_CollectError_Data()
        {
            return CollectionDataHelper.EmptyCollection_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(EmptyCollection_Should_CollectError_Data))]
        public void EmptyCollection_Should_CollectError(Collection<int> items, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.EmptyCollection();

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.EmptyCollection);
        }

        public static IEnumerable<object[]> MaxCollectionSize_Should_CollectError_Data()
        {
            return CollectionDataHelper.MaxCollectionSize_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MaxCollectionSize_Should_CollectError_Data))]
        public void MaxCollectionSize_Should_CollectError(Collection<int> items, int maxCollectionSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.MaxCollectionSize(maxCollectionSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.MaxCollectionSize);
        }

        public static IEnumerable<object[]> MaxCollectionSize_Should_CollectError_When_LongCollectionSize_Data()
        {
            return CollectionDataHelper.MaxCollectionSize_Should_CollectError_When_LongCollectionSize_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MaxCollectionSize_Should_CollectError_Data))]
        [MemberData(nameof(MaxCollectionSize_Should_CollectError_When_LongCollectionSize_Data))]
        public void MaxCollectionSize_Should_CollectError_When_LongCollectionSize(Collection<int> items, long maxCollectionSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.MaxCollectionSize(maxCollectionSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.MaxCollectionSize);
        }

        public static IEnumerable<object[]> MinCollectionSize_Should_CollectError_Data()
        {
            return CollectionDataHelper.MinCollectionSize_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MinCollectionSize_Should_CollectError_Data))]
        public void MinCollectionSize_Should_CollectError(Collection<int> items, int minCollectionSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.MinCollectionSize(minCollectionSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.MinCollectionSize);
        }

        public static IEnumerable<object[]> MinCollectionSize_Should_CollectError_When_LongCollectionSize_Data()
        {
            return CollectionDataHelper.MinCollectionSize_Should_CollectError_When_LongCollectionSize_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MinCollectionSize_Should_CollectError_Data))]
        [MemberData(nameof(MinCollectionSize_Should_CollectError_When_LongCollectionSize_Data))]
        public void MinCollectionSize_Should_CollectError_When_LongCollectionSize(Collection<int> items, long minCollectionSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.MinCollectionSize(minCollectionSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.MinCollectionSize);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void MinCollectionSize_Should_ThrowException_When_NegativeCollectionSize(int minCollectionSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.MinCollectionSize(minCollectionSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void MinCollectionSize_Should_ThrowException_When_NegativeCollectionSize_And_LongCollectionSize(long minCollectionSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.MinCollectionSize(minCollectionSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void MaxCollectionSize_Should_ThrowException_When_NegativeCollectionSize(int maxCollectionSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.MaxCollectionSize(maxCollectionSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void MaxCollectionSize_Should_ThrowException_When_NegativeCollectionSize_And_LongCollectionSize(long maxCollectionSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.MaxCollectionSize(maxCollectionSize); });
        }

        public static IEnumerable<object[]> CollectionSizeBetween_Should_CollectError_Data()
        {
            return CollectionDataHelper.CollectionSizeBetween_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(CollectionSizeBetween_Should_CollectError_Data))]
        public void CollectionSizeBetween_Should_CollectError(Collection<int> items, int minCollectionSize, int maxCollectionSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.CollectionSizeBetween(minCollectionSize, maxCollectionSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.CollectionSizeBetween);
        }

        public static IEnumerable<object[]> CollectionSizeBetween_Should_CollectError_When_LongCollectionSize_Data()
        {
            return CollectionDataHelper.CollectionSizeBetween_Should_CollectError_When_LongCollectionSize_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(CollectionSizeBetween_Should_CollectError_Data))]
        [MemberData(nameof(CollectionSizeBetween_Should_CollectError_When_LongCollectionSize_Data))]
        public void CollectionSizeBetween_Should_CollectError_When_LongCollectionSize(Collection<int> items, long minCollectionSize, long maxCollectionSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.CollectionSizeBetween(minCollectionSize, maxCollectionSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.CollectionSizeBetween);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void CollectionSizeBetween_Should_ThrowException_When_MaxCollectionSizeIsNegative(int maxCollectionSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.CollectionSizeBetween(0, maxCollectionSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void CollectionSizeBetween_Should_ThrowException_When_MaxCollectionSizeIsLongNegative(long maxCollectionSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.CollectionSizeBetween(0, maxCollectionSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void CollectionSizeBetween_Should_ThrowException_When_MinCollectionSizeIsNegative(int minCollectionSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.CollectionSizeBetween(minCollectionSize, 10); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void CollectionSizeBetween_Should_ThrowException_When_MinCollectionSizeIsLongNegative(long minCollectionSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.CollectionSizeBetween(minCollectionSize, 10L); });
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(20, 0)]
        [InlineData(int.MaxValue, 1)]
        public void CollectionSizeBetween_Should_ThrowException_When_MinLargerThanMax(int minCollectionSize, int maxCollectionSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.CollectionSizeBetween(minCollectionSize, maxCollectionSize); });
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(20, 0)]
        [InlineData(long.MaxValue, 1)]
        public void CollectionSizeBetween_Should_ThrowException_When_MinLargerThanMax_And_LongMinAndMax(long minCollectionSize, long maxCollectionSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.CollectionSizeBetween(minCollectionSize, maxCollectionSize); });
        }

        public class MessageTests
        {
            [Fact]
            public void CollectionSizeBetween_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.CollectionSizeBetween(3, 4, "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{min} {max} Overriden error message", "3 4 Overriden error message");
            }

            [Fact]
            public void CollectionSizeBetween_Should_SetCustomMessage_When_LongType()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.CollectionSizeBetween(3, (long)4, "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{min} {max} Overriden error message", "3 4 Overriden error message");
            }

            [Fact]
            public void EmptyCollection_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.EmptyCollection("Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "Overriden error message", "Overriden error message");
            }

            [Fact]
            public void ExactCollectionSize_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.ExactCollectionSize(10, "{size} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{size} Overriden error message", "10 Overriden error message");
            }

            [Fact]
            public void ExactCollectionSize_Should_SetCustomMessage_When_LongType()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.ExactCollectionSize((long)10, "{size} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{size} Overriden error message", "10 Overriden error message");
            }

            [Fact]
            public void MaxCollectionSize_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.MaxCollectionSize(3, "{max} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1, 2, 3, 4}), builder.Rules, "{max} Overriden error message", "3 Overriden error message");
            }

            [Fact]
            public void MaxCollectionSize_Should_SetCustomMessage_When_LongType()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.MaxCollectionSize((long)3, "{max} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1, 2, 3, 4}), builder.Rules, "{max} Overriden error message", "3 Overriden error message");
            }

            [Fact]
            public void MinCollectionSize_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.MinCollectionSize(3, "{min} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{min} Overriden error message", "3 Overriden error message");
            }

            [Fact]
            public void MinCollectionSize_Should_SetCustomMessage_When_LongType()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.MinCollectionSize((long)3, "{min} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{min} Overriden error message", "3 Overriden error message");
            }

            [Fact]
            public void NotEmptyCollection_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.NotEmptyCollection("Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(Array.Empty<int>()), builder.Rules, "Overriden error message", "Overriden error message");
            }
        }
    }
}