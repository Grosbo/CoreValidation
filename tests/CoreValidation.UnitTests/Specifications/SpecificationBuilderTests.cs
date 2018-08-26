using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Validators;
using Moq;
using Xunit;

namespace CoreValidation.UnitTests.Specifications
{
    public class SpecificationBuilderTests
    {
        public static IEnumerable<object[]> TrueFalse_ValidErrorAndArgsCombinations_Data()
        {
            var args = new IMessageArg[] {new NumberArg("test1", 1), new TextArg("test2", "two")};

            yield return new object[] {true, "message", args};
            yield return new object[] {true, "message", null};
            yield return new object[] {true, null, null};
            yield return new object[] {false, "message", args};
            yield return new object[] {false, "message", null};
            yield return new object[] {false, null, null};
        }

        public static IEnumerable<object[]> ValidErrorAndArgsCombinations_Data()
        {
            var args = new IMessageArg[] {new NumberArg("test1", 1), new TextArg("test2", "two")};

            yield return new object[] {"message", args};
            yield return new object[] {"message", null};
            yield return new object[] {null, null};
        }

        public static IEnumerable<object[]> ArgsCombinations_Data()
        {
            var args = new IMessageArg[] {new NumberArg("test1", 1), new TextArg("test2", "two")};

            yield return new object[] {args};
            yield return new object[] {null};
        }

        public class SummaryError
        {
            [Theory]
            [MemberData(nameof(ArgsCombinations_Data), MemberType = typeof(SpecificationBuilderTests))]
            public void Should_SummaryError_BeAdded(IMessageArg[] args)
            {
                var builder = new SpecificationBuilder<User>();

                builder.WithSummaryError("message", args);

                Assert.NotNull(builder.ExecutableRules);
                Assert.Empty(builder.ExecutableRules);

                Assert.Equal("message", builder.SummaryError.Message);
                Assert.Same(args, builder.SummaryError.Arguments);
            }

            [Theory]
            [MemberData(nameof(ArgsCombinations_Data), MemberType = typeof(SpecificationBuilderTests))]
            public void Should_ThrowException_When_SummaryError_Added_With_NullMessage(IMessageArg[] args)
            {
                var builder = new SpecificationBuilder<User>();

                Assert.Throws<ArgumentNullException>(() => { builder.WithSummaryError(null, args); });
            }

            [Fact]
            public void Should_SummaryError_NotBeAdded_When_NoMethod()
            {
                var builder = new SpecificationBuilder<User>();

                Assert.NotNull(builder.ExecutableRules);
                Assert.Empty(builder.ExecutableRules);

                Assert.Null(builder.SummaryError);
            }


            [Fact]
            public void Should_ThrowException_When_SummaryError_Added_MultipleTimes()
            {
                var builder = new SpecificationBuilder<User>();

                Assert.Throws<InvalidOperationException>(() =>
                {
                    builder
                        .WithSummaryError("message1")
                        .WithSummaryError("message2");
                });
            }
        }

