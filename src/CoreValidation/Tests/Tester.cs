using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;
using CoreValidation.Validators;

namespace CoreValidation.Tests
{
    public static class Tester
    {
        public static Exception TestMemberRuleException<TMember>(MemberSpecification<object, TMember> specification, Type expectedException)
        {
            try
            {
                MemberValidatorCreator.Create(specification);
            }
            catch (Exception exception)
            {
                if (!expectedException.IsInstanceOfType(exception))
                {
                    throw new TesterException($"Invalid exception (`{exception.GetType().FullName}`) thrown when `{expectedException.FullName}` was expected");
                }

                return exception;
            }

            throw new TesterException($"No exception thrown when `{expectedException.FullName}` was expected");
        }

        public static void TestSingleMemberRule<TMember>(MemberSpecification<object, TMember> specification, TMember member, bool expectedIsValid, string expectedErrorMessage = null, IReadOnlyCollection<IMessageArg> expectedArgs = null)
        {
            var validator = MemberValidatorCreator.Create(specification);

            var rule = validator.Rules.Single();

            var executionContext = new ExecutionContextStub();

            IErrorsCollection errorsCollection;

            if (rule is ValidRule<TMember> validateRule)
            {
                validateRule.TryGetErrors(member, executionContext, ValidationStrategy.Complete, out errorsCollection);
            }
            else if (rule is AsNullableRule validateNullableRule)
            {
                validateNullableRule.TryGetErrors(new object(), member, executionContext, ValidationStrategy.Complete, out errorsCollection);
            }
            else
            {
                throw new InvalidOperationException($"Only Valid and AsNullable can be tested in {nameof(TestSingleMemberRule)}");
            }

            if (expectedIsValid)
            {
                if (!errorsCollection.IsEmpty)
                {
                    throw new TesterException("Error collection isn't empty, but no error is expected");
                }
            }
            else
            {
                if (errorsCollection.IsEmpty)
                {
                    throw new TesterException("Error collection is empty, but error is expected");
                }

                if ((errorsCollection.Errors.Count != 1) || (errorsCollection.Members.Count > 0))
                {
                    throw new TesterException("Only one error is expected");
                }

                var error = errorsCollection.Errors.Single();

                if (expectedErrorMessage != null)
                {
                    if (!string.Equals(error.Message, expectedErrorMessage))
                    {
                        throw new TesterException($"Message `{error.Message}` is not as expected: `{expectedErrorMessage}`");
                    }
                }

                if (expectedArgs != null)
                {
                    if (error.Arguments == null)
                    {
                        throw new TesterException($"No arguments while expected amount is `{expectedArgs.Count}`");
                    }

                    if (error.Arguments.Count != expectedArgs.Count)
                    {
                        throw new TesterException($"Arguments count `{error.Arguments.Count}` is not as expected `{expectedArgs.Count}`");
                    }

                    for (var i = 0; i < expectedArgs.Count; ++i)
                    {
                        var expected = expectedArgs.ElementAt(i);

                        var actual = error.Arguments.ElementAt(i);

                        if (!string.Equals(expected.Name, actual.Name))
                        {
                            throw new TesterException($"Argument at position `{i}` - name `{actual.Name}` is not as expected `{expected.Name}`");
                        }

                        var actualType = actual.GetType();
                        var expectedType = expected.GetType();

                        if (!expectedType.IsAssignableFrom(actualType))
                        {
                            throw new TesterException($"Argument at position `{i}` - type `{actualType.FullName}` cannot be used as `{expectedType.FullName}`");
                        }

                        var actualValue = actualType.GetProperties().Single(p => p.Name == "Value").GetValue(actual);
                        var expectedValue = actualType.GetProperties().Single(p => p.Name == "Value").GetValue(expected);

                        if (actualValue is double d)
                        {
                            if (Math.Abs(d - (double)expectedValue) > 0.0000001d)
                            {
                                throw new TesterException($"Argument at position `{i}` - value (double) `{actualValue}` is not as expected `{expectedValue}`");
                            }
                        }
                        else if (!expectedValue.Equals(actualValue))
                        {
                            throw new TesterException($"Argument at position `{i}` - value `{actualValue}` is not as expected `{expectedValue}`");
                        }
                    }
                }
                else if (error.Arguments != null)
                {
                    throw new TesterException("Arguments are not expected in the error");
                }
            }
        }

        private class ExecutionContextStub : IExecutionContext
        {
            public string CollectionForceKey { get; } = "*";

            public IError RequiredError { get; } = new Error("Required");

            public IError DefaultError { get; } = new Error("Invalid");

            public int MaxDepth { get; } = 10;

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public IValidatorsFactory ValidatorsFactory { get; set; }

            public IErrorsCollection DefaultErrorAsCollection
            {
                get
                {
                    var defaultErrorCollection = new ErrorsCollection();
                    defaultErrorCollection.AddError(DefaultError);

                    return defaultErrorCollection;
                }
            }
        }
    }
}