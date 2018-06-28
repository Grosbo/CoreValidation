using System;
using System.Collections.Generic;
using System.Globalization;
using CoreValidation.Errors.Args;
using Xunit;

// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable ObjectCreationAsStatement

namespace CoreValidation.UnitTests.Errors.Args
{
    public class NumberArgTests
    {
        public static IEnumerable<object[]> Should_Stringify_Numbers_WithFormatAndCulture_Data()
        {
            yield return new object[] {new NumberArg("name", 123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {new NumberArg("name", (uint)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {new NumberArg("name", (short)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {new NumberArg("name", (ushort)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {new NumberArg("name", (long)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {new NumberArg("name", (ulong)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {new NumberArg("name", (byte)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {new NumberArg("name", (sbyte)123), "0.00", "pl-PL", "123,00"};

            yield return new object[] {new NumberArg("name", (decimal)123.987), "0.00", "pl-PL", "123,99"};
            yield return new object[] {new NumberArg("name", 123.987), "0.00", "pl-PL", "123,99"};
            yield return new object[] {new NumberArg("name", (float)123.987), "0.00", "pl-PL", "123,99"};

            yield return new object[] {new NumberArg("name", Guid.Empty), "N", "pl-PL", "00000000000000000000000000000000"};

            yield return new object[] {new NumberArg("name", new DateTime(2000, 01, 15, 16, 04, 05, 06)), "s", "en-US", "2000-01-15T16:04:05"};
            yield return new object[] {new NumberArg("name", new DateTimeOffset(2000, 01, 15, 16, 04, 05, 06, TimeSpan.Zero)), "s", "en-US", "2000-01-15T16:04:05"};
            yield return new object[] {new NumberArg("name", new TimeSpan(1, 2, 3)), "g", "en-US", "1:02:03"};
        }

        [Theory]
        [MemberData(nameof(Should_Stringify_Numbers_WithFormatAndCulture_Data))]
        public void Should_Stringify_Numbers_WithFormatAndCulture(NumberArg arg, string format, string culture, string expectedString)
        {
            var stringified = arg.ToString(new Dictionary<string, string> {{"format", format}, {"culture", culture}});

            Assert.Equal(expectedString, stringified);
        }

        public static IEnumerable<object[]> Should_Stringify_Dates_WithCulture_Data()
        {
            yield return new object[] {new NumberArg("name", 123), "pl-PL", "123"};
            yield return new object[] {new NumberArg("name", (uint)123), "pl-PL", "123"};
            yield return new object[] {new NumberArg("name", (short)123), "pl-PL", "123"};
            yield return new object[] {new NumberArg("name", (ushort)123), "pl-PL", "123"};
            yield return new object[] {new NumberArg("name", (long)123), "pl-PL", "123"};
            yield return new object[] {new NumberArg("name", (ulong)123), "pl-PL", "123"};
            yield return new object[] {new NumberArg("name", (byte)123), "pl-PL", "123"};
            yield return new object[] {new NumberArg("name", (sbyte)123), "pl-PL", "123"};

            yield return new object[] {new NumberArg("name", (decimal)123.987), "pl-PL", "123,987"};
            yield return new object[] {new NumberArg("name", 123.987), "pl-PL", "123,987"};
            yield return new object[] {new NumberArg("name", (float)123.987), "pl-PL", "123,987"};

            yield return new object[] {new NumberArg("name", Guid.Empty), "pl-PL", "00000000-0000-0000-0000-000000000000"};

            yield return new object[] {new NumberArg("name", new DateTime(2000, 01, 15, 16, 04, 05, 06)), "en-US", new DateTime(2000, 01, 15, 16, 04, 05, 06).ToString(CultureInfo.GetCultureInfo("en-US"))};
            yield return new object[] {new NumberArg("name", new DateTimeOffset(2000, 01, 15, 16, 04, 05, 06, TimeSpan.Zero)), "en-US", new DateTimeOffset(2000, 01, 15, 16, 04, 05, 06, TimeSpan.Zero).ToString(CultureInfo.GetCultureInfo("en-US"))};

            yield return new object[] {new NumberArg("name", new TimeSpan(1, 2, 3)), "en-US", "01:02:03"};
        }

        [Theory]
        [MemberData(nameof(Should_Stringify_Dates_WithCulture_Data))]
        public void Should_Stringify_Dates_WithCulture(NumberArg arg, string culture, string expectedString)
        {
            var stringified = arg.ToString(new Dictionary<string, string> {{"culture", culture}});

            Assert.Equal(expectedString, stringified);
        }

        public static IEnumerable<object[]> Should_Stringify_WithFormat_Data()
        {
            yield return new object[] {new NumberArg("name", 123), "0.00", "123.00"};
            yield return new object[] {new NumberArg("name", (uint)123), "0.00", "123.00"};
            yield return new object[] {new NumberArg("name", (short)123), "0.00", "123.00"};
            yield return new object[] {new NumberArg("name", (ushort)123), "0.00", "123.00"};
            yield return new object[] {new NumberArg("name", (long)123), "0.00", "123.00"};
            yield return new object[] {new NumberArg("name", (ulong)123), "0.00", "123.00"};
            yield return new object[] {new NumberArg("name", (byte)123), "0.00", "123.00"};
            yield return new object[] {new NumberArg("name", (sbyte)123), "0.00", "123.00"};

            yield return new object[] {new NumberArg("name", (decimal)123.987), "0.00", "123.99"};
            yield return new object[] {new NumberArg("name", 123.987), "0.00", "123.99"};
            yield return new object[] {new NumberArg("name", (float)123.987), "0.00", "123.99"};

            yield return new object[] {new NumberArg("name", Guid.Empty), "N", "00000000000000000000000000000000"};

            yield return new object[] {new NumberArg("name", new DateTime(2000, 01, 15, 16, 04, 05, 06)), "s", "2000-01-15T16:04:05"};
            yield return new object[] {new NumberArg("name", new DateTimeOffset(2000, 01, 15, 16, 04, 05, 06, TimeSpan.Zero)), "s", "2000-01-15T16:04:05"};
            yield return new object[] {new NumberArg("name", new TimeSpan(1, 2, 3)), "g", "1:02:03"};
        }

        [Theory]
        [MemberData(nameof(Should_Stringify_WithFormat_Data))]
        public void Should_Stringify_WithFormat(NumberArg arg, string format, string expectedString)
        {
            var stringified = arg.ToString(new Dictionary<string, string> {{"format", format}});

            Assert.Equal(expectedString, stringified);
        }

        public static IEnumerable<object[]> Should_Stringify_Default_Data()
        {
            yield return new object[] {new NumberArg("name", 123), "123"};
            yield return new object[] {new NumberArg("name", (uint)123), "123"};
            yield return new object[] {new NumberArg("name", (short)123), "123"};
            yield return new object[] {new NumberArg("name", (ushort)123), "123"};
            yield return new object[] {new NumberArg("name", (long)123), "123"};
            yield return new object[] {new NumberArg("name", (ulong)123), "123"};
            yield return new object[] {new NumberArg("name", (byte)123), "123"};
            yield return new object[] {new NumberArg("name", (sbyte)123), "123"};

            yield return new object[] {new NumberArg("name", (decimal)123.987), "123.987"};
            yield return new object[] {new NumberArg("name", 123.987), "123.987"};
            yield return new object[] {new NumberArg("name", (float)123.987), "123.987"};

            yield return new object[] {new NumberArg("name", Guid.Empty), "00000000-0000-0000-0000-000000000000"};

            yield return new object[] {new NumberArg("name", new DateTime(2000, 01, 15, 16, 04, 05, 06)), "2000-01-15 16:04:05.006"};
            yield return new object[] {new NumberArg("name", new DateTimeOffset(2000, 01, 15, 16, 04, 05, 06, TimeSpan.Zero)), "2000-01-15 16:04:05.006"};
            yield return new object[] {new NumberArg("name", new TimeSpan(1, 2, 3)), "01:02:03"};
        }

        [Theory]
        [MemberData(nameof(Should_Stringify_Default_Data))]
        public void Should_Stringify_Default(NumberArg arg, string expectedString)
        {
            var stringified = arg.ToString(null);

            Assert.Equal(expectedString, stringified);
        }

        [Fact]
        public void Should_Initialize()
        {
            var arg = new NumberArg("name", 1);

            Assert.Equal("name", arg.Name);
            Assert.Equal(2, arg.AllowedParameters.Count);
            Assert.Contains("format", arg.AllowedParameters);
            Assert.Contains("culture", arg.AllowedParameters);
        }

        [Fact]
        public void Should_ThrowException_When_NullName()
        {
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, 0); });
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, (uint)0); });
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, (short)0); });
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, (ushort)0); });
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, (long)0); });
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, (ulong)0); });
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, (double)0); });
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, (float)0); });
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, (byte)0); });
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, (sbyte)0); });
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, (decimal)0); });
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, Guid.Empty); });
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, TimeSpan.FromTicks(0)); });
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, new DateTime(0)); });
            Assert.Throws<ArgumentNullException>(() => { new NumberArg(null, new DateTimeOffset(0, TimeSpan.Zero)); });
        }
    }
}