        public class SelfRule
        {
            [Theory]
            [MemberData(nameof(ValidErrorAndArgsCombinations_Data), MemberType = typeof(SpecificationBuilderTests))]
            public void Should_ExecutableSelfRule_BeAdded(string message, IMessageArg[] args)
            {
                var builder = new SpecificationBuilder<User>();

                builder.Valid(m => true, message, args);

                Assert.NotNull(builder.ExecutableRules);
                Assert.Single(builder.ExecutableRules);

                var rule = builder.ExecutableRules.Single();

                Assert.IsType<ExecutableSelfRule<User>>(rule);

                var executableSelfRule = (ExecutableSelfRule<User>)rule;

                if (message != null)
                {
                    Assert.Equal(message, executableSelfRule.Rule.Error.Message);
                    Assert.Same(args, executableSelfRule.Rule.Error.Arguments);
                }
                else
                {
                    Assert.Null(executableSelfRule.Rule.Error);
                }
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_ExecutableSelfRule_AddErrorIfInvalid(bool isValid)
            {
                var user = new User();
                var executed = false;

                var args = new IMessageArg[] {new NumberArg("test1", 1), new TextArg("test2", "two")};

                var builder = new SpecificationBuilder<User>();

                builder.Valid(m =>
                {
                    executed = true;

                    return isValid;
                }, "message", args);

                Assert.NotNull(builder.ExecutableRules);
                Assert.Single(builder.ExecutableRules);

                var rule = builder.ExecutableRules.Single();

                Assert.IsType<ExecutableSelfRule<User>>(rule);

                var executableSelfRule = (ExecutableSelfRule<User>)rule;

                var errorAdded = executableSelfRule.TryExecuteAndGetErrors(user, new RulesExecutionContext
                    {
                        RulesOptions = new RulesOptionsStub(),
                        ValidationStrategy = ValidationStrategy.Complete
                    },
                    0,
                    out var errorsCollection);

                Assert.True(executed);
                Assert.Equal(!isValid, errorAdded);

                if (errorAdded)
                {
                    var error = errorsCollection.Errors.Single();
                    Assert.Equal("message", error.Message);
                    Assert.Equal(args, error.Arguments);
                }
            }

            [Fact]
            public void Should_ExecutableSelfRule_BeAdded_MultipleTimes()
            {
                var builder = new SpecificationBuilder<User>();

                var args = new[]
                {
                    new IMessageArg[] { },
                    new IMessageArg[] { },
                    new IMessageArg[] { }
                };

                builder
                    .Valid(m => true, "message1", args[0])
                    .Valid(m => true, "message2", args[1])
                    .Valid(m => true, "message3", args[2]);

                Assert.NotNull(builder.ExecutableRules);
                Assert.Equal(3, builder.ExecutableRules.Count);

                for (var i = 0; i < 3; ++i)
                {
                    var rule = builder.ExecutableRules.ElementAt(i);

                    Assert.IsType<ExecutableSelfRule<User>>(rule);

                    var executableSelfRule = (ExecutableSelfRule<User>)rule;

                    Assert.Equal($"message{i + 1}", executableSelfRule.Rule.Error.Message);
                    Assert.Same(args[i], executableSelfRule.Rule.Error.Arguments);
                }
            }

            [Fact]
            public void Should_ExecutableSelfRule_Receive_ModelReference()
            {
                var user = new User();
                var compared = false;

                var builder = new SpecificationBuilder<User>();

                builder.Valid(m =>
                {
                    Assert.Same(user, m);
                    compared = true;

                    return false;
                });

                Assert.NotNull(builder.ExecutableRules);
                Assert.Single(builder.ExecutableRules);

                var rule = builder.ExecutableRules.Single();

                Assert.IsType<ExecutableSelfRule<User>>(rule);

                var executableSelfRule = (ExecutableSelfRule<User>)rule;

                executableSelfRule.TryExecuteAndGetErrors(user, new RulesExecutionContext
                    {
                        RulesOptions = new RulesOptionsStub(),
                        ValidationStrategy = ValidationStrategy.Complete
                    },
                    0,
                    out _);

                Assert.True(compared);
            }
        }

        public class GeneralMemberRule
        {
            [Theory]
            [MemberData(nameof(ArgsCombinations_Data), MemberType = typeof(SpecificationBuilderTests))]
            public void Should_MemberRule_BeAdded_WithRequiredError(IMessageArg[] args)
            {
                var builder = new SpecificationBuilder<User>();

                builder.For(m => m.Email, be => be.WithRequiredError("message", args));

                Assert.NotNull(builder.ExecutableRules);
                Assert.Single(builder.ExecutableRules);

                var rule = builder.ExecutableRules.Single();

                Assert.IsType<ExecutableMemberRule<User, string>>(rule);

                var executableMemberRule = (ExecutableMemberRule<User, string>)rule;

                Assert.Equal(nameof(User.Email), executableMemberRule.Name);

                Assert.Equal(typeof(User).GetProperty(nameof(User.Email)), executableMemberRule.MemberPropertyInfo);

                Assert.Empty(executableMemberRule.MemberSpecification.Rules);
                Assert.Null(executableMemberRule.MemberSpecification.Name);

                Assert.Equal("message", executableMemberRule.MemberSpecification.RequiredError.Message);
                Assert.Equal(args, executableMemberRule.MemberSpecification.RequiredError.Arguments);

                Assert.Null(executableMemberRule.MemberSpecification.SummaryError);
                Assert.False(executableMemberRule.MemberSpecification.IsOptional);
            }

