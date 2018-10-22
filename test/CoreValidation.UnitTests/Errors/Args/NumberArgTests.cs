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
            yield return new object[] {NumberArg.Create("name", 123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {NumberArg.Create("name", (uint)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {NumberArg.Create("name", (short)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {NumberArg.Create("name", (ushort)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {NumberArg.Create("name", (long)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {NumberArg.Create("name", (ulong)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {NumberArg.Create("name", (byte)123), "0.00", "pl-PL", "123,00"};
            yield return new object[] {NumberArg.Create("name", (sbyte)123), "0.00", "pl-PL", "123,00"};

            yield return new object[] {NumberArg.Create("name", (decimal)123.987), "0.00", "pl-PL", "123,99"};
            yield return new object[] {NumberArg.Create("name", 123.987), "0.00", "pl-PL", "123,99"};
            yield return new object[] {NumberArg.Create("name", (float)123.987), "0.00", "pl-PL", "123,99"};
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
            yield return new object[] {NumberArg.Create("name", 123), "pl-PL", "123"};
            yield return new object[] {NumberArg.Create("name", (uint)123), "pl-PL", "123"};
            yield return new object[] {NumberArg.Create("name", (short)123), "pl-PL", "123"};
            yield return new object[] {NumberArg.Create("name", (ushort)123), "pl-PL", "123"};
            yield return new object[] {NumberArg.Create("name", (long)123), "pl-PL", "123"};
            yield return new object[] {NumberArg.Create("name", (ulong)123), "pl-PL", "123"};
            yield return new object[] {NumberArg.Create("name", (byte)123), "pl-PL", "123"};
            yield return new object[] {NumberArg.Create("name", (sbyte)123), "pl-PL", "123"};

            yield return new object[] {NumberArg.Create("name", (decimal)123.987), "pl-PL", "123,987"};
            yield return new object[] {NumberArg.Create("name", 123.987), "pl-PL", "123,987"};
            yield return new object[] {NumberArg.Create("name", (float)123.987), "pl-PL", "123,987"};
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
            yield return new object[] {NumberArg.Create("name", 123), "0.00", "123.00"};
            yield return new object[] {NumberArg.Create("name", (uint)123), "0.00", "123.00"};
            yield return new object[] {NumberArg.Create("name", (short)123), "0.00", "123.00"};
            yield return new object[] {NumberArg.Create("name", (ushort)123), "0.00", "123.00"};
            yield return new object[] {NumberArg.Create("name", (long)123), "0.00", "123.00"};
            yield return new object[] {NumberArg.Create("name", (ulong)123), "0.00", "123.00"};
            yield return new object[] {NumberArg.Create("name", (byte)123), "0.00", "123.00"};
            yield return new object[] {NumberArg.Create("name", (sbyte)123), "0.00", "123.00"};

            yield return new object[] {NumberArg.Create("name", (decimal)123.987), "0.00", "123.99"};
            yield return new object[] {NumberArg.Create("name", 123.987), "0.00", "123.99"};
            yield return new object[] {NumberArg.Create("name", (float)123.987), "0.00", "123.99"};
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
            yield return new object[] {NumberArg.Create("name", 123), "123"};
            yield return new object[] {NumberArg.Create("name", (uint)123), "123"};
            yield return new object[] {NumberArg.Create("name", (short)123), "123"};
            yield return new object[] {NumberArg.Create("name", (ushort)123), "123"};
            yield return new object[] {NumberArg.Create("name", (long)123), "123"};
            yield return new object[] {NumberArg.Create("name", (ulong)123), "123"};
            yield return new object[] {NumberArg.Create("name", (byte)123), "123"};
            yield return new object[] {NumberArg.Create("name", (sbyte)123), "123"};

            yield return new object[] {NumberArg.Create("name", (decimal)123.987), "123.987"};
            yield return new object[] {NumberArg.Create("name", 123.987), "123.987"};
            yield return new object[] {NumberArg.Create("name", (float)123.987), "123.987"};
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
            var arg = NumberArg.Create("name", 1);

            Assert.Equal("name", arg.Name);
            Assert.Equal(2, arg.AllowedParameters.Count);
            Assert.Contains("format", arg.AllowedParameters);
            Assert.Contains("culture", arg.AllowedParameters);
        }

        [Fact]
        public void Should_ThrowException_When_NullName()
        {
            Assert.Throws<ArgumentNullException>(() => { NumberArg.Create(null, 0); });
            Assert.Throws<ArgumentNullException>(() => { NumberArg.Create(null, (uint)0); });
            Assert.Throws<ArgumentNullException>(() => { NumberArg.Create(null, (short)0); });
            Assert.Throws<ArgumentNullException>(() => { NumberArg.Create(null, (ushort)0); });
            Assert.Throws<ArgumentNullException>(() => { NumberArg.Create(null, (long)0); });
            Assert.Throws<ArgumentNullException>(() => { NumberArg.Create(null, (ulong)0); });
            Assert.Throws<ArgumentNullException>(() => { NumberArg.Create(null, (double)0); });
            Assert.Throws<ArgumentNullException>(() => { NumberArg.Create(null, (float)0); });
            Assert.Throws<ArgumentNullException>(() => { NumberArg.Create(null, (byte)0); });
            Assert.Throws<ArgumentNullException>(() => { NumberArg.Create(null, (sbyte)0); });
            Assert.Throws<ArgumentNullException>(() => { NumberArg.Create(null, (decimal)0); });
        }
    }
}