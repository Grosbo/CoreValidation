using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Validators;
using CoreValidation.Validators.Scopes;
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

                Assert.NotNull(builder.Scopes);
                Assert.Empty(builder.Scopes);

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

                Assert.NotNull(builder.Scopes);
                Assert.Empty(builder.Scopes);

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

                Assert.NotNull(builder.Scopes);
                Assert.Single(builder.Scopes);

                var rule = builder.Scopes.Single();

                Assert.IsType<ModelScope<User>>(rule);

                var executableSelfRule = (ModelScope<User>)rule;

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

                Assert.NotNull(builder.Scopes);
                Assert.Single(builder.Scopes);

                var rule = builder.Scopes.Single();

                Assert.IsType<ModelScope<User>>(rule);

                var executableSelfRule = (ModelScope<User>)rule;

                var errorAdded = executableSelfRule.TryGetErrors(user, new ExecutionContext
                    {
                        ExecutionOptions = new ExecutionOptionsStub(),
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

                Assert.NotNull(builder.Scopes);
                Assert.Equal(3, builder.Scopes.Count);

                for (var i = 0; i < 3; ++i)
                {
                    var rule = builder.Scopes.ElementAt(i);

                    Assert.IsType<ModelScope<User>>(rule);

                    var executableSelfRule = (ModelScope<User>)rule;

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

                Assert.NotNull(builder.Scopes);
                Assert.Single(builder.Scopes);

                var rule = builder.Scopes.Single();

                Assert.IsType<ModelScope<User>>(rule);

                var executableSelfRule = (ModelScope<User>)rule;

                executableSelfRule.TryGetErrors(user, new ExecutionContext
                    {
                        ExecutionOptions = new ExecutionOptionsStub(),
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

                Assert.NotNull(builder.Scopes);
                Assert.Single(builder.Scopes);

                var rule = builder.Scopes.Single();

                Assert.IsType<MemberScope<User, string>>(rule);

                var executableMemberRule = (MemberScope<User, string>)rule;

                Assert.Equal(nameof(User.Email), executableMemberRule.Name);

                Assert.Equal(typeof(User).GetProperty(nameof(User.Email)), executableMemberRule.MemberPropertyInfo);

                Assert.Empty(executableMemberRule.MemberValidator.Rules);
                Assert.Null(executableMemberRule.MemberValidator.Name);

                Assert.Equal("message", executableMemberRule.MemberValidator.RequiredError.Message);
                Assert.Equal(args, executableMemberRule.MemberValidator.RequiredError.Arguments);

                Assert.Null(executableMemberRule.MemberValidator.SummaryError);
                Assert.False(executableMemberRule.MemberValidator.IsOptional);
            }

            [Theory]
            [MemberData(nameof(ArgsCombinations_Data), MemberType = typeof(SpecificationBuilderTests))]
            public void Should_MemberRule_BeAdded_WithSummaryError(IMessageArg[] args)
            {
                var builder = new SpecificationBuilder<User>();

                builder.For(m => m.Email, be => be.WithSummaryError("message", args));

                Assert.NotNull(builder.Scopes);
                Assert.Single(builder.Scopes);

                var rule = builder.Scopes.Single();

                Assert.IsType<MemberScope<User, string>>(rule);

                var executableMemberRule = (MemberScope<User, string>)rule;

                Assert.Equal(nameof(User.Email), executableMemberRule.Name);

                Assert.Equal(typeof(User).GetProperty(nameof(User.Email)), executableMemberRule.MemberPropertyInfo);

                Assert.Empty(executableMemberRule.MemberValidator.Rules);
                Assert.Null(executableMemberRule.MemberValidator.Name);
                Assert.Null(executableMemberRule.MemberValidator.RequiredError);

                Assert.Equal("message", executableMemberRule.MemberValidator.SummaryError.Message);
                Assert.Equal(args, executableMemberRule.MemberValidator.SummaryError.Arguments);

                Assert.False(executableMemberRule.MemberValidator.IsOptional);
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

                Assert.NotNull(builder.Scopes);
                Assert.Single(builder.Scopes);

                var rule = builder.Scopes.Single();

                Assert.IsType<MemberScope<User, string>>(rule);

                var executableMemberRule = (MemberScope<User, string>)rule;

                Assert.Equal(nameof(User.Email), executableMemberRule.Name);

                Assert.Equal(typeof(User).GetProperty(nameof(User.Email)), executableMemberRule.MemberPropertyInfo);

                Assert.Empty(executableMemberRule.MemberValidator.Rules);
                Assert.Null(executableMemberRule.MemberValidator.Name);
                Assert.Null(executableMemberRule.MemberValidator.RequiredError);
                Assert.Null(executableMemberRule.MemberValidator.SummaryError);
                Assert.False(executableMemberRule.MemberValidator.IsOptional);
            }

            [Fact]
            public void Should_MemberRule_BeAdded_MultipleTimes()
            {
                var builder = new SpecificationBuilder<User>();

                builder
                    .For(m => m.Email)
                    .For(m => m.Email)
                    .For(m => m.Email);

                Assert.NotNull(builder.Scopes);
                Assert.Equal(3, builder.Scopes.Count);

                for (var i = 0; i < 3; ++i)
                {
                    var rule = builder.Scopes.ElementAt(i);

                    Assert.IsType<MemberScope<User, string>>(rule);

                    var executableMemberRule = (MemberScope<User, string>)rule;

                    Assert.Equal(nameof(User.Email), executableMemberRule.Name);

                    Assert.Equal(typeof(User).GetProperty(nameof(User.Email)), executableMemberRule.MemberPropertyInfo);

                    Assert.Empty(executableMemberRule.MemberValidator.Rules);
                    Assert.Null(executableMemberRule.MemberValidator.Name);
                    Assert.Null(executableMemberRule.MemberValidator.RequiredError);
                    Assert.Null(executableMemberRule.MemberValidator.SummaryError);
                    Assert.False(executableMemberRule.MemberValidator.IsOptional);
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

                Assert.NotNull(builder.Scopes);
                Assert.Equal(5, builder.Scopes.Count);

                var rule1 = builder.Scopes.ElementAt(0);
                Assert.IsType<MemberScope<User, string>>(rule1);
                Assert.Equal(nameof(User.Email), ((MemberScope<User, string>)rule1).Name);
                Assert.Equal(typeof(User).GetProperty(nameof(User.Email)), ((MemberScope<User, string>)rule1).MemberPropertyInfo);

                var rule2 = builder.Scopes.ElementAt(1);
                Assert.IsType<MemberScope<User, Address>>(rule2);
                Assert.Equal(nameof(User.Address), ((MemberScope<User, Address>)rule2).Name);
                Assert.Equal(typeof(User).GetProperty(nameof(User.Address)), ((MemberScope<User, Address>)rule2).MemberPropertyInfo);

                var rule3 = builder.Scopes.ElementAt(2);
                Assert.IsType<MemberScope<User, IEnumerable<Address>>>(rule3);
                Assert.Equal(nameof(User.PastAddresses), ((MemberScope<User, IEnumerable<Address>>)rule3).Name);
                Assert.Equal(typeof(User).GetProperty(nameof(User.PastAddresses)), ((MemberScope<User, IEnumerable<Address>>)rule3).MemberPropertyInfo);

                var rule4 = builder.Scopes.ElementAt(3);
                Assert.IsType<MemberScope<User, DateTime?>>(rule4);
                Assert.Equal(nameof(User.FirstLogin), ((MemberScope<User, DateTime?>)rule4).Name);
                Assert.Equal(typeof(User).GetProperty(nameof(User.FirstLogin)), ((MemberScope<User, DateTime?>)rule4).MemberPropertyInfo);

                var rule5 = builder.Scopes.ElementAt(4);
                Assert.IsType<MemberScope<User, bool>>(rule5);
                Assert.Equal(nameof(User.IsAdmin), ((MemberScope<User, bool>)rule5).Name);
                Assert.Equal(typeof(User).GetProperty(nameof(User.IsAdmin)), ((MemberScope<User, bool>)rule5).MemberPropertyInfo);
            }

            [Fact]
            public void Should_MemberRule_BeAdded_Optional()
            {
                var builder = new SpecificationBuilder<User>();

                builder.For(m => m.Email, be => be.Optional());

                Assert.NotNull(builder.Scopes);
                Assert.Single(builder.Scopes);

                var rule = builder.Scopes.Single();

                Assert.IsType<MemberScope<User, string>>(rule);

                var executableMemberRule = (MemberScope<User, string>)rule;

                Assert.Equal(nameof(User.Email), executableMemberRule.Name);

                Assert.Equal(typeof(User).GetProperty(nameof(User.Email)), executableMemberRule.MemberPropertyInfo);

                Assert.Empty(executableMemberRule.MemberValidator.Rules);
                Assert.Null(executableMemberRule.MemberValidator.Name);
                Assert.Null(executableMemberRule.MemberValidator.RequiredError);
                Assert.Null(executableMemberRule.MemberValidator.SummaryError);
                Assert.True(executableMemberRule.MemberValidator.IsOptional);
            }

            [Fact]
            public void Should_MemberRule_BeAdded_WithName()
            {
                var builder = new SpecificationBuilder<User>();

                builder.For(m => m.Email, be => be.WithName("new_name"));

                Assert.NotNull(builder.Scopes);
                Assert.Single(builder.Scopes);

                var rule = builder.Scopes.Single();

                Assert.IsType<MemberScope<User, string>>(rule);

                var executableMemberRule = (MemberScope<User, string>)rule;

                Assert.Equal("new_name", executableMemberRule.Name);

                Assert.Equal(typeof(User).GetProperty(nameof(User.Email)), executableMemberRule.MemberPropertyInfo);

                Assert.Empty(executableMemberRule.MemberValidator.Rules);
                Assert.Equal("new_name", executableMemberRule.MemberValidator.Name);
                Assert.Null(executableMemberRule.MemberValidator.RequiredError);
                Assert.Null(executableMemberRule.MemberValidator.SummaryError);
                Assert.False(executableMemberRule.MemberValidator.IsOptional);
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

                var executableMemberRule = (MemberScope<User, Address>)builder.Scopes.Single();

                var executionOptions = new ExecutionOptionsStub();

                var errorAdded = executableMemberRule.TryGetErrors(user, new ExecutionContext
                    {
                        ExecutionOptions = executionOptions,
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
                    Assert.Equal(executionOptions.DefaultError, errorsCollection.Errors.Single());
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

                var executableMemberRule = (MemberScope<User, Address>)builder.Scopes.Single();

                var executionOptions = new ExecutionOptionsStub();

                var errorAdded = executableMemberRule.TryGetErrors(user, new ExecutionContext
                    {
                        ExecutionOptions = executionOptions,
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
                    Assert.Equal(executionOptions.DefaultError, errorsCollection.Errors.Single());
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

                var executableMemberRule = (MemberScope<User, DateTime?>)builder.Scopes.Single();

                var executionOptions = new ExecutionOptionsStub();

                var errorAdded = executableMemberRule.TryGetErrors(user, new ExecutionContext
                    {
                        ExecutionOptions = executionOptions,
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
                    Assert.Equal(executionOptions.DefaultError, errorsCollection.Errors.Single());
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

                var executableMemberRule = (MemberScope<User, Address>)builder.Scopes.Single();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var executionOptions = new ExecutionOptionsStub();

                var errorAdded = executableMemberRule.TryGetErrors(user, new ExecutionContext
                    {
                        ExecutionOptions = executionOptions,
                        ValidationStrategy = ValidationStrategy.Complete,
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
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
                    Assert.Equal(executionOptions.DefaultError, errorsCollection.Errors.Single());
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

                var executableMemberRule = (MemberScope<User, IEnumerable<Address>>)builder.Scopes.Single();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var executionOptions = new ExecutionOptionsStub();

                var errorAdded = executableMemberRule.TryGetErrors(user, new ExecutionContext
                    {
                        ExecutionOptions = executionOptions,
                        ValidationStrategy = ValidationStrategy.Complete,
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
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
                    Assert.Equal(executionOptions.DefaultError, errorsCollection.Members["0"].Errors.Single());
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

                var executableMemberRule = (MemberScope<User, IEnumerable<Address>>)builder.Scopes.Single();

                var specificationsRepositoryMock = new Mock<ISpecificationsRepository>();

                var executionOptions = new ExecutionOptionsStub();

                var errorAdded = executableMemberRule.TryGetErrors(user, new ExecutionContext
                    {
                        ExecutionOptions = executionOptions,
                        ValidationStrategy = ValidationStrategy.Complete,
                        ValidatorsFactory = new ValidatorsFactory(specificationsRepositoryMock.Object)
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
                    Assert.Equal(executionOptions.DefaultError, errorsCollection.Members["0"].Errors.Single());
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