            [Theory]
            [MemberData(nameof(ArgsCombinations_Data), MemberType = typeof(SpecificationBuilderTests))]
            public void Should_MemberRule_BeAdded_WithSummaryError(IMessageArg[] args)
            {
                var builder = new SpecificationBuilder<User>();

                builder.For(m => m.Email, be => be.WithSummaryError("message", args));

                Assert.NotNull(builder.ExecutableRules);
                Assert.Single(builder.ExecutableRules);

                var rule = builder.ExecutableRules.Single();

                Assert.IsType<ExecutableMemberRule<User, string>>(rule);

                var executableMemberRule = (ExecutableMemberRule<User, string>)rule;

                Assert.Equal(nameof(User.Email), executableMemberRule.Name);

                Assert.Equal(typeof(User).GetProperty(nameof(User.Email)), executableMemberRule.MemberPropertyInfo);

                Assert.Empty(executableMemberRule.MemberSpecification.Rules);
                Assert.Null(executableMemberRule.MemberSpecification.Name);
                Assert.Null(executableMemberRule.MemberSpecification.RequiredError);

                Assert.Equal("message", executableMemberRule.MemberSpecification.SummaryError.Message);
                Assert.Equal(args, executableMemberRule.MemberSpecification.SummaryError.Arguments);

                Assert.False(executableMemberRule.MemberSpecification.IsOptional);
            }

            [Theory]
            [MemberData(nameof(ArgsCombinations_Data), MemberType = typeof(SpecificationBuilderTests))]
            public void Should_ThrowException_When_MemberRule_Added_WithRequiredError_And_NullMessage(IMessageArg[] args)
            {
                var builder = new SpecificationBuilder<User>();

                Assert.Throws<ArgumentNullException>(() => { builder.For(m => m.Email, be => be.WithRequiredError(null, args)); });
            }

            [Theory]
            [MemberData(nameof(ArgsCombinations_Data), MemberType = typeof(SpecificationBuilderTests))]
            public void Should_ThrowException_When_MemberRule_Added_WithSummaryError_And_NullMessage(IMessageArg[] args)
            {
                var builder = new SpecificationBuilder<User>();

                Assert.Throws<ArgumentNullException>(() => { builder.For(m => m.Email, be => be.WithSummaryError(null, args)); });
            }

            [Fact]
            public void Should_MemberRule_BeAdded()
            {
                var builder = new SpecificationBuilder<User>();

                builder.For(m => m.Email);

                Assert.NotNull(builder.ExecutableRules);
                Assert.Single(builder.ExecutableRules);

                var rule = builder.ExecutableRules.Single();

                Assert.IsType<ExecutableMemberRule<User, string>>(rule);

                var executableMemberRule = (ExecutableMemberRule<User, string>)rule;

                Assert.Equal(nameof(User.Email), executableMemberRule.Name);

                Assert.Equal(typeof(User).GetProperty(nameof(User.Email)), executableMemberRule.MemberPropertyInfo);

                Assert.Empty(executableMemberRule.MemberSpecification.Rules);
                Assert.Null(executableMemberRule.MemberSpecification.Name);
                Assert.Null(executableMemberRule.MemberSpecification.RequiredError);
                Assert.Null(executableMemberRule.MemberSpecification.SummaryError);
                Assert.False(executableMemberRule.MemberSpecification.IsOptional);
            }

            [Fact]
            public void Should_MemberRule_BeAdded_MultipleTimes()
            {
                var builder = new SpecificationBuilder<User>();

                builder
                    .For(m => m.Email)
                    .For(m => m.Email)
                    .For(m => m.Email);

                Assert.NotNull(builder.ExecutableRules);
                Assert.Equal(3, builder.ExecutableRules.Count);

                for (var i = 0; i < 3; ++i)
                {
                    var rule = builder.ExecutableRules.ElementAt(i);

                    Assert.IsType<ExecutableMemberRule<User, string>>(rule);

                    var executableMemberRule = (ExecutableMemberRule<User, string>)rule;

                    Assert.Equal(nameof(User.Email), executableMemberRule.Name);

                    Assert.Equal(typeof(User).GetProperty(nameof(User.Email)), executableMemberRule.MemberPropertyInfo);

                    Assert.Empty(executableMemberRule.MemberSpecification.Rules);
                    Assert.Null(executableMemberRule.MemberSpecification.Name);
                    Assert.Null(executableMemberRule.MemberSpecification.RequiredError);
                    Assert.Null(executableMemberRule.MemberSpecification.SummaryError);
                    Assert.False(executableMemberRule.MemberSpecification.IsOptional);
                }
            }

