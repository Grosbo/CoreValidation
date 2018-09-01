using System;
using CoreValidation.Errors.Args;
using Xunit;

namespace CoreValidation.UnitTests.Errors.Args
{
    public class MessageArgTests
    {
        private class TestClass
        {
            public string Value { get; set; }

            public override string ToString()
            {
                return Value + " TEST!";
            }
        }

        [Fact]
        public void Should_Initialize()
        {
            var arg = new MessageArg<TestClass>("name", new TestClass {Value = "value"});

            Assert.Equal("name", arg.Name);
            Assert.Empty(arg.AllowedParameters);
        }

        [Fact]
        public void Should_StringifyDefaultValues()
        {
            var arg = new MessageArg<TestClass>("name", new TestClass {Value = "value"});

            Assert.Equal("name", arg.Name);
            Assert.Equal("value TEST!", arg.ToString(null));
        }

        [Fact]
        public void Should_ThrowException_When_NullName()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => { new MessageArg(null, "value"); });
        }

        [Fact]
        public void Should_ThrowException_When_NullValue()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => { new MessageArg("name", null); });
        }
    }
}