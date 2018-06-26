using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Specifications;
using CoreValidation.Translations;
using Xunit;

namespace CoreValidation.PredefinedRules.UnitTests.Collections
{
    public class BaseCollectionRulesTests
    {
        private static readonly Func<int[], CustomCollection> _convert = array => new CustomCollection(array);

        public static IEnumerable<object[]> ExactSize_Should_CollectError_Data()
        {
            return CollectionDataHelper.ExactSize_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(ExactSize_Should_CollectError_Data))]
        public void ExactSize_Should_CollectError(CustomCollection items, int expectedSize, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            builder.ExactSize<object, CustomCollection, int>(expectedSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.ExactSize);
        }

        [Theory]
        [MemberData(nameof(ExactSize_Should_CollectError_Data))]
        public void ExactSize_Should_CollectError_When_LongType(CustomCollection items, long expectedSize, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            builder.ExactSize<object, CustomCollection, int>(expectedSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.ExactSize);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void ExactSize_Should_ThrowException_When_NegativeSize(int expectedSize)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.ExactSize<object, CustomCollection, int>(expectedSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void ExactSize_Should_ThrowException_When_NegativeSize_And_LongType(long expectedSize)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.ExactSize<object, CustomCollection, int>(expectedSize); });
        }

        public static IEnumerable<object[]> NotEmpty_Should_CollectError_Data()
        {
            return CollectionDataHelper.NotEmpty_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(NotEmpty_Should_CollectError_Data))]
        public void NotEmpty_Should_CollectError(CustomCollection items, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            builder.NotEmpty<object, CustomCollection, int>();

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.NotEmpty);
        }

        public static IEnumerable<object[]> Empty_Should_CollectError_Data()
        {
            return CollectionDataHelper.Empty_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(Empty_Should_CollectError_Data))]
        public void Empty_Should_CollectError(CustomCollection items, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            builder.Empty<object, CustomCollection, int>();

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.Empty);
        }

        public static IEnumerable<object[]> MaxSize_Should_CollectError_Data()
        {
            return CollectionDataHelper.MaxSize_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MaxSize_Should_CollectError_Data))]
        public void MaxSize_Should_CollectError(CustomCollection items, int maxSize, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            builder.MaxSize<object, CustomCollection, int>(maxSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.MaxSize);
        }

        public static IEnumerable<object[]> MaxSize_Should_CollectError_When_LongSize_Data()
        {
            return CollectionDataHelper.MaxSize_Should_CollectError_When_LongSize_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MaxSize_Should_CollectError_Data))]
        [MemberData(nameof(MaxSize_Should_CollectError_When_LongSize_Data))]
        public void MaxSize_Should_CollectError_When_LongSize(CustomCollection items, long maxSize, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            builder.MaxSize<object, CustomCollection, int>(maxSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.MaxSize);
        }

        public static IEnumerable<object[]> MinSize_Should_CollectError_Data()
        {
            return CollectionDataHelper.MinSize_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MinSize_Should_CollectError_Data))]
        public void MinSize_Should_CollectError(CustomCollection items, int minSize, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            builder.MinSize<object, CustomCollection, int>(minSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.MinSize);
        }

        public static IEnumerable<object[]> MinSize_Should_CollectError_When_LongSize_Data()
        {
            return CollectionDataHelper.MinSize_Should_CollectError_When_LongSize_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(MinSize_Should_CollectError_Data))]
        [MemberData(nameof(MinSize_Should_CollectError_When_LongSize_Data))]
        public void MinSize_Should_CollectError_When_LongSize(CustomCollection items, long minSize, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            builder.MinSize<object, CustomCollection, int>(minSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.MinSize);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void MinSize_Should_ThrowException_When_NegativeSize(int minSize)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.MinSize<object, CustomCollection, int>(minSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void MinSize_Should_ThrowException_When_NegativeSize_And_LongSize(long minSize)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.MinSize<object, CustomCollection, int>(minSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void MaxSize_Should_ThrowException_When_NegativeSize(int maxSize)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.MaxSize<object, CustomCollection, int>(maxSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void MaxSize_Should_ThrowException_When_NegativeSize_And_LongSize(long maxSize)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.MaxSize<object, CustomCollection, int>(maxSize); });
        }

        public static IEnumerable<object[]> SizeBetween_Should_CollectError_Data()
        {
            return CollectionDataHelper.SizeBetween_Should_CollectError_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(SizeBetween_Should_CollectError_Data))]
        public void SizeBetween_Should_CollectError(CustomCollection items, int minSize, int maxSize, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            builder.SizeBetween<object, CustomCollection, int>(minSize, maxSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.SizeBetween);
        }

        public static IEnumerable<object[]> SizeBetween_Should_CollectError_When_LongSize_Data()
        {
            return CollectionDataHelper.SizeBetween_Should_CollectError_When_LongSize_Data(_convert);
        }

        [Theory]
        [MemberData(nameof(SizeBetween_Should_CollectError_Data))]
        [MemberData(nameof(SizeBetween_Should_CollectError_When_LongSize_Data))]
        public void SizeBetween_Should_CollectError_When_LongSize(CustomCollection items, long minSize, long maxSize, bool expectedIsValid)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            builder.SizeBetween<object, CustomCollection, int>(minSize, maxSize);

            RulesHelper.AssertErrorCompilation(items, builder.Rules, expectedIsValid, Phrases.Keys.Collections.SizeBetween);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void SizeBetween_Should_ThrowException_When_MaxSizeIsNegative(int maxSize)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.SizeBetween<object, CustomCollection, int>(0, maxSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void SizeBetween_Should_ThrowException_When_MaxSizeIsLongNegative(long maxSize)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.SizeBetween<object, CustomCollection, int>(0, maxSize); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void SizeBetween_Should_ThrowException_When_MinSizeIsNegative(int minSize)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.SizeBetween<object, CustomCollection, int>(minSize, 10); });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(long.MinValue)]
        public void SizeBetween_Should_ThrowException_When_MinSizeIsLongNegative(long minSize)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.SizeBetween<object, CustomCollection, int>(minSize, 10L); });
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(20, 0)]
        [InlineData(int.MaxValue, 1)]
        public void SizeBetween_Should_ThrowException_When_MinLargerThanMax(int minSize, int maxSize)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.SizeBetween<object, CustomCollection, int>(minSize, maxSize); });
        }

        [Theory]
        [InlineData(1, 0)]
        [InlineData(20, 0)]
        [InlineData(long.MaxValue, 1)]
        public void SizeBetween_Should_ThrowException_When_MinLargerThanMax_And_LongMinAndMax(long minSize, long maxSize)
        {
            var builder = new MemberSpecification<object, CustomCollection>();

            Assert.Throws<ArgumentOutOfRangeException>(() => { builder.SizeBetween<object, CustomCollection, int>(minSize, maxSize); });
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

        public class MessageTests
        {
            [Fact]
            public void Empty_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, CustomCollection>();

                builder.Empty<object, CustomCollection, int>("Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "Overriden error message", "Overriden error message");
            }

            [Fact]
            public void ExactSize_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, CustomCollection>();

                builder.ExactSize<object, CustomCollection, int>(10, "{size} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{size} Overriden error message", "10 Overriden error message");
            }

            [Fact]
            public void ExactSize_Should_SetCustomMessage_When_LongType()
            {
                var builder = new MemberSpecification<object, CustomCollection>();

                builder.ExactSize<object, CustomCollection, int>((long)10, "{size} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{size} Overriden error message", "10 Overriden error message");
            }

            [Fact]
            public void MaxSize_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, CustomCollection>();

                builder.MaxSize<object, CustomCollection, int>(3, "{max} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1, 2, 3, 4}), builder.Rules, "{max} Overriden error message", "3 Overriden error message");
            }

            [Fact]
            public void MaxSize_Should_SetCustomMessage_When_LongType()
            {
                var builder = new MemberSpecification<object, CustomCollection>();

                builder.MaxSize<object, CustomCollection, int>((long)3, "{max} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1, 2, 3, 4}), builder.Rules, "{max} Overriden error message", "3 Overriden error message");
            }

            [Fact]
            public void MinSize_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, CustomCollection>();

                builder.MinSize<object, CustomCollection, int>(3, "{min} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{min} Overriden error message", "3 Overriden error message");
            }

            [Fact]
            public void MinSize_Should_SetCustomMessage_When_LongType()
            {
                var builder = new MemberSpecification<object, CustomCollection>();

                builder.MinSize<object, CustomCollection, int>((long)3, "{min} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{min} Overriden error message", "3 Overriden error message");
            }

            [Fact]
            public void NotEmpty_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, CustomCollection>();

                builder.NotEmpty<object, CustomCollection, int>("Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(Array.Empty<int>()), builder.Rules, "Overriden error message", "Overriden error message");
            }

            [Fact]
            public void SizeBetween_Should_SetCustomMessage()
            {
                var builder = new MemberSpecification<object, CustomCollection>();

                builder.SizeBetween<object, CustomCollection, int>(3, 4, "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{min} {max} Overriden error message", "3 4 Overriden error message");
            }

            [Fact]
            public void SizeBetween_Should_SetCustomMessage_When_LongType()
            {
                var builder = new MemberSpecification<object, CustomCollection>();

                builder.SizeBetween<object, CustomCollection, int>(3, (long)4, "{min} {max} Overriden error message");

                RulesHelper.AssertErrorMessage(_convert(new[] {1}), builder.Rules, "{min} {max} Overriden error message", "3 4 Overriden error message");
            }
        }
    }
}