            [Fact]
            public void Should_MemberRule_BeAdded_MultipleTimes_DifferentMembers()
            {
                var builder = new SpecificationBuilder<User>();

                builder
                    .For(m => m.Email)
                    .For(m => m.Address)
                    .For(m => m.PastAddresses)
                    .For(m => m.FirstLogin)
                    .For(m => m.IsAdmin);

                Assert.NotNull(builder.ExecutableRules);
                Assert.Equal(5, builder.ExecutableRules.Count);

                var rule1 = builder.ExecutableRules.ElementAt(0);
                Assert.IsType<ExecutableMemberRule<User, string>>(rule1);
                Assert.Equal(nameof(User.Email), ((ExecutableMemberRule<User, string>)rule1).Name);
                Assert.Equal(typeof(User).GetProperty(nameof(User.Email)), ((ExecutableMemberRule<User, string>)rule1).MemberPropertyInfo);

                var rule2 = builder.ExecutableRules.ElementAt(1);
                Assert.IsType<ExecutableMemberRule<User, Address>>(rule2);
                Assert.Equal(nameof(User.Address), ((ExecutableMemberRule<User, Address>)rule2).Name);
                Assert.Equal(typeof(User).GetProperty(nameof(User.Address)), ((ExecutableMemberRule<User, Address>)rule2).MemberPropertyInfo);

                var rule3 = builder.ExecutableRules.ElementAt(2);
                Assert.IsType<ExecutableMemberRule<User, IEnumerable<Address>>>(rule3);
                Assert.Equal(nameof(User.PastAddresses), ((ExecutableMemberRule<User, IEnumerable<Address>>)rule3).Name);
                Assert.Equal(typeof(User).GetProperty(nameof(User.PastAddresses)), ((ExecutableMemberRule<User, IEnumerable<Address>>)rule3).MemberPropertyInfo);

                var rule4 = builder.ExecutableRules.ElementAt(3);
                Assert.IsType<ExecutableMemberRule<User, DateTime?>>(rule4);
                Assert.Equal(nameof(User.FirstLogin), ((ExecutableMemberRule<User, DateTime?>)rule4).Name);
                Assert.Equal(typeof(User).GetProperty(nameof(User.FirstLogin)), ((ExecutableMemberRule<User, DateTime?>)rule4).MemberPropertyInfo);

                var rule5 = builder.ExecutableRules.ElementAt(4);
                Assert.IsType<ExecutableMemberRule<User, bool>>(rule5);
                Assert.Equal(nameof(User.IsAdmin), ((ExecutableMemberRule<User, bool>)rule5).Name);
                Assert.Equal(typeof(User).GetProperty(nameof(User.IsAdmin)), ((ExecutableMemberRule<User, bool>)rule5).MemberPropertyInfo);
            }

            [Fact]
            public void Should_MemberRule_BeAdded_Optional()
            {
                var builder = new SpecificationBuilder<User>();

                builder.For(m => m.Email, be => be.Optional());

                Assert.NotNull(builder.ExecutableRules);
                Assert.Single(builder.ExecutableRules);

                var rule = builder.ExecutableRules.Single();

                Assert.IsType<ExecutableMemberRule<User, string>>(rule);

                var executableMemberRule = (ExecutableMemberRule<User, string>)rule;

                Assert.Equal(nameof(User.Email), executableMemberRule.Name);

                Assert.Equal(typeof(User).GetProperty(nameof(User.Email)), executableMemberRule.MemberPropertyInfo);

                Assert.Empty(executableMemberRule.MemberSpecification.Rules);
                Assert.Null(executableMemberRule.MemberSpecification.Name);
                Assert.Null(executableMemberRule.MemberSpecification.RequiredError);
                Assert.Null(executableMemberRule.MemberSpecification.SummaryError);
                Assert.True(executableMemberRule.MemberSpecification.IsOptional);
            }

