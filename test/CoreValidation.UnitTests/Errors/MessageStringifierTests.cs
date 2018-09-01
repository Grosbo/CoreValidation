using System;
using System.Collections.Generic;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using Xunit;

namespace CoreValidation.UnitTests.Errors
{
    public class MessageStringifierTests
    {
        private class TestArg : IMessageArg
        {
            private readonly Action<IReadOnlyDictionary<string, string>> _parametersCheck;

            public TestArg(Action<IReadOnlyDictionary<string, string>> parametersCheck)
            {
                _parametersCheck = parametersCheck;
            }

            public string Value { get; set; }

            public string Name { get; set; }

            public IReadOnlyCollection<string> AllowedParameters { get; set; }

            public string ToString(IReadOnlyDictionary<string, string> parameters)
            {
                _parametersCheck?.Invoke(parameters);

                return Value;
            }
        }

        [Fact]
        public void Should_NotPassParameters_When_ContainsInvalidParameters()
        {
            var argCount = 0;

            var arg = new TestArg(parameters => { argCount++; }) {Name = "arg", Value = "value", AllowedParameters = new[] {"param"}};

            var result = MessageFormatter.Format("test {arg|param=x|someparameter=somevalue}", new IMessageArg[] {arg});

            Assert.Equal("test {arg|param=x|someparameter=somevalue}", result);
            Assert.Equal(0, argCount);
        }

        [Fact]
        public void Should_PassParameters_When_ManyArguments()
        {
            var argCount = 0;

            var arg = new TestArg(parameters =>
            {
                Assert.Equal(3, parameters.Count);
                Assert.Equal("x", parameters["param"]);
                Assert.Equal("x2", parameters["param2"]);
                Assert.Equal("x3", parameters["param3"]);
                argCount++;
            }) {Name = "arg", Value = "value", AllowedParameters = new[] {"param", "param2", "param3"}};

            var arg2Count = 0;

            var arg2 = new TestArg(parameters =>
            {
                Assert.Equal(1, parameters.Count);
                Assert.Equal("val", parameters["par"]);
                arg2Count++;
            }) {Name = "arg2", Value = "value2", AllowedParameters = new[] {"par"}};

            var result = MessageFormatter.Format("test {arg|param=x|param2=x2|param3=x3} {arg2|par=val}", new IMessageArg[] {arg, arg2});

            Assert.Equal("test value value2", result);
            Assert.Equal(1, argCount);
            Assert.Equal(1, arg2Count);
        }

        [Fact]
        public void Should_PassParameters_When_ManyParameters()
        {
            var argCount = 0;

            var arg = new TestArg(parameters =>
            {
                Assert.Equal(3, parameters.Count);
                Assert.Equal("x", parameters["param"]);
                Assert.Equal("x2", parameters["param2"]);
                Assert.Equal("x3", parameters["param3"]);
                argCount++;
            }) {Name = "arg", Value = "value", AllowedParameters = new[] {"param", "param2", "param3"}};

            var result = MessageFormatter.Format("test {arg|param=x|param2=x2|param3=x3}", new IMessageArg[] {arg});

            Assert.Equal("test value", result);
            Assert.Equal(1, argCount);
        }

        [Fact]
        public void Should_PassParameters_When_SingleParameter()
        {
            var argCount = 0;

            var arg = new TestArg(parameters =>
            {
                Assert.Equal(1, parameters.Count);
                Assert.Equal("x", parameters["param"]);
                argCount++;
            }) {Name = "arg", Value = "value", AllowedParameters = new[] {"param"}};

            var result = MessageFormatter.Format("test {arg|param=x}", new IMessageArg[] {arg});

            Assert.Equal("test value", result);
            Assert.Equal(1, argCount);
        }

        [Fact]
        public void Should_ReturnSameString_When_ArgumentNotUsed()
        {
            var argCount = 0;

            var arg = new TestArg(parameters =>
            {
                Assert.Null(parameters);
                argCount++;
            }) {Name = "arg", Value = "value"};

            var result = MessageFormatter.Format("test", new IMessageArg[] {arg});

            Assert.Equal("test", result);
            Assert.Equal(0, argCount);
        }

        [Fact]
        public void Should_ReturnSameString_When_NoArguments()
        {
            var result = MessageFormatter.Format("test", new IMessageArg[] { });

            Assert.Equal("test", result);
        }

        [Fact]
        public void Should_ReturnSameString_When_NullArguments()
        {
            var result = MessageFormatter.Format("test", null);

            Assert.Equal("test", result);
        }

        [Fact]
        public void Should_ReturnSameString_When_UsingInvalidName()
        {
            var argCount = 0;

            var arg = new TestArg(parameters =>
            {
                Assert.Null(parameters);
                argCount++;
            }) {Name = "arg", Value = "value"};

            var result = MessageFormatter.Format("test {argument}", new IMessageArg[] {arg});

            Assert.Equal("test {argument}", result);
            Assert.Equal(0, argCount);
        }

        [Fact]
        public void Should_Stringify_When_ManyVariables()
        {
            var argCount = 0;

            var arg = new TestArg(parameters =>
            {
                Assert.Null(parameters);
                argCount++;
            }) {Name = "arg", Value = "value"};

            var arg2Count = 0;
            var arg2 = new TestArg(parameters => { arg2Count++; }) {Name = "arg2", Value = "value2"};

            var result = MessageFormatter.Format("test {arg} {arg2}", new IMessageArg[] {arg, arg2});

            Assert.Equal("test value value2", result);
            Assert.Equal(1, argCount);
            Assert.Equal(1, arg2Count);
        }

        [Fact]
        public void Should_Stringify_When_OnlyUsedArguments()
        {
            var argCount = 0;

            var arg = new TestArg(parameters =>
            {
                Assert.Null(parameters);
                argCount++;
            }) {Name = "arg", Value = "value"};

            var arg2Count = 0;
            var arg2 = new TestArg(parameters => { arg2Count++; }) {Name = "arg2", Value = "value2"};

            var result = MessageFormatter.Format("test {arg} {arg2} {arg3}", new IMessageArg[] {arg, arg2});

            Assert.Equal("test value value2 {arg3}", result);
            Assert.Equal(1, argCount);
            Assert.Equal(1, arg2Count);
        }

        [Fact]
        public void Should_Stringify_When_SingleVariable()
        {
            var argCount = 0;

            var arg = new TestArg(parameters =>
            {
                Assert.Null(parameters);
                argCount++;
            }) {Name = "arg", Value = "value"};

            var result = MessageFormatter.Format("test {arg}", new IMessageArg[] {arg});

            Assert.Equal("test value", result);
            Assert.Equal(1, argCount);
        }

        [Fact]
        public void Should_Stringify_When_SingleVariable_ManyOccurances()
        {
            var argCount = 0;

            var arg = new TestArg(parameters =>
            {
                Assert.Null(parameters);
                argCount++;
            }) {Name = "arg", Value = "value"};

            var result = MessageFormatter.Format("test {arg} {arg}", new IMessageArg[] {arg});

            Assert.Equal("test value value", result);
            Assert.Equal(1, argCount);
        }

        [Fact]
        public void Should_ThrowException_When_DuplicateName()
        {
            Assert.Throws<ArgumentException>(() => { MessageFormatter.Format("test {arg}", new IMessageArg[] {new MessageArg("arg", "value1"), new MessageArg("arg", "value2")}); });
        }

        [Fact]
        public void Should_ThrowException_When_NullMessage()
        {
            Assert.Throws<ArgumentNullException>(() => { MessageFormatter.Format(null, new IMessageArg[] { }); });
        }
    }
}