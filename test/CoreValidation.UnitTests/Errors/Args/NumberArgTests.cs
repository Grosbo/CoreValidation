using System;
using System.Collections.Generic;
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
            yield return new object[] {Arg.Number("name", 123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {Arg.Number("name", (uint)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {Arg.Number("name", (short)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {Arg.Number("name", (ushort)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {Arg.Number("name", (long)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {Arg.Number("name", (ulong)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {Arg.Number("name", (byte)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {Arg.Number("name", (sbyte)123), "0.00", "pl-PL", "123,00"};

            yield return new object[] {Arg.Number("name", (decimal)123.987), "0.00", "pl-PL", "123,99"};
            yield return new object[] {Arg.Number("name", 123.987), "0.00", "pl-PL", "123,99"};
            yield return new object[] {Arg.Number("name", (float)123.987), "0.00", "pl-PL", "123,99"};
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
            yield return new object[] {Arg.Number("name", 123), "pl-PL", "123"};
            yield return new object[] {Arg.Number("name", (uint)123), "pl-PL", "123"};
            yield return new object[] {Arg.Number("name", (short)123), "pl-PL", "123"};
            yield return new object[] {Arg.Number("name", (ushort)123), "pl-PL", "123"};
            yield return new object[] {Arg.Number("name", (long)123), "pl-PL", "123"};
            yield return new object[] {Arg.Number("name", (ulong)123), "pl-PL", "123"};
            yield return new object[] {Arg.Number("name", (byte)123), "pl-PL", "123"};
            yield return new object[] {Arg.Number("name", (sbyte)123), "pl-PL", "123"};

            yield return new object[] {Arg.Number("name", (decimal)123.987), "pl-PL", "123,987"};
            yield return new object[] {Arg.Number("name", 123.987), "pl-PL", "123,987"};
            yield return new object[] {Arg.Number("name", (float)123.987), "pl-PL", "123,987"};
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
            yield return new object[] {Arg.Number("name", 123), "0.00", "123.00"};
            yield return new object[] {Arg.Number("name", (uint)123), "0.00", "123.00"};
            yield return new object[] {Arg.Number("name", (short)123), "0.00", "123.00"};
            yield return new object[] {Arg.Number("name", (ushort)123), "0.00", "123.00"};
            yield return new object[] {Arg.Number("name", (long)123), "0.00", "123.00"};
            yield return new object[] {Arg.Number("name", (ulong)123), "0.00", "123.00"};
            yield return new object[] {Arg.Number("name", (byte)123), "0.00", "123.00"};
            yield return new object[] {Arg.Number("name", (sbyte)123), "0.00", "123.00"};

            yield return new object[] {Arg.Number("name", (decimal)123.987), "0.00", "123.99"};
            yield return new object[] {Arg.Number("name", 123.987), "0.00", "123.99"};
            yield return new object[] {Arg.Number("name", (float)123.987), "0.00", "123.99"};
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
            yield return new object[] {Arg.Number("name", 123), "123"};
            yield return new object[] {Arg.Number("name", (uint)123), "123"};
            yield return new object[] {Arg.Number("name", (short)123), "123"};
            yield return new object[] {Arg.Number("name", (ushort)123), "123"};
            yield return new object[] {Arg.Number("name", (long)123), "123"};
            yield return new object[] {Arg.Number("name", (ulong)123), "123"};
            yield return new object[] {Arg.Number("name", (byte)123), "123"};
            yield return new object[] {Arg.Number("name", (sbyte)123), "123"};

            yield return new object[] {Arg.Number("name", (decimal)123.987), "123.987"};
            yield return new object[] {Arg.Number("name", 123.987), "123.987"};
            yield return new object[] {Arg.Number("name", (float)123.987), "123.987"};
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
            var arg = Arg.Number("name", 1);

            Assert.Equal("name", arg.Name);
            Assert.Equal(2, arg.AllowedParameters.Count);
            Assert.Contains("format", arg.AllowedParameters);
            Assert.Contains("culture", arg.AllowedParameters);
        }

        [Fact]
        public void Should_ThrowException_When_NullName()
        {
            Assert.Throws<ArgumentNullException>(() => { Arg.Number(null, 0); });
            Assert.Throws<ArgumentNullException>(() => { Arg.Number(null, (uint)0); });
            Assert.Throws<ArgumentNullException>(() => { Arg.Number(null, (short)0); });
            Assert.Throws<ArgumentNullException>(() => { Arg.Number(null, (ushort)0); });
            Assert.Throws<ArgumentNullException>(() => { Arg.Number(null, (long)0); });
            Assert.Throws<ArgumentNullException>(() => { Arg.Number(null, (ulong)0); });
            Assert.Throws<ArgumentNullException>(() => { Arg.Number(null, (double)0); });
            Assert.Throws<ArgumentNullException>(() => { Arg.Number(null, (float)0); });
            Assert.Throws<ArgumentNullException>(() => { Arg.Number(null, (byte)0); });
            Assert.Throws<ArgumentNullException>(() => { Arg.Number(null, (sbyte)0); });
            Assert.Throws<ArgumentNullException>(() => { Arg.Number(null, (decimal)0); });
        }
    }
}