            [Fact]
            public void Should_MemberRule_BeAdded_WithName()
            {
                var builder = new SpecificationBuilder<User>();

                builder.For(m => m.Email, be => be.WithName("new_name"));

                Assert.NotNull(builder.ExecutableRules);
                Assert.Single(builder.ExecutableRules);

                var rule = builder.ExecutableRules.Single();

                Assert.IsType<ExecutableMemberRule<User, string>>(rule);

                var executableMemberRule = (ExecutableMemberRule<User, string>)rule;

                Assert.Equal("new_name", executableMemberRule.Name);

                Assert.Equal(typeof(User).GetProperty(nameof(User.Email)), executableMemberRule.MemberPropertyInfo);

                Assert.Empty(executableMemberRule.MemberSpecification.Rules);
                Assert.Equal("new_name", executableMemberRule.MemberSpecification.Name);
                Assert.Null(executableMemberRule.MemberSpecification.RequiredError);
                Assert.Null(executableMemberRule.MemberSpecification.SummaryError);
                Assert.False(executableMemberRule.MemberSpecification.IsOptional);
            }

            [Fact]
            public void Should_ThrowException_When_MemberRule_Added_Optional_MultipleTimes()
            {
                var builder = new SpecificationBuilder<User>();

                Assert.Throws<InvalidOperationException>(() =>
                {
                    builder.For(m => m.Email, be => be
                        .Optional()
                        .Optional()
                    );
                });
            }

            [Fact]
            public void Should_ThrowException_When_MemberRule_Added_WithName_MultipleTimes()
            {
                var builder = new SpecificationBuilder<User>();

                Assert.Throws<InvalidOperationException>(() =>
                {
                    builder.For(m => m.Email, be => be
                        .WithName("name1")
                        .WithName("name2")
                    );
                });
            }

            [Fact]
            public void Should_ThrowException_When_MemberRule_Added_WithRequiredError_MultipleTimes()
            {
                var builder = new SpecificationBuilder<User>();

                Assert.Throws<InvalidOperationException>(() =>
                {
                    builder.For(m => m.Email, be => be
                        .WithRequiredError("error1")
                        .WithRequiredError("error2")
                    );
                });
            }

            [Fact]
            public void Should_ThrowException_When_MemberRule_Added_WithSummaryError_And_MultipleTimes()
            {
                var builder = new SpecificationBuilder<User>();

                Assert.Throws<InvalidOperationException>(() =>
                {
                    builder.For(m => m.Email, be => be
                        .WithSummaryError("error1")
                        .WithSummaryError("error2"));
                });
            }
        }

        public class MemberRules
        {
            [Theory]
            [MemberData(nameof(TrueFalse_ValidErrorAndArgsCombinations_Data), MemberType = typeof(SpecificationBuilderTests))]
            public void Should_ValidRule_BeAdded_When_Valid(bool isValid, string message, IMessageArg[] args)
            {
                var user = new User
                {
                    Address = new Address()
                };

                var builder = new SpecificationBuilder<User>();

                var executed = 0;
                builder.For(m => m.Address, be => be.Valid(v =>
                {
                    Assert.Same(user.Address, v);
                    executed++;

                    return isValid;
                }, message, args));

                var executableMemberRule = (ExecutableMemberRule<User, Address>)builder.ExecutableRules.Single();

                var rulesOptions = new RulesOptionsStub();

                var errorAdded = executableMemberRule.TryExecuteAndGetErrors(user, new RulesExecutionContext
                    {
                        RulesOptions = rulesOptions,
                        ValidationStrategy = ValidationStrategy.Complete
                    },
                    0,
                    out var errorsCollection);

                Assert.Equal(1, executed);
                Assert.Equal(!isValid, errorAdded);
                Assert.NotNull(errorsCollection);
                Assert.Empty(errorsCollection.Members);

                if (isValid)
                {
                    Assert.Empty(errorsCollection.Errors);
                }
                else if (message != null)
                {
                    Assert.Equal(message, errorsCollection.Errors.Single().Message);
                    Assert.Same(args, errorsCollection.Errors.Single().Arguments);
                }
                else
                {
                    Assert.Equal(rulesOptions.DefaultError, errorsCollection.Errors.Single());
                }
            }

