using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CoreValidation.UnitTests.Errors.Args
{
    public class EnumArgTests
    {
        [Theory]
        [InlineData(StringComparison.Ordinal, "G", "Ordinal")]
        [InlineData(StringComparison.Ordinal, "D", "4")]
        [InlineData(StringComparison.Ordinal, "X", "00000004")]
        public void Should_Stringify(StringComparison stringComparison, string format, string expectedString)
        {
            var arg = Arg.Enum("name", stringComparison);

            var stringified = arg.ToString(new Dictionary<string, string>
            {
                {"format", format}
            });

            Assert.Equal(expectedString, stringified);
        }

        [Fact]
        public void Should_Initialize()
        {
            var arg = Arg.Enum("name", StringComparison.CurrentCulture);

            Assert.Equal("name", arg.Name);
            Assert.Equal("format", arg.AllowedParameters.Single());
        }

        [Fact]
        public void Should_StringifyDefaultValues()
        {
            var arg = Arg.Enum("name", StringComparison.CurrentCulture);

            Assert.Equal("name", arg.Name);
            Assert.Equal("CurrentCulture", arg.ToString(null));
        }

        [Fact]
        public void Should_ThrowException_When_NullName()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => { Arg.Enum(null, StringComparison.CurrentCulture); });
        }
    }
}