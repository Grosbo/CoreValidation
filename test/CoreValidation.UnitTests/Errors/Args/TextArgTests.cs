using System;
using System.Collections.Generic;
using CoreValidation.Errors.Args;
using Xunit;

// ReSharper disable ObjectCreationAsStatement

namespace CoreValidation.UnitTests.Errors.Args
{
    public class TextArgTests
    {
        [Theory]
        [InlineData("TeSt", null, "TeSt")]
        [InlineData("TeSt", "upper", "TEST")]
        [InlineData("TeSt", "lower", "test")]
        [InlineData("TeSt", "something", "TeSt")]
        public void Should_Stringify_String(string value, string caseParameter, string expectedString)
        {
            var arg = new TextArg("name", value);

            var stringified = arg.ToString(caseParameter != null
                ? new Dictionary<string, string>
                {
                    {"case", caseParameter}
                }
                : null);

            Assert.Equal(expectedString, stringified);
        }

        [Theory]
        [InlineData('t', null, "t")]
        [InlineData('t', "upper", "T")]
        [InlineData('T', "upper", "T")]
        [InlineData('t', "lower", "t")]
        [InlineData('T', "lower", "t")]
        public void Should_Stringify_Char(char value, string caseParameter, string expectedString)
        {
            var arg = new TextArg("name", value);

            var stringified = arg.ToString(caseParameter != null
                ? new Dictionary<string, string>
                {
                    {"case", caseParameter}
                }
                : null);

            Assert.Equal(expectedString, stringified);
        }

        [Fact]
        public void Should_Initialize()
        {
            var arg = new TextArg("name", "value");

            Assert.Equal("name", arg.Name);
            Assert.Equal(1, arg.AllowedParameters.Count);
            Assert.Contains("case", arg.AllowedParameters);
        }

        [Fact]
        public void Should_ThrowException_When_NullName()
        {
            Assert.Throws<ArgumentNullException>(() => { new TextArg(null, "value"); });
            Assert.Throws<ArgumentNullException>(() => { new TextArg(null, 'v'); });
        }

        [Fact]
        public void Should_ThrowException_When_NullValue()
        {
            Assert.Throws<ArgumentNullException>(() => { new TextArg("name", null); });
        }
    }
}