            [Theory]
            [MemberData(nameof(TrueFalse_ValidErrorAndArgsCombinations_Data), MemberType = typeof(SpecificationBuilderTests))]
            public void Should_ValidRelativeRule_BeAdded_When_ValidRelative(bool isValid, string message, IMessageArg[] args)
            {
                var user = new User
                {
                    Address = new Address()
                };

                var builder = new SpecificationBuilder<User>();

                var executed = 0;
                builder.For(m => m.Address, be => be.ValidRelative(v =>
                {
                    Assert.Same(user, v);
                    executed++;

                    return isValid;
                }, message, args));

                var executableMemberRule = (ExecutableMemberRule<User, Address>)builder.ExecutableRules.Single();

                var rulesOptions = new RulesOptionsStub();

                var errorAdded = executableMemberRule.TryExecuteAndGetErrors(user, new RulesExecutionContext
                    {
                        RulesOptions = rulesOptions,
                        ValidationStrategy = ValidationStrategy.Complete
                    },
                    0,
                    out var errorsCollection);

                Assert.Equal(1, executed);
                Assert.Equal(!isValid, errorAdded);
                Assert.NotNull(errorsCollection);
                Assert.Empty(errorsCollection.Members);

                if (isValid)
                {
                    Assert.Empty(errorsCollection.Errors);
                }
                else if (message != null)
                {
                    Assert.Equal(message, errorsCollection.Errors.Single().Message);
                    Assert.Same(args, errorsCollection.Errors.Single().Arguments);
                }
                else
                {
                    Assert.Equal(rulesOptions.DefaultError, errorsCollection.Errors.Single());
                }
            }

            [Theory]
            [MemberData(nameof(TrueFalse_ValidErrorAndArgsCombinations_Data), MemberType = typeof(SpecificationBuilderTests))]
            public void Should_ValidNullableRule_BeAdded_When_ValidNullable(bool isValid, string message, IMessageArg[] args)
            {
                var user = new User
                {
                    FirstLogin = DateTime.Today
                };

                var builder = new SpecificationBuilder<User>();

                var executed = 0;
                builder.For(m => m.FirstLogin, be => be.ValidNullable(v => v
                    .Valid(m =>
                    {
                        Assert.Equal(user.FirstLogin, m);
                        executed++;

                        return isValid;
                    }, message, args)
                ));

                var executableMemberRule = (ExecutableMemberRule<User, DateTime?>)builder.ExecutableRules.Single();

                var rulesOptions = new RulesOptionsStub();

                var errorAdded = executableMemberRule.TryExecuteAndGetErrors(user, new RulesExecutionContext
                    {
                        RulesOptions = rulesOptions,
                        ValidationStrategy = ValidationStrategy.Complete
                    },
                    0,
                    out var errorsCollection);

                Assert.Equal(1, executed);
                Assert.Equal(!isValid, errorAdded);
                Assert.NotNull(errorsCollection);
                Assert.Empty(errorsCollection.Members);

                if (isValid)
                {
                    Assert.Empty(errorsCollection.Errors);
                }
                else if (message != null)
                {
                    Assert.Equal(message, errorsCollection.Errors.Single().Message);
                    Assert.Same(args, errorsCollection.Errors.Single().Arguments);
                }
                else
                {
                    Assert.Equal(rulesOptions.DefaultError, errorsCollection.Errors.Single());
                }
            }

            [Theory]
            [MemberData(nameof(TrueFalse_ValidErrorAndArgsCombinations_Data), MemberType = typeof(SpecificationBuilderTests))]
            public void Should_ValidModelRule_BeAdded_When_ValidModel(bool isValid, string message, IMessageArg[] args)
            {
                var user = new User
                {
                    Address = new Address()
                };

                var builder = new SpecificationBuilder<User>();

                var executed = 0;
                builder.For(m => m.Address, be => be.ValidModel(v => v
                    .Valid(m =>
                    {
                        Assert.Equal(user.Address, m);
                        executed++;

                        return isValid;
                    }, message, args)));

                var executableMemberRule = (ExecutableMemberRule<User, Address>)builder.ExecutableRules.Single();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rulesOptions = new RulesOptionsStub();

                var errorAdded = executableMemberRule.TryExecuteAndGetErrors(user, new RulesExecutionContext
                    {
                        RulesOptions = rulesOptions,
                        ValidationStrategy = ValidationStrategy.Complete,
                        SpecificationsRepository = new SpecificationsRepository(validatorsRepositoryMock.Object)
                    },
                    0,
                    out var errorsCollection);

                Assert.Equal(1, executed);
                Assert.Equal(!isValid, errorAdded);
                Assert.NotNull(errorsCollection);
                Assert.Empty(errorsCollection.Members);

                if (isValid)
                {
                    Assert.Empty(errorsCollection.Errors);
                }
                else if (message != null)
                {
                    Assert.Equal(message, errorsCollection.Errors.Single().Message);
                    Assert.Same(args, errorsCollection.Errors.Single().Arguments);
                }
                else
                {
                    Assert.Equal(rulesOptions.DefaultError, errorsCollection.Errors.Single());
                }
            }

