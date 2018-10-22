using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Exceptions;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;
using Xunit;

namespace CoreValidation.UnitTests.Specifications.Commands
{
    public class AsNullableRuleTests
    {
        public enum RuleType
        {
            Validate = 0,

            AsRelative = 1
        }

        public static IEnumerable<object[]> OptionsData(ValidationStrategy[] strategies, RuleType[] types)
        {
            foreach (var strategy in strategies)
            {
                foreach (var type in types)
                {
                    yield return new object[] {strategy, type};
                }
            }
        }

        public static MemberSpecification<object, int> GetSingleRuleMemberSpecification(RuleType ruleType, bool isValid, string message = null, IReadOnlyCollection<IMessageArg> args = null)
        {
            if (ruleType == RuleType.Validate)
            {
                return be => be.Valid(m => isValid, message, args);
            }

            if (message != null)
            {
                // ReSharper disable once ImplicitlyCapturedClosure
                return be => be.AsRelative(m => isValid).WithMessage(message);
            }

            // ReSharper disable once ImplicitlyCapturedClosure
            return be => be.AsRelative(m => isValid);
        }

        public class AddingErrors
        {
            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast, ValidationStrategy.Force}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_AddError_When_Invalid(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var args = new[] {new MessageArg("key", "value")};
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, false, "message", args);

                AsNullableRule rule = new AsNullableRule<object, int>(memberSpecification);

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("message", errorsCollection.Errors.Single().Message);

                if (ruleType == RuleType.Validate)
                {
                    Assert.Same(args, errorsCollection.Errors.Single().Arguments);
                }
                else
                {
                    Assert.Null(errorsCollection.Errors.Single().Arguments);
                }
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast, ValidationStrategy.Force}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_AddError_When_Invalid_And_NullArgs(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, false, "message");

                AsNullableRule rule = new AsNullableRule<object, int>(memberSpecification);

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("message", errorsCollection.Errors.Single().Message);
                Assert.Null(errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_NotAddError_When_Valid(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, true, "message");

                AsNullableRule rule = new AsNullableRule<object, int>(memberSpecification);

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.False(getErrorsResult);
                Assert.True(errorsCollection.IsEmpty);
                Assert.Empty(errorsCollection.Errors);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_NotAddError_When_NullMember(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, true, "message");

                AsNullableRule rule = new AsNullableRule<object, int>(memberSpecification);

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    null,
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.False(getErrorsResult);
                Assert.True(errorsCollection.IsEmpty);
                Assert.Empty(errorsCollection.Errors);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Force}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_AddError_When_NullMember_And_Force(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var args = new[] {new MessageArg("key", "value")};
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, false, "message", args);

                AsNullableRule rule = new AsNullableRule<object, int>(memberSpecification);

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    null,
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("message", errorsCollection.Errors.Single().Message);

