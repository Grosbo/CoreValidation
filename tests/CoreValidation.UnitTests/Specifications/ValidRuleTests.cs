using System;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using Xunit;

namespace CoreValidation.UnitTests.Specifications
{
    public class ValidRuleTests
    {
        public class AddingErrors
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_AddError_When_Invalid(ValidationStrategy validationStrategy)
            {
                Predicate<object> isValid = m => false;

                var args = new[] {new MessageArg("key", "value")};

                var rule = new ValidRule<object>(isValid, "message", args) as IRule;

                var error = rule.Compile(new[]
                    {
                        new object(),
                        validationStrategy,
                        ErrorHelpers.DefaultErrorStub
                    })
                    .Errors
                    .Single();

                Assert.Equal("message", error.Message);
                Assert.Same(args, error.Arguments);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_AddError_When_Invalid_And_NullArgs(ValidationStrategy validationStrategy)
            {
                Predicate<object> isValid = m => false;

                var rule = new ValidRule<object>(isValid, "message") as IRule;

                var error = rule.Compile(new[]
                    {
                        new object(),
                        validationStrategy,
                        ErrorHelpers.DefaultErrorStub
                    })
                    .Errors
                    .Single();

                Assert.Equal("message", error.Message);
                Assert.Null(error.Arguments);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_NotAddError_When_Valid(ValidationStrategy validationStrategy)
            {
                Predicate<object> isValid = m => true;

                var rule = new ValidRule<object>(isValid, "message") as IRule;

                var error = rule.Compile(new[]
                    {
                        new object(),
                        validationStrategy,
                        ErrorHelpers.DefaultErrorStub
                    })
                    .Errors
                    .SingleOrDefault();

                Assert.Null(error);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_AddDefaultError_When_Invalid_And_NoMessage_And_NoArgs(ValidationStrategy validationStrategy)
            {
                Predicate<object> isValid = m => false;

                var args = new[] {new MessageArg("key", "value")};
                var message = "default error {arg}";

                var rule = new ValidRule<object>(isValid);

                var error = rule.Compile(new[]
                    {
                        new object(),
                        validationStrategy,
                        new Error(message, args)
                    })
                    .Errors
                    .Single();

                Assert.Equal(message, error.Message);
                Assert.Same(args, error.Arguments);
            }

            [Fact]
            public void Should_AddError_When_Valid_And_Force()
            {
                Predicate<object> isValid = m => true;

                var rule = new ValidRule<object>(isValid, "message") as IRule;

                var error = rule.Compile(new[]
                    {
                        new object(),
                        ValidationStrategy.Force,
                        ErrorHelpers.DefaultErrorStub
                    })
                    .Errors
                    .Single();

                Assert.Equal("message", error.Message);
                Assert.Null(error.Arguments);
            }
        }

        public class PassingValuesToPredicates
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_PassReferenceToPredicate(ValidationStrategy validationStrategy)
            {
                var executed = false;
                var member = new object();

                Predicate<object> isValid = m =>
                {
                    executed = true;
                    Assert.Same(member, m);

                    return true;
                };

                var rule = new ValidRule<object>(isValid, "message") as IRule;

                rule.Compile(new[]
                {
                    member,
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.True(executed);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_PassEqualValueToPredicate(ValidationStrategy validationStrategy)
            {
                var executed = false;
                var member = 1230.123;

                Predicate<double> isValid = m =>
                {
                    executed = true;
                    Assert.Equal(member, m);

                    return true;
                };

                var rule = new ValidRule<double>(isValid, "message") as IRule;

                rule.Compile(new object[]
                {
                    member,
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.True(executed);
            }
        }

        public class ExecutingPredicates
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_ExecutePredicate(ValidationStrategy validationStrategy)
            {
                var executed = 0;

                Predicate<object> isValid = m =>
                {
                    executed++;

                    return true;
                };

                var rule = new ValidRule<object>(isValid, "message") as IRule;

                rule.Compile(new[]
                {
                    new object(),
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.Equal(1, executed);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_NotExecutePredicate_When_NullMember(ValidationStrategy validationStrategy)
            {
                var executed = 0;

                Predicate<object> isValid = m =>
                {
                    executed++;

                    return true;
                };

                var rule = new ValidRule<object>(isValid, "message") as IRule;

                rule.Compile(new object[]
                {
                    null,
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.Equal(0, executed);
            }

            [Fact]
            public void Should_NotExecutePredicate_When_Force()
            {
                var executed = 0;

                Predicate<object> isValid = m =>
                {
                    executed++;

                    return true;
                };

                var rule = new ValidRule<object>(isValid, "message") as IRule;

                rule.Compile(new[]
                {
                    new object(),
                    ValidationStrategy.Force,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.Equal(0, executed);
            }
        }

        public class InvalidArguments
        {
            [Fact]
            public void Should_ThrowException_When_Constructing_And_NullMessage_And_Arguments()
            {
                // ReSharper disable once ObjectCreationAsStatement
                Assert.Throws<ArgumentException>(() => { new ValidRule<object>(c => true, null, new IMessageArg[] { }); });
            }

            [Fact]
            public void Should_ThrowException_When_Constructing_And_NullPredicate()
            {
                // ReSharper disable once ObjectCreationAsStatement
                Assert.Throws<ArgumentNullException>(() => { new ValidRule<object>(null, "message"); });
            }

            [Fact]
            public void Should_ThrowException_When_NullStrategy()
            {
                var rule = new ValidRule<object>(c => true, "message");

                Assert.Throws<NullReferenceException>(() =>
                {
                    rule.Compile(new[]
                    {
                        new object(),
                        null
                    });
                });
            }
        }
    }
}