            [Theory]
            [MemberData(nameof(TrueFalse_ValidErrorAndArgsCombinations_Data), MemberType = typeof(SpecificationBuilderTests))]
            public void Should_ValidCollectionRule_BeAdded_When_ValidCollection(bool isValid, string message, IMessageArg[] args)
            {
                var user = new User
                {
                    PastAddresses = new[] {new Address()}
                };

                var builder = new SpecificationBuilder<User>();

                var executed = 0;
                builder.For(m => m.PastAddresses, be => be.ValidCollection(c => c.Valid(m =>
                {
                    Assert.Equal(user.PastAddresses.Single(), m);
                    executed++;

                    return isValid;
                }, message, args)));

                var executableMemberRule = (ExecutableMemberRule<User, IEnumerable<Address>>)builder.ExecutableRules.Single();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rulesOptions = new RulesOptionsStub();

                var errorAdded = executableMemberRule.TryExecuteAndGetErrors(user, new RulesExecutionContext
                    {
                        RulesOptions = rulesOptions,
                        ValidationStrategy = ValidationStrategy.Complete,
                        SpecificationsRepository = new SpecificationsRepository(validatorsRepositoryMock.Object)
                    },
                    0,
                    out var errorsCollection);

                Assert.Equal(1, executed);
                Assert.Equal(!isValid, errorAdded);
                Assert.NotNull(errorsCollection);
                Assert.Empty(errorsCollection.Errors);

                if (isValid)
                {
                    Assert.Empty(errorsCollection.Members);
                }
                else if (message != null)
                {
                    Assert.Equal(message, errorsCollection.Members["0"].Errors.Single().Message);
                    Assert.Same(args, errorsCollection.Members["0"].Errors.Single().Arguments);
                }
                else
                {
                    Assert.Equal(rulesOptions.DefaultError, errorsCollection.Members["0"].Errors.Single());
                }
            }

            [Theory]
            [MemberData(nameof(TrueFalse_ValidErrorAndArgsCombinations_Data), MemberType = typeof(SpecificationBuilderTests))]
            public void Should_ValidCollectionRule_BeAdded_When_ValidModelsCollection(bool isValid, string message, IMessageArg[] args)
            {
                var user = new User
                {
                    PastAddresses = new[] {new Address()}
                };

                var builder = new SpecificationBuilder<User>();

                var executed = 0;
                builder.For(m => m.PastAddresses, be => be.ValidModelsCollection(c => c.Valid(m =>
                {
                    Assert.Equal(user.PastAddresses.Single(), m);
                    executed++;

                    return isValid;
                }, message, args)));

                var executableMemberRule = (ExecutableMemberRule<User, IEnumerable<Address>>)builder.ExecutableRules.Single();

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                var rulesOptions = new RulesOptionsStub();

                var errorAdded = executableMemberRule.TryExecuteAndGetErrors(user, new RulesExecutionContext
                    {
                        RulesOptions = rulesOptions,
                        ValidationStrategy = ValidationStrategy.Complete,
                        SpecificationsRepository = new SpecificationsRepository(validatorsRepositoryMock.Object)
                    },
                    0,
                    out var errorsCollection);

                Assert.Equal(1, executed);
                Assert.Equal(!isValid, errorAdded);
                Assert.NotNull(errorsCollection);
                Assert.Empty(errorsCollection.Errors);

                if (isValid)
                {
                    Assert.Empty(errorsCollection.Members);
                }
                else if (message != null)
                {
                    Assert.Equal(message, errorsCollection.Members["0"].Errors.Single().Message);
                    Assert.Same(args, errorsCollection.Members["0"].Errors.Single().Arguments);
                }
                else
                {
                    Assert.Equal(rulesOptions.DefaultError, errorsCollection.Members["0"].Errors.Single());
                }
            }
        }

        private class User
        {
            public string Email { get; set; }

            public DateTime? FirstLogin { get; set; }

            public bool IsAdmin { get; set; }

            public Address Address { get; set; }

            public IEnumerable<Address> PastAddresses { get; set; }
        }

        private class Address
        {
        }
    }
}