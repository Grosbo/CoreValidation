using System;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Specifications.Commands;
using Xunit;

namespace CoreValidation.UnitTests.Specifications.Commands
{
    public class AsRelativeRuleTests
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

                var error = new Error("message");

                var rule = new AsRelativeRule<object>(isValid, error);

                var getErrorsResult = rule.TryGetErrors(new object(),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Same(error, errorsCollection.Errors.Single());
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_NotAddError_When_Valid(ValidationStrategy validationStrategy)
            {
                Predicate<object> isValid = m => true;

                var error = new Error("message");

                var rule = new AsRelativeRule<object>(isValid, error);

                rule.TryGetErrors(new object(),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.Same(ErrorsCollection.Empty, errorsCollection);
                Assert.True(errorsCollection.IsEmpty);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_AddDefaultError_When_Invalid_And_NoError(ValidationStrategy validationStrategy)
            {
                Predicate<object> isValid = m => false;

                var error = new Error("default error {arg}", new[] {Arg.Text("key", "value")});

                var rule = new AsRelativeRule<object>(isValid);

                var getErrorsResult = rule.TryGetErrors(new object(),
                    new ExecutionContextStub
                    {
                        DefaultError = error
                    },
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

                Assert.Same(error, errorsCollection.Errors.Single());
            }

            [Fact]
            public void Should_AddError_When_Valid_And_Force()
            {
                Predicate<object> isValid = m => true;

                var error = new Error("message");

                var rule = new AsRelativeRule<object>(isValid, error);

                var getErrorsResult = rule.TryGetErrors(new object(),
                    new ExecutionContextStub(),
                    ValidationStrategy.Force,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Same(error, errorsCollection.Errors.Single());
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

                var rule = new AsRelativeRule<object>(isValid);

                rule.TryGetErrors(member,
                    new ExecutionContextStub(),
                    validationStrategy,
                    out _);

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

                var rule = new AsRelativeRule<object>(isValid);

                rule.TryGetErrors(new object(),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out _);

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

                var rule = new AsRelativeRule<object>(isValid);

                rule.TryGetErrors(null,
                    new ExecutionContextStub(),
                    validationStrategy,
                    out _);

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

                var rule = new AsRelativeRule<object>(isValid);

                rule.TryGetErrors(new object(),
                    new ExecutionContextStub(),
                    ValidationStrategy.Force,
                    out _);

                Assert.Equal(0, executed);
            }
        }

        public class RuleSingleError
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_AddError_When_Invalid(ValidationStrategy validationStrategy)
            {
                Predicate<object> isValid = m => false;

                var error = new Error("message");

                var rule = new AsRelativeRule<object>(isValid, error);

                rule.RuleSingleError = new Error("ruleSingleError");

                var getErrorsResult = rule.TryGetErrors(new object(),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Same("ruleSingleError", errorsCollection.Errors.Single().Message);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_NotAddError_When_Valid(ValidationStrategy validationStrategy)
            {
                Predicate<object> isValid = m => true;

                var error = new Error("message");

                var rule = new AsRelativeRule<object>(isValid, error);

                rule.RuleSingleError = new Error("ruleSingleError");

                rule.TryGetErrors(new object(),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.Same(ErrorsCollection.Empty, errorsCollection);
                Assert.True(errorsCollection.IsEmpty);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_AddDefaultError_When_Invalid_And_NoError(ValidationStrategy validationStrategy)
            {
                Predicate<object> isValid = m => false;

                var error = new Error("default error {arg}", new[] {Arg.Text("key", "value")});

                var rule = new AsRelativeRule<object>(isValid);

                rule.RuleSingleError = new Error("ruleSingleError");

                var getErrorsResult = rule.TryGetErrors(new object(),
                    new ExecutionContextStub
                    {
                        DefaultError = error
                    },
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

                Assert.Same("ruleSingleError", errorsCollection.Errors.Single().Message);
            }

            [Fact]
            public void Should_AddError_When_Valid_And_Force()
            {
                Predicate<object> isValid = m => true;

                var error = new Error("message");

                var rule = new AsRelativeRule<object>(isValid, error);

                rule.RuleSingleError = new Error("ruleSingleError");

                var getErrorsResult = rule.TryGetErrors(new object(),
                    new ExecutionContextStub(),
                    ValidationStrategy.Force,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Same("ruleSingleError", errorsCollection.Errors.Single().Message);
            }
        }

        public class InvalidArguments
        {
            [Fact]
            public void Should_ThrowException_When_Constructing_And_NullPredicate()
            {
                // ReSharper disable once ObjectCreationAsStatement
                Assert.Throws<ArgumentNullException>(() => { new AsRelativeRule<object>(null, new Error("message")); });
            }

            [Fact]
            public void Should_ThrowException_When_NullContext()
            {
                var rule = new AsRelativeRule<object>(c => true, new Error("message"));

                Assert.Throws<ArgumentNullException>(() =>
                {
                    rule.TryGetErrors(new object(),
                        null,
                        ValidationStrategy.Complete,
                        out _);
                });
            }
        }
    }
}