using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using Xunit;

namespace CoreValidation.UnitTests.Errors
{
    public class ErrorTests
    {
        public static IEnumerable<object[]> Should_EqualContent_ReturnTrue_Data()
        {
            yield return new object[]
            {
                new Error("message"),
                new Error("message")
            };

            yield return new object[]
            {
                new Error("message {arg}", new IMessageArg[] {new MessageArg("arg", "value")}),
                new Error("message {arg}", new IMessageArg[] {new MessageArg("arg", "value")})
            };

            yield return new object[]
            {
                new Error("message {arg} {arg2}", new IMessageArg[] {new MessageArg("arg", "value"), new MessageArg("arg2", "value2")}),
                new Error("message {arg} value2", new IMessageArg[] {new MessageArg("arg", "value")})
            };
        }

        [Theory]
        [MemberData(nameof(Should_EqualContent_ReturnTrue_Data))]
        public void Should_EqualContent_ReturnTrue(Error e1, Error e2)
        {
            Assert.True(e1.EqualContent(e2));
        }

        public static IEnumerable<object[]> Should_EqualContent_ReturnFalse_Data()
        {
            yield return new object[]
            {
                new Error("message 123"),
                new Error("message 321")
            };

            yield return new object[]
            {
                new Error("MESSAGE"),
                new Error("message")
            };

            yield return new object[]
            {
                new Error("message {arg}", new IMessageArg[] {new MessageArg("arg", "VALUE")}),
                new Error("message {arg}", new IMessageArg[] {new MessageArg("arg", "value")})
            };

            yield return new object[]
            {
                new Error("message {arg} {arg2}", new IMessageArg[] {new MessageArg("arg", "value"), new MessageArg("arg2", "value2")}),
                new Error("message {arg} VALUE", new IMessageArg[] {new MessageArg("arg", "value")})
            };
        }

        [Theory]
        [MemberData(nameof(Should_EqualContent_ReturnFalse_Data))]
        public void Should_EqualContent_ReturnFalse(Error e1, Error e2)
        {
            Assert.False(e1.EqualContent(e2));
        }

        [Fact]
        public void Should_SetMessages_When_EmptyArgs()
        {
            var args = Array.Empty<IMessageArg>();

            var error = new Error("test123 {arg}", args);

            Assert.Equal("test123 {arg}", error.Message);
            Assert.Equal("test123 {arg}", error.StringifiedMessage);
            Assert.Equal(0, error.Arguments.Count);
        }

        [Fact]
        public void Should_SetMessages_When_OnlyMessageSet()
        {
            var error = new Error("test123 {arg}");

            Assert.Equal("test123 {arg}", error.Message);
            Assert.Equal("test123 {arg}", error.StringifiedMessage);
            Assert.Null(error.Arguments);
        }

        [Fact]
        public void Should_SetMessages_When_SingleArg()
        {
            var arg = new MessageArg("arg", "value");
            var args = new IMessageArg[] {arg};

            var error = new Error("test123 {arg}", args);

            Assert.Equal("test123 {arg}", error.Message);
            Assert.Equal("test123 value", error.StringifiedMessage);
            Assert.Equal(1, error.Arguments.Count);
            Assert.Same(arg, error.Arguments.Single());
        }

        [Fact]
        public void Should_ThrowException_When_NullMessage()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => { new Error(null); });
        }
    }
}