                if (ruleType == RuleType.Validate)
                {
                    Assert.Same(args, errorsCollection.Errors.Single().Arguments);
                }
                else
                {
                    Assert.Null(errorsCollection.Errors.Single().Arguments);
                }
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Force}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_AddError_When_Valid_And_Force(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var args = new[] {new MessageArg("key", "value")};
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, true, "message", args);

                AsNullableRule rule = new AsNullableRule<object, int>(memberSpecification);

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("message", errorsCollection.Errors.Single().Message);

                if (ruleType == RuleType.Validate)
                {
                    Assert.Same(args, errorsCollection.Errors.Single().Arguments);
                }
                else
                {
                    Assert.Null(errorsCollection.Errors.Single().Arguments);
                }
            }

            [Fact]
            public void Should_AddManyErrors()
            {
                AsNullableRule rule = new AsNullableRule<object, int>(m => m
                    .Valid(v => false, "error1")
                    .Valid(v => false, "error2")
                    .Valid(v => false, "error3")
                );

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    ValidationStrategy.Complete,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

                Assert.Equal(3, errorsCollection.Errors.Count);
                Assert.Equal("error1", errorsCollection.Errors.ElementAt(0).Message);
                Assert.Equal("error2", errorsCollection.Errors.ElementAt(1).Message);
                Assert.Equal("error3", errorsCollection.Errors.ElementAt(2).Message);
            }
        }

        public class AddingErrorsInChains
        {
            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast, ValidationStrategy.Force}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_AddDefaultError_When_Invalid_And_NoError(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var args = new[] {new MessageArg("key", "value")};
                var message = "default error {arg}";
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, false);

                var rule = new AsNullableRule<object, int>(memberSpecification);

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub
                    {
                        DefaultError = new Error(message, args)
                    },
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

                Assert.Equal(message, errorsCollection.Errors.Single().Message);
                Assert.Same(args, errorsCollection.Errors.Single().Arguments);
            }

            [Fact]
            public void Should_AddErrors_When_Chain_And_Complete()
            {
                var args3 = new[] {new MessageArg("key3", "value3")};
                var args7 = new[] {new MessageArg("key7", "value7")};

                var rule = new AsNullableRule<object, int>(be => be
                    .Valid(m => true, "message1", new[] {new MessageArg("key1", "value1")})
                    .AsRelative(m => true).WithMessage("message2")
                    .Valid(m => false, "message3", args3)
                    .AsRelative(m => false).WithMessage("message4")
                    .Valid(m => true, "message5", new[] {new MessageArg("key5", "value5")})
                    .AsRelative(m => false).WithMessage("message6")
                    .Valid(m => false, "message7", args7)
                    .AsRelative(m => true).WithMessage("message8")
                );

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    ValidationStrategy.Complete,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

                Assert.Equal(4, errorsCollection.Errors.Count);

                Assert.Equal("message3", errorsCollection.Errors.ElementAt(0).Message);
                Assert.Same(args3, errorsCollection.Errors.ElementAt(0).Arguments);

                Assert.Equal("message4", errorsCollection.Errors.ElementAt(1).Message);
                Assert.Null(errorsCollection.Errors.ElementAt(1).Arguments);

                Assert.Equal("message6", errorsCollection.Errors.ElementAt(2).Message);
                Assert.Null(errorsCollection.Errors.ElementAt(2).Arguments);

                Assert.Equal("message7", errorsCollection.Errors.ElementAt(3).Message);
                Assert.Same(args7, errorsCollection.Errors.ElementAt(3).Arguments);
            }

            [Fact]
            public void Should_AddErrors_When_Chain_And_FailFast()
            {
                var args3 = new[] {new MessageArg("key3", "value3")};
                var args7 = new[] {new MessageArg("key7", "value7")};

                var rule = new AsNullableRule<object, int>(be => be
                    .Valid(m => true, "message1", new[] {new MessageArg("key1", "value1")})
                    .AsRelative(m => true).WithMessage("message2")
                    .Valid(m => false, "message3", args3)
                    .AsRelative(m => false).WithMessage("message4")
                    .Valid(m => true, "message5", new[] {new MessageArg("key5", "value5")})
                    .AsRelative(m => false).WithMessage("message6")
                    .Valid(m => false, "message7", args7)
                    .AsRelative(m => true).WithMessage("message8")
                );

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    ValidationStrategy.FailFast,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

                Assert.Equal(1, errorsCollection.Errors.Count);

                Assert.Equal("message3", errorsCollection.Errors.ElementAt(0).Message);
                Assert.Same(args3, errorsCollection.Errors.ElementAt(0).Arguments);
            }

            [Fact]
            public void Should_AddErrors_When_Chain_And_Force()
            {
                var args1 = new[] {new MessageArg("key1", "value1")};
                var args3 = new[] {new MessageArg("key3", "value3")};
                var args5 = new[] {new MessageArg("key5", "value5")};
                var args7 = new[] {new MessageArg("key7", "value7")};

                var rule = new AsNullableRule<object, int>(be => be
                    .Valid(m => true, "message1", args1)
                    .AsRelative(m => true).WithMessage("message2")
                    .Valid(m => false, "message3", args3)
                    .AsRelative(m => false).WithMessage("message4")
                    .Valid(m => true, "message5", args5)
                    .AsRelative(m => false).WithMessage("message6")
                    .Valid(m => false, "message7", args7)
                    .AsRelative(m => true).WithMessage("message8")
                );

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    ValidationStrategy.Force,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

                Assert.Equal(8, errorsCollection.Errors.Count);

                Assert.Equal("message1", errorsCollection.Errors.ElementAt(0).Message);
                Assert.Same(args1, errorsCollection.Errors.ElementAt(0).Arguments);

                Assert.Equal("message2", errorsCollection.Errors.ElementAt(1).Message);
                Assert.Null(errorsCollection.Errors.ElementAt(1).Arguments);

                Assert.Equal("message3", errorsCollection.Errors.ElementAt(2).Message);
                Assert.Same(args3, errorsCollection.Errors.ElementAt(2).Arguments);

                Assert.Equal("message4", errorsCollection.Errors.ElementAt(3).Message);
                Assert.Null(errorsCollection.Errors.ElementAt(3).Arguments);

                Assert.Equal("message5", errorsCollection.Errors.ElementAt(4).Message);
                Assert.Same(args5, errorsCollection.Errors.ElementAt(4).Arguments);

                Assert.Equal("message6", errorsCollection.Errors.ElementAt(5).Message);
                Assert.Null(errorsCollection.Errors.ElementAt(5).Arguments);

                Assert.Equal("message7", errorsCollection.Errors.ElementAt(6).Message);
                Assert.Same(args7, errorsCollection.Errors.ElementAt(6).Arguments);

                Assert.Equal("message8", errorsCollection.Errors.ElementAt(7).Message);
                Assert.Null(errorsCollection.Errors.ElementAt(7).Arguments);
            }
        }

        public class OverridingErrorMessage
        {
            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast, ValidationStrategy.Force}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_OverrideErrorMessage_When_Invalid(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, false, "message", new[] {new MessageArg("key", "value")});

                var rule = new AsNullableRule<object, int>(c => memberSpecification(c).SetSingleError("message_overriden"));

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

                Assert.Equal("message_overriden", errorsCollection.Errors.Single().Message);
                Assert.Null(errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_NotOverrideErrorMessage_When_Valid(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, true, "message", new[] {new MessageArg("key", "value")});

                var rule = new AsNullableRule<object, int>(c => memberSpecification(c).SetSingleError("message_overriden"));

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.False(getErrorsResult);

                Assert.True(errorsCollection.IsEmpty);
                Assert.Empty(errorsCollection.Errors);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Force}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_OverrideErrorMessage_When_Valid_And_Force(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, true, "message", new[] {new MessageArg("key", "value")});

                var rule = new AsNullableRule<object, int>(c => memberSpecification(c).SetSingleError("message_overriden"));

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

                Assert.Equal("message_overriden", errorsCollection.Errors.Single().Message);
                Assert.Null(errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_OverrideErrorMessage_When_Chain(ValidationStrategy validationStrategy)
            {
                var rule = new AsNullableRule<object, int>(be => be
                    .Valid(m => true, "message1", new[] {new MessageArg("key1", "value1")})
                    .AsRelative(m => true).WithMessage("message2")
                    .Valid(m => false, "message3", new[] {new MessageArg("key3", "value3")})
                    .AsRelative(m => false).WithMessage("message4")
                    .Valid(m => true, "message5", new[] {new MessageArg("key5", "value5")})
                    .AsRelative(m => false).WithMessage("message6")
                    .Valid(m => false, "message7", new[] {new MessageArg("key7", "value7")})
                    .AsRelative(m => true).WithMessage("message8")
                    .SetSingleError("message_overriden")
                );

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);

                Assert.Equal("message_overriden", errorsCollection.Errors.Single().Message);
                Assert.Null(errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_FailFast_When_OverrideErrorMessage(ValidationStrategy validationStrategy)
            {
                var executionCounter = 0;

                var rule = new AsNullableRule<object, int>(be => be
                    .Valid(m =>
                    {
                        executionCounter++;

                        return true;
                    }, "message1", new[] {new MessageArg("key1", "value1")})
                    .AsRelative(m =>
                    {
                        executionCounter++;

                        return true;
                    })
                    .Valid(m =>
                    {
                        executionCounter++;

                        return false;
                    }, "message3", new[] {new MessageArg("key3", "value3")})
                    .AsRelative(m =>
                    {
                        executionCounter++;

                        return false;
                    })
                    .Valid(m =>
                    {
                        executionCounter++;

                        return true;
                    }, "message5", new[] {new MessageArg("key5", "value5")})
                    .AsRelative(m =>
                    {
                        executionCounter++;

                        return false;
                    })
                    .Valid(m =>
                    {
                        executionCounter++;

                        return false;
                    }, "message7", new[] {new MessageArg("key7", "value7")})
                    .AsRelative(m =>
                    {
                        executionCounter++;

                        return true;
                    })
                    .SetSingleError("message_overriden")
                );

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out _);

                Assert.True(getErrorsResult);

                Assert.Equal(3, executionCounter);
            }
        }

        public class PasingValuesToPredicates
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_PassMemberToPredicate_When_Validate(ValidationStrategy validationStrategy)
            {
                var executed = false;
                int? member = 1230;

                MemberSpecification<object, int> memberSpecification = be => be.Valid(m =>
                {
                    Assert.Equal(member.Value, m);

                    executed = true;

                    return true;
                });

                var rule = new AsNullableRule<object, int>(memberSpecification);

                rule.TryGetErrors(
                    new object(),
                    member,
                    new ExecutionContextStub(),
                    validationStrategy,
                    out _);

                Assert.True(executed);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_PassModelToPredicate_When_AsRelative(ValidationStrategy validationStrategy)
            {
                var executed = false;
                var model = new object();

                MemberSpecification<object, int> memberSpecification = be => be.AsRelative(m =>
                {
                    Assert.Same(model, m);

                    executed = true;

                    return true;
                });

                var rule = new AsNullableRule<object, int>(memberSpecification);

                rule.TryGetErrors(
                    model,
                    1230,
                    new ExecutionContextStub(),
                    validationStrategy,
                    out _);

                Assert.True(executed);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_PassModelAndMemberToPredicate_When_Validate_And_AsRelative_InChain(ValidationStrategy validationStrategy)
            {
                var validateExecuted = false;
                var validateRelationExecuted = false;
                int? member = 1230;
                var model = new object();

                MemberSpecification<object, int> memberSpecification = be => be.Valid(m =>
                    {
                        Assert.Equal(member.Value, m);

                        validateExecuted = true;

                        return true;
                    }, "message")
                    .AsRelative(m =>
                    {
                        Assert.Same(model, m);

                        validateRelationExecuted = true;

                        return true;
                    });

                var rule = new AsNullableRule<object, int>(memberSpecification);

                rule.TryGetErrors(
                    model,
                    member,
                    new ExecutionContextStub(),
                    validationStrategy,
                    out _);

                Assert.True(validateExecuted);
                Assert.True(validateRelationExecuted);
            }
        }

        public class ExecutingCollect
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_ExecuteCollect_When_Validate(ValidationStrategy validationStrategy)
            {
                var executed = false;

                MemberSpecification<object, int> memberSpecification = be => be.Valid(m =>
                {
                    executed = true;

                    return true;
                }, "message");

                var rule = new AsNullableRule<object, int>(memberSpecification);

                rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out _);

                Assert.True(executed);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_ExecuteCollect_When_AsRelative(ValidationStrategy validationStrategy)
            {
                var executed = false;

                MemberSpecification<object, int> memberSpecification = be => be.AsRelative(m =>
                {
                    executed = true;

                    return true;
                });

                var rule = new AsNullableRule<object, int>(memberSpecification);

                rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out _);

                Assert.True(executed);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_NotExecuteCollect_When_Validate_And_NullMember(ValidationStrategy validationStrategy)
            {
                var executed = false;

                MemberSpecification<object, int> memberSpecification = be => be.Valid(m =>
                {
                    executed = true;

                    return true;
                }, "message");

                var rule = new AsNullableRule<object, int>(memberSpecification);

                rule.TryGetErrors(
                    new object(),
                    null,
                    new ExecutionContextStub(),
                    validationStrategy,
                    out _);

                Assert.False(executed);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_NotExecuteCollect_When_AsRelative_And_NullMember(ValidationStrategy validationStrategy)
            {
                var executed = false;

                MemberSpecification<object, int> memberSpecification = be => be.AsRelative(m =>
                {
                    executed = true;

                    return true;
                });

                var rule = new AsNullableRule<object, int>(memberSpecification);

                rule.TryGetErrors(
                    new object(),
                    null,
                    new ExecutionContextStub(),
                    validationStrategy,
                    out _);

                Assert.False(executed);
            }

            [Fact]
            public void Should_NotExecuteCollect_When_AsRelative_And_Force()
            {
                var executed = false;

                MemberSpecification<object, int> memberSpecification = be => be.AsRelative(m =>
                {
                    executed = true;

                    return true;
                });

                var rule = new AsNullableRule<object, int>(memberSpecification);

                rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    ValidationStrategy.Force,
                    out _);

                Assert.False(executed);
            }

            [Fact]
            public void Should_NotExecuteCollect_When_Validate_And_Force()
            {
                var executed = false;

                MemberSpecification<object, int> memberSpecification = be => be.Valid(m =>
                {
                    executed = true;

                    return true;
                }, "message");

                var rule = new AsNullableRule<object, int>(memberSpecification);

                rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    ValidationStrategy.Force,
                    out _);

                Assert.False(executed);
            }
        }

        public class RuleSingleError
        {
            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast, ValidationStrategy.Force}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_AddRuleSingleError_When_Invalid(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var args = new[] {new MessageArg("key", "value")};
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, false, "message", args);

                AsNullableRule rule = new AsNullableRule<object, int>(memberSpecification);

                rule.RuleSingleError = new Error("ruleSingleError");

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("ruleSingleError", errorsCollection.Errors.Single().Message);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_NotAddRuleSingleError_When_Valid(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, true, "message");

                AsNullableRule rule = new AsNullableRule<object, int>(memberSpecification);

                rule.RuleSingleError = new Error("ruleSingleError");

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.False(getErrorsResult);
                Assert.True(errorsCollection.IsEmpty);
                Assert.Empty(errorsCollection.Errors);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_NotAddRuleSingleError_When_NullMember(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, true, "message");

                AsNullableRule rule = new AsNullableRule<object, int>(memberSpecification);

                rule.RuleSingleError = new Error("ruleSingleError");

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    null,
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.False(getErrorsResult);
                Assert.True(errorsCollection.IsEmpty);
                Assert.Empty(errorsCollection.Errors);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Force}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_AddRuleSingleError_When_NullMember_And_Force(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var args = new[] {new MessageArg("key", "value")};
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, false, "message", args);

                AsNullableRule rule = new AsNullableRule<object, int>(memberSpecification);

                rule.RuleSingleError = new Error("ruleSingleError");

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    null,
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("ruleSingleError", errorsCollection.Errors.Single().Message);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Force}, new[] {RuleType.Validate, RuleType.AsRelative}, MemberType = typeof(AsNullableRuleTests))]
            public void Should_AddSingleError_When_Valid_And_Force(ValidationStrategy validationStrategy, RuleType ruleType)
            {
                var args = new[] {new MessageArg("key", "value")};
                var memberSpecification = GetSingleRuleMemberSpecification(ruleType, true, "message", args);

                AsNullableRule rule = new AsNullableRule<object, int>(memberSpecification);

                rule.RuleSingleError = new Error("ruleSingleError");

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    validationStrategy,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("ruleSingleError", errorsCollection.Errors.Single().Message);
            }

            [Fact]
            public void Should_AddSingleError_When_ManyErrors()
            {
                AsNullableRule rule = new AsNullableRule<object, int>(m => m
                    .Valid(v => false, "error1")
                    .Valid(v => false, "error2")
                    .Valid(v => false, "error3")
                );

                rule.RuleSingleError = new Error("ruleSingleError");

                var getErrorsResult = rule.TryGetErrors(
                    new object(),
                    default(int),
                    new ExecutionContextStub(),
                    ValidationStrategy.Complete,
                    out var errorsCollection);

                Assert.True(getErrorsResult);
                Assert.Equal("ruleSingleError", errorsCollection.Errors.Single().Message);
            }
        }


        [Fact]
        public void Should_ThrowException_When_ReturningNewInstanceOfSpecification()
        {
            Assert.Throws<InvalidProcessedReferenceException>(() =>
            {
                // ReSharper disable once ObjectCreationAsStatement
                new AsNullableRule<object, int>(be => new MemberSpecificationBuilder<object, int>());
            });
        }
    }
}