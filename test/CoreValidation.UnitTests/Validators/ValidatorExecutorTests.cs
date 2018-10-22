using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Exceptions;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;
using CoreValidation.UnitTests.Errors;
using CoreValidation.Validators;
using Moq;
using Xunit;

namespace CoreValidation.UnitTests.Validators
{
    public class ValidatorExecutorTests
    {
        public class ModelScopeRules
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_GatherNoErrors_When_NoRules(ValidationStrategy validationStrategy)
            {
                var validator = ValidatorCreator.Create<User>(m => m);

                var user = new User {Email = "", Address = new Address(), FirstLogin = DateTime.UtcNow};

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub(),
                    validationStrategy,
                    0);

                Assert.True(errorsCollection.IsEmpty);
                Assert.Empty(errorsCollection.Errors);
                Assert.Empty(errorsCollection.Members);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_GatherNoErrors_When_AllValid(ValidationStrategy validationStrategy)
            {
                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Email, be => be.Valid(m => true, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)}))
                    .Valid(m => true).WithMessage("error2")
                    .Member(m => m.Address, be => be.Valid(m => true, "error3 {id}", new IMessageArg[] {NumberArg.Create("id", 3)}))
                    .Valid(m => true).WithMessage("error4")
                    .Member(m => m.FirstLogin, be => be.Valid(m => true, "error5 {id}", new IMessageArg[] {NumberArg.Create("id", 5)}))
                );

                var user = new User {Email = "", Address = new Address(), FirstLogin = DateTime.UtcNow};

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub(),
                    validationStrategy,
                    0);

                Assert.True(errorsCollection.IsEmpty);
                Assert.Empty(errorsCollection.Errors);
                Assert.Empty(errorsCollection.Members);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_ExecuteNone_And_AddRequiredError_When_NullMember(ValidationStrategy validationStrategy)
            {
                var executed = false;

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Email, be => be.Valid(m =>
                    {
                        executed = true;

                        return false;
                    }, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)}))
                );

                var user = new User();

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        RequiredError = new Error("required {arg}", new IMessageArg[] {new TextArg("arg", "error!")})
                    },
                    validationStrategy,
                    0);

                Assert.False(executed);

                Assert.Empty(errorsCollection.Errors);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Email"], new[] {"required error!"});
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_ExecuteNone_And_NoError_When_NullMember_And_SetOptional(ValidationStrategy validationStrategy)
            {
                var executed = false;

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Email, be => be.SetOptional().Valid(m =>
                    {
                        executed = true;

                        return false;
                    }, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)}))
                );

                var user = new User();

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        RequiredError = new Error("required {arg}", new IMessageArg[] {new TextArg("arg", "error!")})
                    },
                    validationStrategy,
                    0);

                Assert.False(executed);
                Assert.True(errorsCollection.IsEmpty);
                Assert.Empty(errorsCollection.Errors);
                Assert.Empty(errorsCollection.Members);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_ExecuteNone_And_AddCustomRequiredError_When_NullMember(ValidationStrategy validationStrategy)
            {
                var executed = false;

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Email, be => be.Valid(m =>
                        {
                            executed = true;

                            return false;
                        }, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)})
                        .SetRequired("custom required")
                    )
                );

                var user = new User();

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        RequiredError = new Error("required")
                    },
                    validationStrategy,
                    0);

                Assert.False(executed);

                Assert.Empty(errorsCollection.Errors);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Email"], new[] {"custom required"});
            }

            [Theory]
            [InlineData(false, false)]
            [InlineData(false, true)]
            [InlineData(true, false)]
            [InlineData(true, true)]
            public void Should_AddSingleError_When_ErrorCollected(bool memberValid, bool modelValid)
            {
                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Email, be => be.Valid(m => memberValid, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)}))
                    .Valid(m => modelValid).WithMessage("error2")
                    .SetSingleError("summary")
                );

                var user = new User {Email = "", Address = new Address(), FirstLogin = DateTime.UtcNow};

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub(),
                    ValidationStrategy.Complete,
                    0);

                if (memberValid && modelValid)
                {
                    Assert.Empty(errorsCollection.Errors);
                }
                else
                {
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection, new[] {"summary"});
                }

                Assert.Empty(errorsCollection.Members);
            }

            [Fact]
            public void Should_AddDefaultError_When_NoMessages()
            {
                var executed = false;

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Email, be => be.Valid(m =>
                    {
                        executed = true;

                        return false;
                    }))
                );

                var user = new User {Email = "", Address = new Address(), FirstLogin = DateTime.UtcNow};

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        DefaultError = new Error("default {arg}", new IMessageArg[] {new TextArg("arg", "error!")})
                    },
                    ValidationStrategy.Complete,
                    0);

                Assert.True(executed);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Email"], new[] {"default error!"});
            }

            [Fact]
            public void Should_ExecuteAllErrors_When_CompleteStrategy()
            {
                var executed = new bool[5];

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Email, be => be.Valid(m =>
                    {
                        executed[0] = true;

                        return true;
                    }, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)}))
                    .Valid(m =>
                    {
                        executed[1] = true;

                        return false;
                    }).WithMessage("error2")
                    .Member(m => m.Address, be => be.Valid(m =>
                    {
                        executed[2] = true;

                        return false;
                    }, "error3 {id}", new IMessageArg[] {NumberArg.Create("id", 3)}))
                    .Valid(m =>
                    {
                        executed[3] = true;

                        return true;
                    }).WithMessage("error4")
                    .Member(m => m.FirstLogin, be => be.Valid(m =>
                    {
                        executed[4] = true;

                        return false;
                    }, "error5 {id}", new IMessageArg[] {NumberArg.Create("id", 5)}))
                );

                var user = new User {Email = "", Address = new Address(), FirstLogin = DateTime.UtcNow};

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub(),
                    ValidationStrategy.Complete,
                    0);

                Assert.True(executed.All(e => e));

                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection, new[] {"error2"});
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address", "FirstLogin"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Address"], new[] {"error3 3"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["FirstLogin"], new[] {"error5 5"});
            }

            [Fact]
            public void Should_ExecuteNone_And_AddCustomRequiredError_With_Error_When_NullMember_And_Force()
            {
                var executed = false;

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Email, be => be.Valid(m =>
                        {
                            executed = true;

                            return false;
                        }, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)})
                        .SetRequired("custom required")
                    )
                );

                var user = new User();

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        RequiredError = new Error("required")
                    },
                    ValidationStrategy.Force,
                    0);

                Assert.False(executed);

                Assert.Empty(errorsCollection.Errors);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Email"], new[] {"custom required", "error1 1"});
            }

            [Fact]
            public void Should_ExecuteNone_And_AddError_When_NullMember_And_SetOptional_And_Force()
            {
                var executed = false;

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Email, be => be.SetOptional().Valid(m =>
                    {
                        executed = true;

                        return false;
                    }, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)}))
                );

                var user = new User();

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        RequiredError = new Error("required {arg}", new IMessageArg[] {new TextArg("arg", "error!")})
                    },
                    ValidationStrategy.Force,
                    0);

                Assert.False(executed);

                Assert.Empty(errorsCollection.Errors);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Email"], new[] {"error1 1"});
            }

            [Fact]
            public void Should_ExecuteNone_And_AddRequiredError_With_Error_When_NullMember_And_Force()
            {
                var executed = false;

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Email, be => be.Valid(m =>
                    {
                        executed = true;

                        return false;
                    }, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)}))
                );

                var user = new User();

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        RequiredError = new Error("required {arg}", new IMessageArg[] {new TextArg("arg", "error!")})
                    },
                    ValidationStrategy.Force,
                    0);

                Assert.False(executed);

                Assert.Empty(errorsCollection.Errors);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Email"], new[] {"required error!", "error1 1"});
            }

            [Fact]
            public void Should_ExecuteNone_And_GatherAllMessages_When_Force()
            {
                var executed = new bool[5];

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Email, be => be.Valid(m =>
                    {
                        executed[0] = true;

                        return true;
                    }, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)}))
                    .Valid(m =>
                    {
                        executed[1] = true;

                        return false;
                    }).WithMessage("error2")
                    .Member(m => m.Address, be => be.Valid(m =>
                    {
                        executed[2] = true;

                        return false;
                    }, "error3 {id}", new IMessageArg[] {NumberArg.Create("id", 3)}))
                    .Valid(m =>
                    {
                        executed[3] = true;

                        return true;
                    }).WithMessage("error4")
                    .Member(m => m.FirstLogin, be => be.Valid(m =>
                    {
                        executed[4] = true;

                        return false;
                    }, "error5 {id}", new IMessageArg[] {NumberArg.Create("id", 5)}))
                );

                var user = new User {Email = "", Address = new Address(), FirstLogin = DateTime.UtcNow};

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub(),
                    ValidationStrategy.Force,
                    0);

                Assert.DoesNotContain(executed, e => e);

                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection, new[] {"error2", "error4"});
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email", "Address", "FirstLogin"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Email"], new[] {"Required", "error1 1"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Address"], new[] {"Required", "error3 3"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["FirstLogin"], new[] {"Required", "error5 5"});
            }

            [Fact]
            public void Should_ExecuteUntilFirstError_When_Complete_And_SingleError()
            {
                var executed = new bool[5];

                var validator = ValidatorCreator.Create<User>(u => u
                    .SetSingleError("single error")
                    .Member(m => m.Email, be => be.Valid(m =>
                    {
                        executed[0] = true;

                        return true;
                    }, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)}))
                    .Valid(m =>
                    {
                        executed[1] = true;

                        return false;
                    }).WithMessage("error2")
                    .Member(m => m.Address, be => be.Valid(m =>
                    {
                        executed[2] = true;

                        return false;
                    }, "error3 {id}", new IMessageArg[] {NumberArg.Create("id", 3)}))
                    .Valid(m =>
                    {
                        executed[3] = true;

                        return true;
                    }).WithMessage("error4")
                    .Member(m => m.FirstLogin, be => be.Valid(m =>
                    {
                        executed[4] = true;

                        return false;
                    }, "error5 {id}", new IMessageArg[] {NumberArg.Create("id", 5)}))
                );

                var user = new User {Email = "", Address = new Address(), FirstLogin = DateTime.UtcNow};

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub(),
                    ValidationStrategy.Complete,
                    0);

                Assert.True(executed[0]);
                Assert.True(executed[1]);
                Assert.False(executed[2]);
                Assert.False(executed[3]);
                Assert.False(executed[4]);

                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection, new[] {"single error"});
                Assert.Empty(errorsCollection.Members);
            }

            [Fact]
            public void Should_ExecuteUntilFirstError_When_FailFast()
            {
                var executed = new bool[5];

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Email, be => be.Valid(m =>
                    {
                        executed[0] = true;

                        return true;
                    }, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)}))
                    .Valid(m =>
                    {
                        executed[1] = true;

                        return false;
                    }).WithMessage("error2")
                    .Member(m => m.Address, be => be.Valid(m =>
                    {
                        executed[2] = true;

                        return false;
                    }, "error3 {id}", new IMessageArg[] {NumberArg.Create("id", 3)}))
                    .Valid(m =>
                    {
                        executed[3] = true;

                        return true;
                    }).WithMessage("error4")
                    .Member(m => m.FirstLogin, be => be.Valid(m =>
                    {
                        executed[4] = true;

                        return false;
                    }, "error5 {id}", new IMessageArg[] {NumberArg.Create("id", 5)}))
                );

                var user = new User {Email = "", Address = new Address(), FirstLogin = DateTime.UtcNow};

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub(),
                    ValidationStrategy.FailFast,
                    0);

                Assert.True(executed[0]);
                Assert.True(executed[1]);
                Assert.False(executed[2]);
                Assert.False(executed[3]);
                Assert.False(executed[4]);

                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection, new[] {"error2"});
                Assert.Empty(errorsCollection.Members);
            }

            [Fact]
            public void Should_GroupMemberErrors()
            {
                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Email, be => be.Valid(m => false, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)}))
                    .Valid(m => false).WithMessage("error2")
                    .Member(m => m.Email, be => be.Valid(m => false, "error3 {id}", new IMessageArg[] {NumberArg.Create("id", 3)}))
                    .Valid(m => false).WithMessage("error4")
                    .Member(m => m.Email, be => be.Valid(m => false, "error5 {id}", new IMessageArg[] {NumberArg.Create("id", 5)}))
                );

                var user = new User {Email = "", Address = new Address(), FirstLogin = DateTime.UtcNow};

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub(),
                    ValidationStrategy.Complete,
                    0);

                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection, new[] {"error2", "error4"});
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Email"], new[] {"error1 1", "error3 3", "error5 5"});
            }

            [Fact]
            public void Should_ThrowException_When_MaxDepthExceeded()
            {
                var validator = ValidatorCreator.Create<User>(u => u);

                Assert.Throws<MaxDepthExceededException>(() =>
                {
                    ValidatorExecutor.Execute(
                        validator,
                        new User(),
                        new ExecutionContextStub
                        {
                            MaxDepth = 5
                        },
                        ValidationStrategy.Complete,
                        6);
                });
            }
        }

        public class NullableMemberScope
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_Pass_Strategy(ValidationStrategy validationStrategy)
            {
                var user = new User {FirstLogin = DateTime.UtcNow};

                var executed = new bool[3];

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.FirstLogin, be => be
                        .AsNullable(m => m
                            .Valid(n =>
                            {
                                executed[0] = true;

                                return true;
                            }, "error1 {arg}", new[] {NumberArg.Create("arg", 1)})
                            .AsRelative(n =>
                            {
                                executed[1] = true;

                                return false;
                            }).WithMessage("error2")
                            .Valid(n =>
                            {
                                executed[2] = true;

                                return false;
                            }, "error3 {arg}", new[] {NumberArg.Create("arg", 3)})
                        ))
                );

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub(),
                    validationStrategy,
                    0);

                Assert.Empty(errorsCollection.Errors);

                if (validationStrategy == ValidationStrategy.Complete)
                {
                    Assert.True(executed.All(i => i));
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"FirstLogin"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["FirstLogin"], new[] {"error2", "error3 3"});
                }
                else if (validationStrategy == ValidationStrategy.FailFast)
                {
                    Assert.True(executed[0]);
                    Assert.True(executed[1]);
                    Assert.False(executed[2]);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"FirstLogin"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["FirstLogin"], new[] {"error2"});
                }
                else if (validationStrategy == ValidationStrategy.Force)
                {
                    Assert.True(executed.All(i => !i));

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"FirstLogin"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["FirstLogin"], new[] {"Required", "error1 1", "error2", "error3 3"});
                }
            }

            [Theory]
            [InlineData(false, false)]
            [InlineData(false, true)]
            [InlineData(true, false)]
            [InlineData(true, true)]
            public void Should_AddSingleError(bool memberValid, bool modelValid)
            {
                var value = DateTime.UtcNow;

                var user = new User {FirstLogin = value};

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.FirstLogin, be => be.AsNullable(m => m
                        .Valid(n => memberValid, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)})
                        .AsRelative(n => modelValid).WithMessage("error2")
                        .SetSingleError("single error")
                    ))
                );

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub(),
                    ValidationStrategy.Complete,
                    0);

                if (memberValid && modelValid)
                {
                    Assert.Empty(errorsCollection.Errors);
                }
                else
                {
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"FirstLogin"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["FirstLogin"], new[] {"single error"});
                }
            }

            public static IEnumerable<object[]> Should_ThrowException_When_RuleNotAllowed_Data()
            {
                yield return new object[] {new CustomRule()};
                yield return new object[] {new AsModelRule<object>()};
                yield return new object[] {new AsCollectionRule<object, object>(c => c)};
                yield return new object[] {new AsNullableRule<object, int>(c => c)};
            }

            [Theory]
            [MemberData(nameof(Should_ThrowException_When_RuleNotAllowed_Data))]
            public void Should_ThrowException_When_RuleNotAllowed(object rule)
            {
                var memberValidator = new MemberValidator();
                var castRule = (IRule)rule;
                memberValidator.AddRule(castRule);

                Assert.Throws<InvalidRuleException>(() =>
                {
                    ValidatorExecutor.ExecuteNullableMember<object, int>(
                        memberValidator,
                        new object(),
                        0,
                        new ExecutionContextStub(),
                        ValidationStrategy.Complete);
                });
            }

            [Fact]
            public void Should_Pass_Values()
            {
                var value = DateTime.UtcNow;

                var user = new User {FirstLogin = value};

                var executed = new bool[2];

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.FirstLogin, be => be
                        .AsNullable(m => m
                            .Valid(n =>
                            {
                                Assert.Equal(value, n);
                                executed[0] = true;

                                return true;
                            })
                            .AsRelative(n =>
                            {
                                Assert.Same(user, n);
                                executed[1] = true;

                                return true;
                            })
                        ))
                );

                ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub(),
                    ValidationStrategy.Complete,
                    0);

                Assert.True(executed[0]);
                Assert.True(executed[1]);
            }
        }

        public class ModelMemberScope
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_Pass_Strategy(ValidationStrategy validationStrategy)
            {
                var user = new User {Address = new Address()};

                var executed = new bool[3];

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Address, be => be.AsModel(m => m
                        .Valid(n =>
                        {
                            executed[0] = true;

                            return true;
                        }).WithMessage("error1")
                        .Valid(n =>
                        {
                            executed[1] = true;

                            return false;
                        }).WithMessage("error2")
                        .Valid(n =>
                        {
                            executed[2] = true;

                            return false;
                        }).WithMessage("error3")
                    ))
                );

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    validationStrategy,
                    0);

                Assert.Empty(errorsCollection.Errors);

                if (validationStrategy == ValidationStrategy.Complete)
                {
                    Assert.True(executed.All(i => i));
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Address"], new[] {"error2", "error3"});
                }
                else if (validationStrategy == ValidationStrategy.FailFast)
                {
                    Assert.True(executed[0]);
                    Assert.True(executed[1]);
                    Assert.False(executed[2]);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Address"], new[] {"error2"});
                }
                else if (validationStrategy == ValidationStrategy.Force)
                {
                    Assert.True(executed.All(i => !i));

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Address"], new[] {"Required", "error1", "error2", "error3"});
                }
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_AddSingleError(bool isValid)
            {
                var user = new User {Address = new Address()};

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Address, be => be.AsModel(m => m
                        .Valid(n => isValid).WithMessage("error1 {id}")
                        .SetSingleError("single error")
                    ))
                );

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                if (isValid)
                {
                    Assert.Empty(errorsCollection.Errors);
                }
                else
                {
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Address"], new[] {"single error"});
                }
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_UseSpecificationFromSelectedSource(bool useDefined)
            {
                var user = new User {Address = new Address()};

                var executedDefined = 0;
                var executedFromRepository = 0;

                Specification<Address> defined = m => m
                    .Valid(n =>
                    {
                        Assert.Equal(user.Address, n);
                        executedDefined++;

                        return true;
                    });

                Specification<Address> fromRepository = m => m
                    .Valid(n =>
                    {
                        Assert.Equal(user.Address, n);
                        executedFromRepository++;

                        return true;
                    });

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Address, be => be.AsModel(useDefined ? defined : null))
                );

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(be => be.Get<Address>())
                    .Returns(fromRepository);

                ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                if (useDefined)
                {
                    Assert.Equal(1, executedDefined);
                    Assert.Equal(0, executedFromRepository);
                }
                else
                {
                    Assert.Equal(0, executedDefined);
                    Assert.Equal(1, executedFromRepository);
                }
            }

            [Theory]
            [InlineData(0, true)]
            [InlineData(1, true)]
            [InlineData(2, false)]
            [InlineData(3, false)]
            public void Should_Pass_DecrementedDepth(int maxDepth, bool expectException)
            {
                var user = new User {Address = new Address {StreetDetails = new StreetDetails()}};

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Address, be => be.AsModel(m => m
                        .Member(m1 => m1.StreetDetails, be1 => be1
                            .AsModel(m2 => m2
                                .Valid(x => true)))
                    ))
                );

                Action execution = () =>
                {
                    ValidatorExecutor.Execute(
                        validator,
                        user,
                        new ExecutionContextStub
                        {
                            MaxDepth = maxDepth,
                            ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                        },
                        ValidationStrategy.Complete,
                        0);
                };

                if (expectException)
                {
                    Assert.Throws<MaxDepthExceededException>(execution);
                }
                else
                {
                    execution();
                }
            }

            [Fact]
            public void Should_Pass_CollectionForceKey()
            {
                var user = new User {Address = new Address()};

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Address, be => be
                        .AsModel(m => m
                            .Member(n => n.Citizens, be2 => be2.AsCollection(m2 => m2
                                .Valid(x => true, "some error"))
                            )
                        ))
                );

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        CollectionForceKey = "[*]",
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Force,
                    0);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                Assert.Empty(errorsCollection.Errors);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["Address"], new[] {"Citizens"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Address"], new[] {"Required"});
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["Address"].Members["Citizens"], new[] {"[*]"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Address"].Members["Citizens"].Members["[*]"], new[] {"Required", "some error"});
            }

            [Fact]
            public void Should_Pass_DefaultError()
            {
                var user = new User {Address = new Address()};

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Address, be => be
                        .AsModel(m => m
                            .Valid(n => false)
                        ))
                );

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        DefaultError = new Error("custom default {arg}", new[] {new TextArg("arg", "error")}),
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Address"], new[] {"custom default error"});
            }

            [Fact]
            public void Should_Pass_RequiredError()
            {
                var user = new User {Address = new Address()};

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Address, be => be.AsModel(m => m.Member(n => n.Country)
                    )));

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        RequiredError = new Error("custom required {arg}", new[] {new TextArg("arg", "error")}),
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                Assert.Empty(errorsCollection.Errors);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["Address"], new[] {"Country"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["Address"].Members["Country"], new[] {"custom required error"});
            }

            [Fact]
            public void Should_Pass_Value()
            {
                var user = new User {Address = new Address()};

                var executed = false;

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.Address, be => be
                        .AsModel(m => m
                            .Valid(n =>
                            {
                                Assert.Equal(user.Address, n);
                                executed = true;

                                return true;
                            })
                        )
                    )
                );

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                Assert.True(executed);
            }

            [Fact]
            public void Should_ThrowException_When_RuleNotAllowed()
            {
                var memberValidator = new MemberValidator();
                memberValidator.AddRule(new CustomRule());

                Assert.Throws<InvalidRuleException>(() =>
                {
                    ValidatorExecutor.ExecuteMember(
                        memberValidator,
                        new object(),
                        0,
                        new ExecutionContextStub(),
                        ValidationStrategy.Complete,
                        0);
                });
            }
        }

        public class CollectionMemberScope
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_Pass_Strategy(ValidationStrategy validationStrategy)
            {
                var address1 = new Address();
                var address2 = new Address();
                var address3 = new Address();

                var pastAddresses = new[] {address1, address2, address3};
                var user = new User {PastAddresses = pastAddresses};

                var executed = new[]
                {
                    new[] {false, false, false},
                    new[] {false, false, false},
                    new[] {false, false, false}
                };

                var isValid = new[]
                {
                    new[] {true, true, true},
                    new[] {true, false, false},
                    new[] {false, false, false}
                };

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.PastAddresses, be => be
                        .AsCollection(m => m
                            .Valid(n =>
                            {
                                var itemIndex = Array.IndexOf(pastAddresses, n);
                                executed[itemIndex][0] = true;

                                return isValid[itemIndex][0];
                            }, "error1 {arg}", new[] {NumberArg.Create("arg", 1)})
                            .Valid(n =>
                            {
                                var itemIndex = Array.IndexOf(pastAddresses, n);
                                executed[itemIndex][1] = true;

                                return isValid[itemIndex][1];
                            }, "error2 {arg}", new[] {NumberArg.Create("arg", 2)})
                            .Valid(n =>
                            {
                                var itemIndex = Array.IndexOf(pastAddresses, n);
                                executed[itemIndex][2] = true;

                                return isValid[itemIndex][2];
                            }, "error3 {arg}", new[] {NumberArg.Create("arg", 3)})
                        )));

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    validationStrategy,
                    0);

                Assert.Empty(errorsCollection.Errors);

                if (validationStrategy == ValidationStrategy.Complete)
                {
                    Assert.True(executed.All(i => i.All(it => it)));
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"1", "2"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["1"], new[] {"error2 2", "error3 3"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["2"], new[] {"error1 1", "error2 2", "error3 3"});
                }
                else if (validationStrategy == ValidationStrategy.FailFast)
                {
                    Assert.True(executed[0].All(it => it));
                    Assert.True(executed[1][0]);
                    Assert.True(executed[1][1]);
                    Assert.True(executed[1][2] == false);
                    Assert.True(executed[2].All(it => !it));

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"1"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["1"], new[] {"error2 2"});
                }
                else if (validationStrategy == ValidationStrategy.Force)
                {
                    Assert.True(executed.All(i => i.All(it => !it)));

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new[] {"Required"});
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"*"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["*"], new[] {"Required", "error1 1", "error2 2", "error3 3"});
                }
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_AddSingleError(bool isValid)
            {
                var address1 = new Address();
                var address2 = new Address();
                var address3 = new Address();

                var pastAddresses = new[] {address1, address2, address3};
                var user = new User {PastAddresses = pastAddresses};

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.PastAddresses, be => be.AsCollection(m => m
                        .Valid(n => isValid, "error1 {id}", new IMessageArg[] {NumberArg.Create("id", 1)})
                        .SetSingleError("single error")
                    )));

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                if (isValid)
                {
                    Assert.Empty(errorsCollection.Errors);
                }
                else
                {
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    Assert.Empty(errorsCollection.Members["PastAddresses"].Errors);
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"0", "1", "2"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["0"], new[] {"single error"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["1"], new[] {"single error"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["2"], new[] {"single error"});
                }
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_UseValidatorFromSelectedSource(bool useDefined)
            {
                var address1 = new Address();
                var address2 = new Address();
                var address3 = new Address();

                var pastAddresses = new[] {address1, address2, address3};
                var user = new User {PastAddresses = pastAddresses};

                var executedDefined = 0;
                var executedFromRepository = 0;

                Specification<Address> defined = m => m
                    .Valid(n =>
                    {
                        Assert.Equal(user.PastAddresses.ElementAt(executedDefined), n);
                        executedDefined++;

                        return true;
                    });

                Specification<Address> fromRepository = m => m
                    .Valid(n =>
                    {
                        Assert.Equal(user.PastAddresses.ElementAt(executedFromRepository), n);
                        executedFromRepository++;

                        return true;
                    });

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.PastAddresses, be => be.AsCollection(m => m.AsModel(useDefined ? defined : null)))
                );

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(be => be.Get<Address>())
                    .Returns(fromRepository);

                ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                if (useDefined)
                {
                    Assert.Equal(3, executedDefined);
                    Assert.Equal(0, executedFromRepository);
                }
                else
                {
                    Assert.Equal(0, executedDefined);
                    Assert.Equal(3, executedFromRepository);
                }
            }

            [Theory]
            [InlineData(0, true)]
            [InlineData(1, true)]
            [InlineData(2, false)]
            [InlineData(3, false)]
            public void Should_Pass_DecrementedDepth(int maxDepth, bool expectException)
            {
                var address1 = new Address {StreetDetails = new StreetDetails()};
                var address2 = new Address {StreetDetails = new StreetDetails()};
                var address3 = new Address {StreetDetails = new StreetDetails()};

                var pastAddresses = new[] {address1, address2, address3};
                var user = new User {PastAddresses = pastAddresses};

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u.Member(m => m.PastAddresses, be => be
                    .AsCollection(m => m
                        .AsModel(m1 => m1
                            .Member(m2 => m2.StreetDetails, be2 => be2.AsModel(m3 => m3.Valid(v => true))))
                    ))
                );

                Action execution = () =>
                {
                    ValidatorExecutor.Execute(
                        validator,
                        user,
                        new ExecutionContextStub
                        {
                            MaxDepth = maxDepth,
                            ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                        },
                        ValidationStrategy.Complete,
                        0);
                };

                if (expectException)
                {
                    Assert.Throws<MaxDepthExceededException>(execution);
                }
                else
                {
                    execution();
                }
            }

            [Fact]
            public void Should_Pass_CollectionForceKey()
            {
                var user = new User();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.PastAddresses, be => be.AsCollection(be2 => be2.AsModel(m => m
                        .Member(n => n.Citizens, be3 => be3.AsCollection(m2 => m2.Valid(x => true, "some error")))
                    ))));

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        CollectionForceKey = "[*]",
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Force,
                    0);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                Assert.Empty(errorsCollection.Errors);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"[*]"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new[] {"Required"});
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"].Members["[*]"], new[] {"Citizens"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["[*]"], new[] {"Required"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["[*]"].Members["Citizens"].Members["[*]"], new[] {"Required", "some error"});
            }

            [Fact]
            public void Should_Pass_DefaultError()
            {
                var address1 = new Address();
                var address2 = new Address();
                var address3 = new Address();

                var pastAddresses = new[] {address1, address2, address3};
                var user = new User {PastAddresses = pastAddresses};

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.PastAddresses, be => be.AsCollection(m => m.Valid(n => false)))
                );

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        DefaultError = new Error("custom default {arg}", new[] {new TextArg("arg", "error")}),
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                Assert.Empty(errorsCollection.Members["PastAddresses"].Errors);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"0", "1", "2"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["0"], new[] {"custom default error"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["1"], new[] {"custom default error"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["2"], new[] {"custom default error"});
            }

            [Fact]
            public void Should_Pass_RequiredError()
            {
                var user = new User {PastAddresses = new Address[] {null, null, null}};

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.PastAddresses, be => be.AsCollection(m => m.Valid(n => false))
                    ));

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        RequiredError = new Error("custom required {arg}", new[] {new TextArg("arg", "error")}),
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                Assert.Empty(errorsCollection.Members["PastAddresses"].Errors);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"0", "1", "2"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["0"], new[] {"custom required error"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["1"], new[] {"custom required error"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["2"], new[] {"custom required error"});
            }


            [Fact]
            public void Should_Pass_Value()
            {
                var address1 = new Address();
                var address2 = new Address();
                var address3 = new Address();

                var pastAddresses = new[] {address1, address2, address3};
                var user = new User {PastAddresses = pastAddresses};

                var executed = 0;

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.PastAddresses, be => be.AsCollection(m => m
                        .Valid(n =>
                        {
                            Assert.Equal(user.PastAddresses.ElementAt(executed), n);
                            executed++;

                            return true;
                        })
                    )));

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                Assert.Equal(3, executed);
            }
        }

        public class ModelsCollectionMemberScope
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_Pass_Strategy(ValidationStrategy validationStrategy)
            {
                var address1 = new Address();
                var address2 = new Address();
                var address3 = new Address();

                var pastAddresses = new[] {address1, address2, address3};
                var user = new User {PastAddresses = pastAddresses};

                var executed = new[]
                {
                    new[] {false, false, false},
                    new[] {false, false, false},
                    new[] {false, false, false}
                };

                var isValid = new[]
                {
                    new[] {true, true, true},
                    new[] {true, false, false},
                    new[] {false, false, false}
                };

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.PastAddresses, be => be.AsModelsCollection(m => m
                        .Valid(n =>
                        {
                            var itemIndex = Array.IndexOf(pastAddresses, n);
                            executed[itemIndex][0] = true;

                            return isValid[itemIndex][0];
                        }).WithMessage("error1")
                        .Valid(n =>
                        {
                            var itemIndex = Array.IndexOf(pastAddresses, n);
                            executed[itemIndex][1] = true;

                            return isValid[itemIndex][1];
                        }).WithMessage("error2")
                        .Valid(n =>
                        {
                            var itemIndex = Array.IndexOf(pastAddresses, n);
                            executed[itemIndex][2] = true;

                            return isValid[itemIndex][2];
                        }).WithMessage("error3")
                    )));

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    validationStrategy,
                    0);

                Assert.Empty(errorsCollection.Errors);

                if (validationStrategy == ValidationStrategy.Complete)
                {
                    Assert.True(executed.All(i => i.All(it => it)));
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"1", "2"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["1"], new[] {"error2", "error3"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["2"], new[] {"error1", "error2", "error3"});
                }
                else if (validationStrategy == ValidationStrategy.FailFast)
                {
                    Assert.True(executed[0].All(it => it));
                    Assert.True(executed[1][0]);
                    Assert.True(executed[1][1]);
                    Assert.True(executed[1][2] == false);
                    Assert.True(executed[2].All(it => !it));

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"1"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["1"], new[] {"error2"});
                }
                else if (validationStrategy == ValidationStrategy.Force)
                {
                    Assert.True(executed.All(i => i.All(it => !it)));

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new[] {"Required"});
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"*"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["*"], new[] {"Required", "error1", "error2", "error3"});
                }
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_AddSingleError(bool isValid)
            {
                var address1 = new Address();
                var address2 = new Address();
                var address3 = new Address();

                var pastAddresses = new[] {address1, address2, address3};
                var user = new User {PastAddresses = pastAddresses};

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u.Member(m => m.PastAddresses, be => be.AsModelsCollection(m => m
                    .Valid(n => isValid).WithMessage("error 1")
                    .SetSingleError("single error")
                )));

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                if (isValid)
                {
                    Assert.Empty(errorsCollection.Errors);
                }
                else
                {
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    Assert.Empty(errorsCollection.Members["PastAddresses"].Errors);
                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"0", "1", "2"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["0"], new[] {"single error"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["1"], new[] {"single error"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["2"], new[] {"single error"});
                }
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_UseValidatorFromSelectedSource(bool useDefined)
            {
                var address1 = new Address();
                var address2 = new Address();
                var address3 = new Address();

                var pastAddresses = new[] {address1, address2, address3};
                var user = new User {PastAddresses = pastAddresses};

                var executedDefined = 0;
                var executedFromRepository = 0;

                Specification<Address> defined = m => m
                    .Valid(n =>
                    {
                        Assert.Equal(user.PastAddresses.ElementAt(executedDefined), n);
                        executedDefined++;

                        return true;
                    });

                Specification<Address> fromRepository = m => m
                    .Valid(n =>
                    {
                        Assert.Equal(user.PastAddresses.ElementAt(executedFromRepository), n);
                        executedFromRepository++;

                        return true;
                    });

                var validator = ValidatorCreator.Create<User>(u => u
                    .Member(m => m.PastAddresses, be => be.AsModelsCollection(useDefined ? defined : null))
                );

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                specificationsRepositoryMock
                    .Setup(be => be.Get<Address>())
                    .Returns(fromRepository);

                ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                if (useDefined)
                {
                    Assert.Equal(3, executedDefined);
                    Assert.Equal(0, executedFromRepository);
                }
                else
                {
                    Assert.Equal(0, executedDefined);
                    Assert.Equal(3, executedFromRepository);
                }
            }

            [Theory]
            [InlineData(0, true)]
            [InlineData(1, true)]
            [InlineData(2, false)]
            [InlineData(3, false)]
            public void Should_Pass_DecrementedDepth(int maxDepth, bool expectException)
            {
                var address1 = new Address {StreetDetails = new StreetDetails()};
                var address2 = new Address {StreetDetails = new StreetDetails()};
                var address3 = new Address {StreetDetails = new StreetDetails()};

                var pastAddresses = new[] {address1, address2, address3};
                var user = new User {PastAddresses = pastAddresses};

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u.Member(m => m.PastAddresses, be => be
                    .AsModelsCollection(m => m
                        .Member(m2 => m2.StreetDetails, be2 => be2.AsModel(m3 => m3.Valid(v => true)))
                    ))
                );

                Action execution = () =>
                {
                    ValidatorExecutor.Execute(
                        validator,
                        user,
                        new ExecutionContextStub
                        {
                            MaxDepth = maxDepth,
                            ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                        },
                        ValidationStrategy.Complete,
                        0);
                };

                if (expectException)
                {
                    Assert.Throws<MaxDepthExceededException>(execution);
                }
                else
                {
                    execution();
                }
            }

            [Fact]
            public void Should_Pass_CollectionForceKey()
            {
                var user = new User();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u.Member(m => m.PastAddresses, be => be.AsModelsCollection(m => m
                    .Member(n => n.Citizens, be3 => be3.AsCollection(m2 => m2.Valid(x => true, "some error")))))
                );

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        CollectionForceKey = "[*]",
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Force,
                    0);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                Assert.Empty(errorsCollection.Errors);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"[*]"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new[] {"Required"});
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"].Members["[*]"], new[] {"Citizens"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["[*]"], new[] {"Required"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["[*]"].Members["Citizens"].Members["[*]"], new[] {"Required", "some error"});
            }

            [Fact]
            public void Should_Pass_DefaultError()
            {
                var address1 = new Address();
                var address2 = new Address();
                var address3 = new Address();

                var pastAddresses = new[] {address1, address2, address3};
                var user = new User {PastAddresses = pastAddresses};

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u.Member(m => m.PastAddresses, be => be.AsModelsCollection(m => m
                    .Valid(n => false)
                )));

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        DefaultError = new Error("custom default {arg}", new[] {new TextArg("arg", "error")}),
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                Assert.Empty(errorsCollection.Members["PastAddresses"].Errors);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"0", "1", "2"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["0"], new[] {"custom default error"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["1"], new[] {"custom default error"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["2"], new[] {"custom default error"});
            }

            [Fact]
            public void Should_Pass_RequiredError()
            {
                var user = new User {PastAddresses = new Address[] {null, null, null}};

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var validator = ValidatorCreator.Create<User>(u => u.Member(m => m.PastAddresses, be => be.AsModelsCollection(m => m
                    .Valid(n => false)
                )));

                var errorsCollection = ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        RequiredError = new Error("custom required {arg}", new[] {new TextArg("arg", "error")}),
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                Assert.Empty(errorsCollection.Members["PastAddresses"].Errors);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"0", "1", "2"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["0"], new[] {"custom required error"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["1"], new[] {"custom required error"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["2"], new[] {"custom required error"});
            }

            [Fact]
            public void Should_Pass_Value()
            {
                var address1 = new Address();
                var address2 = new Address();
                var address3 = new Address();

                var pastAddresses = new[] {address1, address2, address3};
                var user = new User {PastAddresses = pastAddresses};

                var executed = 0;

                var validator = ValidatorCreator.Create<User>(u => u.Member(m => m.PastAddresses, be => be.AsModelsCollection(m => m
                    .Valid(n =>
                    {
                        Assert.Equal(user.PastAddresses.ElementAt(executed), n);
                        executed++;

                        return true;
                    })
                )));

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                ValidatorExecutor.Execute(
                    validator,
                    user,
                    new ExecutionContextStub
                    {
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
                    },
                    ValidationStrategy.Complete,
                    0);

                Assert.Equal(3, executed);
            }
        }


        private class CustomRule : IRule
        {
            public string Name { get; } = "Custom";
            public Error RuleSingleError { get; set; }
        }

        private class User
        {
            public string Email { get; set; }

            public DateTime? FirstLogin { get; set; }

            public Address Address { get; set; }

            public IEnumerable<Address> PastAddresses { get; set; }
        }


        private class Address
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Country { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public IEnumerable<string> Citizens { get; set; }
            public StreetDetails StreetDetails { get; set; }
        }

        private class StreetDetails
        {
        }
    }
}