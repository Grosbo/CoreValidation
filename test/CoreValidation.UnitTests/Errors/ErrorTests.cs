using System;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using Xunit;

namespace CoreValidation.UnitTests.Errors
{
    public class ErrorTests
    {
        [Fact]
        public void Should_SetMessages_When_EmptyArgs()
        {
            var args = Array.Empty<IMessageArg>();

            var error = new Error("test123 {arg}", args);

            Assert.Equal("test123 {arg}", error.Message);
            Assert.Equal("test123 {arg}", error.ToFormattedMessage());
            Assert.Equal(0, error.Arguments.Count);
        }

        [Fact]
        public void Should_SetMessages_When_OnlyMessageSet()
        {
            var error = new Error("test123 {arg}");

            Assert.Equal("test123 {arg}", error.Message);
            Assert.Equal("test123 {arg}", error.ToFormattedMessage());
            Assert.Null(error.Arguments);
        }

        [Fact]
        public void Should_SetMessages_When_SingleArg()
        {
            var arg = new MessageArg("arg", "value");
            var args = new IMessageArg[] {arg};

            var error = new Error("test123 {arg}", args);

            Assert.Equal("test123 {arg}", error.Message);
            Assert.Equal("test123 value", error.ToFormattedMessage());
            Assert.Equal(1, error.Arguments.Count);
            Assert.Same(arg, error.Arguments.Single());
        }

        [Fact]
        public void Should_ThrowException_When_NullInArgs()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentException>(() => { new Error("message", new IMessageArg[] {null}); });
        }

        [Fact]
        public void Should_ThrowException_When_NullMessage()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => { new Error(null); });
        }
    }
}