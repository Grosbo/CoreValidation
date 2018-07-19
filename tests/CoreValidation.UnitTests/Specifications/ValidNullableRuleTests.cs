using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Validators;
using Xunit;

namespace CoreValidation.UnitTests.Specifications
{
    public class ValidNullableRuleTests
    {
        public enum ValidateType
        {
            Validate,

            ValidateRelation
        }

        public static IEnumerable<object[]> OptionsData(ValidationStrategy[] strategies, ValidateType[] types)
        {
            foreach (var strategy in strategies)
            {
                foreach (var type in types)
                {
                    yield return new object[] {strategy, type};
                }
            }
        }

        public static MemberValidator<object, int> GetSingleRuleMemberValidator(ValidateType validateType, bool isValid, string message = null, IReadOnlyCollection<IMessageArg> args = null)
        {
            if (validateType == ValidateType.Validate)
            {
                return be => be.Valid(m => isValid, message, args);
            }

            return be => be.ValidRelative(m => isValid, message, args);
        }

        public class AddingErrors
        {
            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast, ValidationStrategy.Force}, new[] {ValidateType.Validate, ValidateType.ValidateRelation}, MemberType = typeof(ValidNullableRuleTests))]
            public void Should_AddError_When_Invalid(ValidationStrategy validationStrategy, ValidateType validateType)
            {
                var args = new[] {new MessageArg("key", "value")};
                var memberValidator = GetSingleRuleMemberValidator(validateType, false, "message", args);

                var rule = new ValidNullableRule<object, int>(memberValidator);

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.Equal("message", errorsCollection.Errors.Single().Message);
                Assert.Same(args, errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast, ValidationStrategy.Force}, new[] {ValidateType.Validate, ValidateType.ValidateRelation}, MemberType = typeof(ValidNullableRuleTests))]
            public void Should_AddError_When_Invalid_And_NullArgs(ValidationStrategy validationStrategy, ValidateType validateType)
            {
                var memberValidator = GetSingleRuleMemberValidator(validateType, false, "message");

                var rule = new ValidNullableRule<object, int>(memberValidator);

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.Equal("message", errorsCollection.Errors.Single().Message);
                Assert.Null(errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast}, new[] {ValidateType.Validate, ValidateType.ValidateRelation}, MemberType = typeof(ValidNullableRuleTests))]
            public void Should_NotAddError_When_Valid(ValidationStrategy validationStrategy, ValidateType validateType)
            {
                var memberValidator = GetSingleRuleMemberValidator(validateType, true, "message");

                var rule = new ValidNullableRule<object, int>(memberValidator);

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.True(errorsCollection.IsEmpty);
                Assert.Empty(errorsCollection.Errors);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast}, new[] {ValidateType.Validate, ValidateType.ValidateRelation}, MemberType = typeof(ValidNullableRuleTests))]
            public void Should_NotAddError_When_NullMember(ValidationStrategy validationStrategy, ValidateType validateType)
            {
                var memberValidator = GetSingleRuleMemberValidator(validateType, false, "message");

                var rule = new ValidNullableRule<object, int>(memberValidator);

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    null,
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.True(errorsCollection.IsEmpty);
                Assert.Empty(errorsCollection.Errors);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Force}, new[] {ValidateType.Validate, ValidateType.ValidateRelation}, MemberType = typeof(ValidNullableRuleTests))]
            public void Should_AddError_When_NullMember_And_Force(ValidationStrategy validationStrategy, ValidateType validateType)
            {
                var memberValidator = GetSingleRuleMemberValidator(validateType, true, "message");

                var rule = new ValidNullableRule<object, int>(memberValidator);

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    null,
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.Equal("message", errorsCollection.Errors.Single().Message);
                Assert.Null(errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Force}, new[] {ValidateType.Validate, ValidateType.ValidateRelation}, MemberType = typeof(ValidNullableRuleTests))]
            public void Should_AddError_When_Valid_And_Force(ValidationStrategy validationStrategy, ValidateType validateType)
            {
                var memberValidator = GetSingleRuleMemberValidator(validateType, true, "message");

                var rule = new ValidNullableRule<object, int>(memberValidator);

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.Equal("message", errorsCollection.Errors.Single().Message);
                Assert.Null(errorsCollection.Errors.Single().Arguments);
            }
        }

        public class AddingErrorsInChains
        {
            [Fact]
            public void Should_AddErrors_When_Chain_And_Complete()
            {
                var args3 = new[] {new MessageArg("key3", "value3")};
                var args4 = new[] {new MessageArg("key4", "value4")};
                var args6 = new[] {new MessageArg("key6", "value6")};
                var args7 = new[] {new MessageArg("key7", "value7")};

                var rule = new ValidNullableRule<object, int>(be => be
                    .Valid(m => true, "message1", new[] {new MessageArg("key1", "value1")})
                    .ValidRelative(m => true, "message2", new[] {new MessageArg("key2", "value2")})
                    .Valid(m => false, "message3", args3)
                    .ValidRelative(m => false, "message4", args4)
                    .Valid(m => true, "message5", new[] {new MessageArg("key5", "value5")})
                    .ValidRelative(m => false, "message6", args6)
                    .Valid(m => false, "message7", args7)
                    .ValidRelative(m => true, "message8", new[] {new MessageArg("key8", "value8")})
                );

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    ValidationStrategy.Complete,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.Equal(4, errorsCollection.Errors.Count);

                Assert.Equal("message3", errorsCollection.Errors.ElementAt(0).Message);
                Assert.Same(args3, errorsCollection.Errors.ElementAt(0).Arguments);

                Assert.Equal("message4", errorsCollection.Errors.ElementAt(1).Message);
                Assert.Same(args4, errorsCollection.Errors.ElementAt(1).Arguments);

                Assert.Equal("message6", errorsCollection.Errors.ElementAt(2).Message);
                Assert.Same(args6, errorsCollection.Errors.ElementAt(2).Arguments);

                Assert.Equal("message7", errorsCollection.Errors.ElementAt(3).Message);
                Assert.Same(args7, errorsCollection.Errors.ElementAt(3).Arguments);
            }

            [Fact]
            public void Should_AddErrors_When_Chain_And_FailFast()
            {
                var args3 = new[] {new MessageArg("key3", "value3")};

                var rule = new ValidNullableRule<object, int>(be => be
                    .Valid(m => true, "message1", new[] {new MessageArg("key1", "value1")})
                    .ValidRelative(m => true, "message2", new[] {new MessageArg("key2", "value2")})
                    .Valid(m => false, "message3", args3)
                    .ValidRelative(m => false, "message4", new[] {new MessageArg("key4", "value4")})
                    .Valid(m => true, "message5", new[] {new MessageArg("key5", "value5")})
                    .ValidRelative(m => false, "message6", new[] {new MessageArg("key6", "value6")})
                    .Valid(m => false, "message7", new[] {new MessageArg("key7", "value7")})
                    .ValidRelative(m => true, "message8", new[] {new MessageArg("key8", "value8")})
                );

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    ValidationStrategy.FailFast,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.Equal(1, errorsCollection.Errors.Count);

                Assert.Equal("message3", errorsCollection.Errors.ElementAt(0).Message);
                Assert.Same(args3, errorsCollection.Errors.ElementAt(0).Arguments);
            }

            [Fact]
            public void Should_AddErrors_When_Chain_And_Force()
            {
                var args1 = new[] {new MessageArg("key1", "value1")};
                var args2 = new[] {new MessageArg("key2", "value2")};
                var args3 = new[] {new MessageArg("key3", "value3")};
                var args4 = new[] {new MessageArg("key4", "value4")};
                var args5 = new[] {new MessageArg("key5", "value5")};
                var args6 = new[] {new MessageArg("key6", "value6")};
                var args7 = new[] {new MessageArg("key7", "value7")};
                var args8 = new[] {new MessageArg("key7", "value7")};

                var rule = new ValidNullableRule<object, int>(be => be
                    .Valid(m => true, "message1", args1)
                    .ValidRelative(m => true, "message2", args2)
                    .Valid(m => false, "message3", args3)
                    .ValidRelative(m => false, "message4", args4)
                    .Valid(m => true, "message5", args5)
                    .ValidRelative(m => false, "message6", args6)
                    .Valid(m => false, "message7", args7)
                    .ValidRelative(m => true, "message8", args8)
                );

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    ValidationStrategy.Force,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.Equal(8, errorsCollection.Errors.Count);

                Assert.Equal("message1", errorsCollection.Errors.ElementAt(0).Message);
                Assert.Same(args1, errorsCollection.Errors.ElementAt(0).Arguments);

                Assert.Equal("message2", errorsCollection.Errors.ElementAt(1).Message);
                Assert.Same(args2, errorsCollection.Errors.ElementAt(1).Arguments);

                Assert.Equal("message3", errorsCollection.Errors.ElementAt(2).Message);
                Assert.Same(args3, errorsCollection.Errors.ElementAt(2).Arguments);

                Assert.Equal("message4", errorsCollection.Errors.ElementAt(3).Message);
                Assert.Same(args4, errorsCollection.Errors.ElementAt(3).Arguments);

                Assert.Equal("message5", errorsCollection.Errors.ElementAt(4).Message);
                Assert.Same(args5, errorsCollection.Errors.ElementAt(4).Arguments);

                Assert.Equal("message6", errorsCollection.Errors.ElementAt(5).Message);
                Assert.Same(args6, errorsCollection.Errors.ElementAt(5).Arguments);

                Assert.Equal("message7", errorsCollection.Errors.ElementAt(6).Message);
                Assert.Same(args7, errorsCollection.Errors.ElementAt(6).Arguments);

                Assert.Equal("message8", errorsCollection.Errors.ElementAt(7).Message);
                Assert.Same(args8, errorsCollection.Errors.ElementAt(7).Arguments);
            }
            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast, ValidationStrategy.Force}, new[] {ValidateType.Validate, ValidateType.ValidateRelation}, MemberType = typeof(ValidNullableRuleTests))]
            public void Should_AddDefaultError_When_Invalid_And_NoMessage_And_NoArgs(ValidationStrategy validationStrategy, ValidateType validateType)
            {
                var args = new[] {new MessageArg("key", "value")};
                var message = "default error {arg}";
                var memberValidator = GetSingleRuleMemberValidator(validateType, false);

                var rule = new ValidNullableRule<object, int>(memberValidator);

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    validationStrategy,
                    new Error(message, args)
                });

                Assert.Equal(message, errorsCollection.Errors.Single().Message);
                Assert.Same(args, errorsCollection.Errors.Single().Arguments);
            }
        }

        public class OverridingErrorMessage
        {
            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast, ValidationStrategy.Force}, new[] {ValidateType.Validate, ValidateType.ValidateRelation}, MemberType = typeof(ValidNullableRuleTests))]
            public void Should_OverrideErrorMessage_When_Invalid(ValidationStrategy validationStrategy, ValidateType validateType)
            {
                var memberValidator = GetSingleRuleMemberValidator(validateType, false, "message", new[] {new MessageArg("key", "value")});

                var args = new[] {new MessageArg("key_overriden", "value_overriden")};

                var rule = new ValidNullableRule<object, int>(c => memberValidator(c).WithSummaryError("message_overriden", args));

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.Equal("message_overriden", errorsCollection.Errors.Single().Message);
                Assert.Equal(args, errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Complete, ValidationStrategy.FailFast}, new[] {ValidateType.Validate, ValidateType.ValidateRelation}, MemberType = typeof(ValidNullableRuleTests))]
            public void Should_NotOverrideErrorMessage_When_Valid(ValidationStrategy validationStrategy, ValidateType validateType)
            {
                var memberValidator = GetSingleRuleMemberValidator(validateType, true, "message", new[] {new MessageArg("key", "value")});

                var rule = new ValidNullableRule<object, int>(c => memberValidator(c).WithSummaryError("message_overriden", new[] {new MessageArg("key_overriden", "value_overriden")}));

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.True(errorsCollection.IsEmpty);
                Assert.Empty(errorsCollection.Errors);
            }

            [Theory]
            [MemberData(nameof(OptionsData), new[] {ValidationStrategy.Force}, new[] {ValidateType.Validate, ValidateType.ValidateRelation}, MemberType = typeof(ValidNullableRuleTests))]
            public void Should_OverrideErrorMessage_When_Valid_And_Force(ValidationStrategy validationStrategy, ValidateType validateType)
            {
                var memberValidator = GetSingleRuleMemberValidator(validateType, true, "message", new[] {new MessageArg("key", "value")});

                var args = new[] {new MessageArg("key_overriden", "value_overriden")};
                var rule = new ValidNullableRule<object, int>(c => memberValidator(c).WithSummaryError("message_overriden", args));

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.Equal("message_overriden", errorsCollection.Errors.Single().Message);
                Assert.Same(args, errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_OverrideErrorMessage_When_Chain(ValidationStrategy validationStrategy)
            {
                var args = new[] {new MessageArg("key_overriden", "value_overriden")};

                var rule = new ValidNullableRule<object, int>(be => be
                    .Valid(m => true, "message1", new[] {new MessageArg("key1", "value1")})
                    .ValidRelative(m => true, "message2", new[] {new MessageArg("key2", "value2")})
                    .Valid(m => false, "message3", new[] {new MessageArg("key3", "value3")})
                    .ValidRelative(m => false, "message4", new[] {new MessageArg("key4", "value4")})
                    .Valid(m => true, "message5", new[] {new MessageArg("key5", "value5")})
                    .ValidRelative(m => false, "message6", new[] {new MessageArg("key6", "value6")})
                    .Valid(m => false, "message7", new[] {new MessageArg("key7", "value7")})
                    .ValidRelative(m => true, "message8", new[] {new MessageArg("key8", "value8")})
                    .WithSummaryError("message_overriden", args)
                );

                var errorsCollection = rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.Equal(1, errorsCollection.Errors.Count);

                Assert.Equal("message_overriden", errorsCollection.Errors.Single().Message);
                Assert.Same(args, errorsCollection.Errors.Single().Arguments);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_FailFast_When_OverrideErrorMessage(ValidationStrategy validationStrategy)
            {
                var executionCounter = 0;

                var rule = new ValidNullableRule<object, int>(be => be
                    .Valid(m =>
                    {
                        executionCounter++;

                        return true;
                    }, "message1", new[] {new MessageArg("key1", "value1")})
                    .ValidRelative(m =>
                    {
                        executionCounter++;

                        return true;
                    }, "message2", new[] {new MessageArg("key2", "value2")})
                    .Valid(m =>
                    {
                        executionCounter++;

                        return false;
                    }, "message3", new[] {new MessageArg("key3", "value3")})
                    .ValidRelative(m =>
                    {
                        executionCounter++;

                        return false;
                    }, "message4", new[] {new MessageArg("key4", "value4")})
                    .Valid(m =>
                    {
                        executionCounter++;

                        return true;
                    }, "message5", new[] {new MessageArg("key5", "value5")})
                    .ValidRelative(m =>
                    {
                        executionCounter++;

                        return false;
                    }, "message6", new[] {new MessageArg("key6", "value6")})
                    .Valid(m =>
                    {
                        executionCounter++;

                        return false;
                    }, "message7", new[] {new MessageArg("key7", "value7")})
                    .ValidRelative(m =>
                    {
                        executionCounter++;

                        return true;
                    }, "message8", new[] {new MessageArg("key8", "value8")})
                    .WithSummaryError("message_overriden", new[] {new MessageArg("key_overriden", "value_overriden")})
                );

                rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

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

                MemberValidator<object, int> memberValidator = be => be.Valid(m =>
                {
                    Assert.Equal(member.Value, m);

                    executed = true;

                    return true;
                }, "message");

                var rule = new ValidNullableRule<object, int>(memberValidator);

                rule.Compile(new[]
                {
                    new object(),
                    member,
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.True(executed);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_PassModelToPredicate_When_ValidateRelation(ValidationStrategy validationStrategy)
            {
                var executed = false;
                var model = new object();

                MemberValidator<object, int> memberValidator = be => be.ValidRelative(m =>
                {
                    Assert.Same(model, m);

                    executed = true;

                    return true;
                }, "message");

                var rule = new ValidNullableRule<object, int>(memberValidator);

                rule.Compile(new[]
                {
                    model,
                    123,
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.True(executed);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_PassModelAndMemberToPredicate_When_Validate_And_ValidateRelation_InChain(ValidationStrategy validationStrategy)
            {
                var validateExecuted = false;
                var validateRelationExecuted = false;
                int? member = 1230;
                var model = new object();

                MemberValidator<object, int> memberValidator = be => be.Valid(m =>
                    {
                        Assert.Equal(member.Value, m);

                        validateExecuted = true;

                        return true;
                    }, "message")
                    .ValidRelative(m =>
                    {
                        Assert.Same(model, m);

                        validateRelationExecuted = true;

                        return true;
                    }, "message");

                var rule = new ValidNullableRule<object, int>(memberValidator);

                rule.Compile(new[]
                {
                    model,
                    member,
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

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

                MemberValidator<object, int> memberValidator = be => be.Valid(m =>
                {
                    executed = true;

                    return true;
                }, "message");

                var rule = new ValidNullableRule<object, int>(memberValidator);

                rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.True(executed);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_ExecuteCollect_When_ValidateRelation(ValidationStrategy validationStrategy)
            {
                var executed = false;

                MemberValidator<object, int> memberValidator = be => be.ValidRelative(m =>
                {
                    executed = true;

                    return true;
                }, "message");

                var rule = new ValidNullableRule<object, int>(memberValidator);

                rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.True(executed);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_NotExecuteCollect_When_Validate_And_NullMember(ValidationStrategy validationStrategy)
            {
                var executed = false;

                MemberValidator<object, int> memberValidator = be => be.Valid(m =>
                {
                    executed = true;

                    return true;
                }, "message");

                var rule = new ValidNullableRule<object, int>(memberValidator);

                rule.Compile(new[]
                {
                    new object(),
                    null,
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.False(executed);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_NotExecuteCollect_When_ValidateRelation_And_NullMember(ValidationStrategy validationStrategy)
            {
                var executed = false;

                MemberValidator<object, int> memberValidator = be => be.ValidRelative(m =>
                {
                    executed = true;

                    return true;
                }, "message");

                var rule = new ValidNullableRule<object, int>(memberValidator);

                rule.Compile(new[]
                {
                    new object(),
                    null,
                    validationStrategy,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.False(executed);
            }

            [Fact]
            public void Should_NotExecuteCollect_When_Validate_And_Force()
            {
                var executed = false;

                MemberValidator<object, int> memberValidator = be => be.Valid(m =>
                {
                    executed = true;

                    return true;
                }, "message");

                var rule = new ValidNullableRule<object, int>(memberValidator);

                rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    ValidationStrategy.Force,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.False(executed);
            }

            [Fact]
            public void Should_NotExecuteCollect_When_ValidateRelation_And_Force()
            {
                var executed = false;

                MemberValidator<object, int> memberValidator = be => be.ValidRelative(m =>
                {
                    executed = true;

                    return true;
                }, "message");

                var rule = new ValidNullableRule<object, int>(memberValidator);

                rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    ValidationStrategy.Force,
                    ErrorHelpers.DefaultErrorStub
                });

                Assert.False(executed);
            }
        }

        [Fact]
        public void Should_ThrowException_When_WithName()
        {
            var rule = new ValidNullableRule<object, int>(be => be
                .WithName("anything")
            );

            Assert.Throws<InvalidOperationException>(() =>
            {
                rule.Compile(new[]
                {
                    new object(),
                    default(int),
                    ValidationStrategy.Complete,
                    ErrorHelpers.DefaultErrorStub
                });
            });
        }
    }
}