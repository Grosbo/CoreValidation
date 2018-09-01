using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using CoreValidation.Specifications;
using CoreValidation.Translations;
using Xunit;

namespace CoreValidation.UnitTests.PredefinedRules.Collections
{
    public class CollectionRulesTests
    {
        private static readonly Func<int[], Collection<int>> _convert = array => new Collection<int>(array);

        public static IEnumerable<object[]> ExactSize_Should_CollectError_Data()
        {
            return CollectionDataHelper.ExactSize_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(ExactSize_Should_CollectError_Data))]
        public void ExactSize_Should_CollectError(Collection<int> items, int expectedSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.ExactSize(expectedSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.ExactSize);
        }

        [Theory]
        [MemberData(nameof(ExactSize_Should_CollectError_Data))]
        public void ExactSize_Should_CollectError_When_LongType(Collection<int> items, long expectedSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.ExactSize(expectedSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.ExactSize);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void ExactSize_Should_ThrowException_When_NegativeSize(int expectedSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.ExactSize(expectedSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void ExactSize_Should_ThrowException_When_NegativeSize_And_LongType(long expectedSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.ExactSize(expectedSize); });
        }

        public static IEnumerable<object[]> NotEmpty_Should_CollectError_Data()
        {
            return CollectionDataHelper.NotEmpty_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(NotEmpty_Should_CollectError_Data))]
        public void NotEmpty_Should_CollectError(Collection<int> items, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.NotEmpty();

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.NotEmpty);
        }

        public static IEnumerable<object[]> Empty_Should_CollectError_Data()
        {
            return CollectionDataHelper.Empty_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(Empty_Should_CollectError_Data))]
        public void Empty_Should_CollectError(Collection<int> items, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.Empty();

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.Empty);
        }

        public static IEnumerable<object[]> MaxSize_Should_CollectError_Data()
        {
            return CollectionDataHelper.MaxSize_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MaxSize_Should_CollectError_Data))]
        public void MaxSize_Should_CollectError(Collection<int> items, int maxSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.MaxSize(maxSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.MaxSize);
        }

        public static IEnumerable<object[]> MaxSize_Should_CollectError_When_LongSize_Data()
        {
            return CollectionDataHelper.MaxSize_Should_CollectError_When_LongSize_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MaxSize_Should_CollectError_Data))]
        [MemberData(nameof(MaxSize_Should_CollectError_When_LongSize_Data))]
        public void MaxSize_Should_CollectError_When_LongSize(Collection<int> items, long maxSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.MaxSize(maxSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.MaxSize);
        }

        public static IEnumerable<object[]> MinSize_Should_CollectError_Data()
        {
            return CollectionDataHelper.MinSize_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MinSize_Should_CollectError_Data))]
        public void MinSize_Should_CollectError(Collection<int> items, int minSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.MinSize(minSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.MinSize);
        }

        public static IEnumerable<object[]> MinSize_Should_CollectError_When_LongSize_Data()
        {
            return CollectionDataHelper.MinSize_Should_CollectError_When_LongSize_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MinSize_Should_CollectError_Data))]
        [MemberData(nameof(MinSize_Should_CollectError_When_LongSize_Data))]
        public void MinSize_Should_CollectError_When_LongSize(Collection<int> items, long minSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.MinSize(minSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.MinSize);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void MinSize_Should_ThrowException_When_NegativeSize(int minSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.MinSize(minSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void MinSize_Should_ThrowException_When_NegativeSize_And_LongSize(long minSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.MinSize(minSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void MaxSize_Should_ThrowException_When_NegativeSize(int maxSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.MaxSize(maxSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void MaxSize_Should_ThrowException_When_NegativeSize_And_LongSize(long maxSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.MaxSize(maxSize); });
        }

        public static IEnumerable<object[]> SizeBetween_Should_CollectError_Data()
        {
            return CollectionDataHelper.SizeBetween_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(SizeBetween_Should_CollectError_Data))]
        public void SizeBetween_Should_CollectError(Collection<int> items, int minSize, int maxSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.SizeBetween(minSize, maxSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.SizeBetween);
        }

        public static IEnumerable<object[]> SizeBetween_Should_CollectError_When_LongSize_Data()
        {
            return CollectionDataHelper.SizeBetween_Should_CollectError_When_LongSize_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(SizeBetween_Should_CollectError_Data))]
        [MemberData(nameof(SizeBetween_Should_CollectError_When_LongSize_Data))]
        public void SizeBetween_Should_CollectError_When_LongSize(Collection<int> items, long minSize, long maxSize, bool expectedIsValid)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            builder.SizeBetween(minSize, maxSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.SizeBetween);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void SizeBetween_Should_ThrowException_When_MaxSizeIsNegative(int maxSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.SizeBetween(0, maxSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void SizeBetween_Should_ThrowException_When_MaxSizeIsLongNegative(long maxSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.SizeBetween(0, maxSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void SizeBetween_Should_ThrowException_When_MinSizeIsNegative(int minSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.SizeBetween(minSize, 10); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void SizeBetween_Should_ThrowException_When_MinSizeIsLongNegative(long minSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.SizeBetween(minSize, 10L); });
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(20, 0)]
        [InlineData(int.MaxValue, 1)]
        public void SizeBetween_Should_ThrowException_When_MinLargerThanMax(int minSize, int maxSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.SizeBetween(minSize, maxSize); });
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(20, 0)]
        [InlineData(long.MaxValue, 1)]
        public void SizeBetween_Should_ThrowException_When_MinLargerThanMax_And_LongMinAndMax(long minSize, long maxSize)
        {
            var builder = new MemberSpecificationBuilder<object, Collection<int>>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.SizeBetween(minSize, maxSize); });
        }

        public class MessageTests
        {
            [Fact]
            public void Empty_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.Empty("Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "Overriden error message", "Overriden error message");
            }

            [Fact]
            public void ExactSize_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.ExactSize(10, "{size} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{size} Overriden error message", "10 Overriden error message");
            }

            [Fact]
            public void ExactSize_Should_SetCustomMessage_When_LongType()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.ExactSize((long)10, "{size} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{size} Overriden error message", "10 Overriden error message");
            }

            [Fact]
            public void MaxSize_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.MaxSize(3, "{max} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1, 2, 3, 4}), builder.Rules, "{max} Overriden error message", "3 Overriden error message");
            }

            [Fact]
            public void MaxSize_Should_SetCustomMessage_When_LongType()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.MaxSize((long)3, "{max} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1, 2, 3, 4}), builder.Rules, "{max} Overriden error message", "3 Overriden error message");
            }

            [Fact]
            public void MinSize_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.MinSize(3, "{min} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{min} Overriden error message", "3 Overriden error message");
            }

            [Fact]
            public void MinSize_Should_SetCustomMessage_When_LongType()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.MinSize((long)3, "{min} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{min} Overriden error message", "3 Overriden error message");
            }

            [Fact]
            public void NotEmpty_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.NotEmpty("Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(Array.Empty<int>()), builder.Rules, "Overriden error message", "Overriden error message");
            }

            [Fact]
            public void SizeBetween_Should_SetCustomMessage()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.SizeBetween(3, 4, "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{min} {max} Overriden error message", "3 4 Overriden error message");
            }

            [Fact]
            public void SizeBetween_Should_SetCustomMessage_When_LongType()
            {
                var builder = new MemberSpecificationBuilder<object, Collection<int>>();

                builder.SizeBetween(3, (long)4, "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{min} {max} Overriden error message", "3 4 Overriden error message");
            }
        }
    }
}