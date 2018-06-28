using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors;
using CoreValidation.Errors.Args;
using CoreValidation.Exceptions;
using CoreValidation.Specifications;
using CoreValidation.UnitTests.Errors;
using CoreValidation.Validators;
using Moq;
using Xunit;

namespace CoreValidation.UnitTests.Specifications
{
    public class SpecificationTests
    {
        internal static void AssertCompilableRule<T>(CompilableRule<T> rule, string memberName, Type memberType)
        {
            Assert.NotNull(rule);

            if (memberName == null)
            {
                Assert.Null(rule.RulesCollection);
                Assert.Null(rule.MemberPropertyInfo);
            }
            else
            {
                Assert.NotNull(rule.RulesCollection);
                Assert.NotNull(rule.MemberPropertyInfo);
                Assert.Equal(memberName, rule.MemberPropertyInfo.Name);
                Assert.Equal(memberType, rule.MemberPropertyInfo.PropertyType);
            }
        }

        internal static void AssertError(Error error, string message, string stringifiedMessage, string[] argsNames = null)
        {
            Assert.NotNull(error);
            Assert.Equal(message, error.Message);
            Assert.Equal(stringifiedMessage, error.StringifiedMessage);

            if (argsNames == null)
            {
                Assert.Null(error.Arguments);
            }
            else
            {
                Assert.Equal(error.Arguments.Count, argsNames.Length);

                foreach (var argName in argsNames)
                {
                    Assert.Contains(error.Arguments, m => m.Name == argName);
                }
            }
        }

        internal static (Validator<T>, IValidatorsRepository) GetValidatorObjects<T>(bool useRepository, Validator<T> validator)
            where T : class
        {
            var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

            if (useRepository)
            {
                validatorsRepositoryMock
                    .Setup(be => be.Get<T>())
                    .Returns(validator);
            }

            return (useRepository ? null : validator, validatorsRepositoryMock.Object);
        }

        [Theory]
        [InlineData(ValidationStrategy.Complete)]
        [InlineData(ValidationStrategy.FailFast)]
        [InlineData(ValidationStrategy.Force)]
        public void Should_SetModel(ValidationStrategy validationStrategy)
        {
            var model = new User {Email = "", FirstLogin = DateTime.MinValue};
            var specification = new Specification<User>(model, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

            var validCounter = 0;

            specification
                .Valid(c =>
                {
                    Assert.Same(model, c);
                    validCounter++;

                    return false;
                }, "error");

            Assert.Same(model, specification.Model);

            Assert.Null(specification.SummaryError);
            Assert.Equal(1, specification.CompilableRules.Count);

            var rule = specification.CompilableRules.ElementAt(0);

            AssertCompilableRule(rule, null, null);

            Assert.Equal(0, validCounter);

            var errorsCollection = specification.GetErrors();

            Assert.Equal(validationStrategy == ValidationStrategy.Force ? 0 : 1, validCounter);

            ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new string[] { });
            ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors(), new[] {"error"});
        }

        public class Address
        {
            public string Street { get; set; }

            public IEnumerable<string> Opinions { get; set; }
        }

        private class User
        {
            public string Email { get; set; }

            public DateTime? FirstLogin { get; set; }

            public bool IsAdmin { get; set; }

            public Address Address { get; set; }

            public IEnumerable<Address> PastAddresses { get; set; }
        }

        public class WithRequiredError
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_CompileNoError_When_NullModel(ValidationStrategy validationStrategy)
            {
                var specification = new Specification<User>(null, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification
                    .Valid(c =>
                    {
                        validCounter++;

                        return false;
                    }, "error");

                Assert.Null(specification.Model);

                Assert.Null(specification.SummaryError);
                Assert.Equal(1, specification.CompilableRules.Count);

                var rule = specification.CompilableRules.ElementAt(0);

                AssertCompilableRule(rule, null, null);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);

                Assert.True(errorsCollection.IsEmpty);
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_SetDefaultRequiredError_When_Nullmember(ValidationStrategy validationStrategy)
            {
                var specification = new Specification<User>(new User {Email = null}, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub {RequiredError = new Error("required{arg}", new IMessageArg[] {new MessageArg("arg", "!")})});

                var validCounter = 0;

                specification.For(c => c.Email, r => r.Valid(m =>
                {
                    validCounter++;

                    return false;
                }, "error"));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                Assert.Equal("Email", rule.MemberPropertyInfo.Name);
                Assert.Equal(typeof(string), rule.MemberPropertyInfo.PropertyType);

                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.Equal("error", ((ValidRule<string>)rule.RulesCollection.Rules.Single()).Message);
                Assert.Null(((ValidRule<string>)rule.RulesCollection.Rules.Single()).Arguments);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Email"], validationStrategy == ValidationStrategy.Force ? new[] {"required!", "error"} : new[] {"required!"});
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_SetCustomRequiredError_When_WithRequiredError_Nullmember(ValidationStrategy validationStrategy)
            {
                var specification = new Specification<User>(new User {Email = null}, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification.For(c => c.Email, r => r
                    .WithRequiredError("required_custom{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
                    .Valid(m =>
                    {
                        validCounter++;

                        return false;
                    }, "error"));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                Assert.Equal("Email", rule.MemberPropertyInfo.Name);
                Assert.Equal(typeof(string), rule.MemberPropertyInfo.PropertyType);

                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);

                AssertError(rule.RulesCollection.RequiredError, "required_custom{arg}", "required_custom!", new[] {"arg"});

                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.Equal("error", ((ValidRule<string>)rule.RulesCollection.Rules.Single()).Message);
                Assert.Null(((ValidRule<string>)rule.RulesCollection.Rules.Single()).Arguments);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Email"], validationStrategy == ValidationStrategy.Force ? new[] {"required_custom!", "error"} : new[] {"required_custom!"});
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_CompileRequiredError_When_WithRequiredError_And_WithSummaryError_And_Nullmember(ValidationStrategy validationStrategy)
            {
                var specification = new Specification<User>(new User {Email = null}, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification.For(c => c.Email, r => r
                    .WithRequiredError("required_custom{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
                    .WithSummaryError("summary{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
                    .Valid(m =>
                    {
                        validCounter++;

                        return false;
                    }, "error"));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                Assert.Equal("Email", rule.MemberPropertyInfo.Name);
                Assert.Equal(typeof(string), rule.MemberPropertyInfo.PropertyType);

                Assert.Null(rule.RulesCollection.Name);

                AssertError(rule.RulesCollection.SummaryError, "summary{arg}", "summary!", new[] {"arg"});
                AssertError(rule.RulesCollection.RequiredError, "required_custom{arg}", "required_custom!", new[] {"arg"});

                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.Equal("error", ((ValidRule<string>)rule.RulesCollection.Rules.Single()).Message);
                Assert.Null(((ValidRule<string>)rule.RulesCollection.Rules.Single()).Arguments);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Email"], validationStrategy == ValidationStrategy.Force ? new[] {"required_custom!", "summary!"} : new[] {"required_custom!"});
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_CompileError_When_WithRequiredError_And_WithSummaryError(ValidationStrategy validationStrategy)
            {
                var specification = new Specification<User>(new User {Email = ""}, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification.For(c => c.Email, r => r
                    .WithRequiredError("required_custom{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
                    .WithSummaryError("summary{arg}", new IMessageArg[] {new MessageArg("arg", "!")})
                    .Valid(m =>
                    {
                        validCounter++;

                        return false;
                    }, "error"));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                Assert.Equal("Email", rule.MemberPropertyInfo.Name);
                Assert.Equal(typeof(string), rule.MemberPropertyInfo.PropertyType);

                Assert.Null(rule.RulesCollection.Name);
                AssertError(rule.RulesCollection.SummaryError, "summary{arg}", "summary!", new[] {"arg"});
                AssertError(rule.RulesCollection.RequiredError, "required_custom{arg}", "required_custom!", new[] {"arg"});
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.Equal("error", ((ValidRule<string>)rule.RulesCollection.Rules.Single()).Message);
                Assert.Null(((ValidRule<string>)rule.RulesCollection.Rules.Single()).Arguments);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(validationStrategy == ValidationStrategy.Force ? 0 : 1, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Email"], validationStrategy == ValidationStrategy.Force ? new[] {"required_custom!", "summary!"} : new[] {"summary!"});
            }

            [Fact]
            public void Should_CompileWithForce_When_NullModel_And_ForceStrategy()
            {
                var specification = new Specification<User>(null, new VoidValidatorsRepository(), ValidationStrategy.Force, 0, new RulesOptionsStub());

                var validCounter = 0;
                var validCounter2 = 0;

                specification
                    .Valid(c =>
                    {
                        validCounter++;

                        return true;
                    }, "error")
                    .Valid(c =>
                    {
                        validCounter2++;

                        return true;
                    }, "error2");

                Assert.Null(specification.Model);

                Assert.Null(specification.SummaryError);
                Assert.Equal(2, specification.CompilableRules.Count);

                AssertCompilableRule(specification.CompilableRules.ElementAt(0), null, null);
                AssertCompilableRule(specification.CompilableRules.ElementAt(1), null, null);

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new string[] { });
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection, new[] {"error", "error2"});
            }
        }

        public class AddingBasicRules
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_Add_ValidRule(ValidationStrategy validationStrategy)
            {
                var model = new User {Email = ""};
                var specification = new Specification<User>(model, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification.For(c => c.Email, r => r.Valid(m =>
                {
                    Assert.Same(model.Email, m);
                    validCounter++;

                    return false;
                }, "error"));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "Email", typeof(string));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidRule<string>>(rule.RulesCollection.Rules.Single());

                var validRule = (ValidRule<string>)rule.RulesCollection.Rules.Single();
                Assert.Equal("error", validRule.Message);
                Assert.Null(validRule.Arguments);
                Assert.NotNull(validRule.IsValid);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(validationStrategy == ValidationStrategy.Force ? 0 : 1, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Email"], validationStrategy == ValidationStrategy.Force ? new[] {"Required", "error"} : new[] {"error"});
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_Add_ValidRule_And_PassEqualValueType(ValidationStrategy validationStrategy)
            {
                var model = new User {Email = "", IsAdmin = true};
                var specification = new Specification<User>(model, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification.For(c => c.IsAdmin, r => r.Valid(m =>
                {
                    Assert.Equal(model.IsAdmin, m);
                    validCounter++;

                    return false;
                }, "error"));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "IsAdmin", typeof(bool));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidRule<bool>>(rule.RulesCollection.Rules.Single());

                var validRule = (ValidRule<bool>)rule.RulesCollection.Rules.Single();
                Assert.Equal("error", validRule.Message);
                Assert.Null(validRule.Arguments);
                Assert.NotNull(validRule.IsValid);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(validationStrategy == ValidationStrategy.Force ? 0 : 1, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"IsAdmin"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["IsAdmin"], validationStrategy == ValidationStrategy.Force ? new[] {"Required", "error"} : new[] {"error"});
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_Add_ValidRelativeRule(ValidationStrategy validationStrategy)
            {
                var model = new User {Email = ""};
                var specification = new Specification<User>(model, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification.For(c => c.Email, r => r.ValidRelative(m =>
                {
                    Assert.Same(model, m);
                    validCounter++;

                    return false;
                }, "error"));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "Email", typeof(string));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidRelativeRule<User>>(rule.RulesCollection.Rules.Single());

                var validRelativeRule = (ValidRelativeRule<User>)rule.RulesCollection.Rules.Single();
                Assert.Equal("error", validRelativeRule.Message);
                Assert.Null(validRelativeRule.Arguments);
                Assert.NotNull(validRelativeRule.IsValid);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(validationStrategy == ValidationStrategy.Force ? 0 : 1, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Email"], validationStrategy == ValidationStrategy.Force ? new[] {"Required", "error"} : new[] {"error"});
            }
        }

        public class AddingValidNullableRule
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_Add_ValidNullableRule(ValidationStrategy validationStrategy)
            {
                var model = new User {FirstLogin = DateTime.MaxValue};
                var specification = new Specification<User>(model, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification.For(c => c.FirstLogin, r => r.ValidNullable(n => n.Valid(m =>
                {
                    Assert.True(model.FirstLogin.HasValue);
                    Assert.Equal(model.FirstLogin.Value, m);
                    validCounter++;

                    return false;
                }, "error")));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "FirstLogin", typeof(DateTime?));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidNullableRule<User, DateTime>>(rule.RulesCollection.Rules.Single());

                var validNullableRule = (ValidNullableRule<User, DateTime>)rule.RulesCollection.Rules.Single();
                Assert.NotNull(validNullableRule.MemberValidator);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(validationStrategy == ValidationStrategy.Force ? 0 : 1, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"FirstLogin"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["FirstLogin"], validationStrategy == ValidationStrategy.Force ? new[] {"Required", "error"} : new[] {"error"});
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_Add_ValidNullableRule_And_NotExecuteIfNull(ValidationStrategy validationStrategy)
            {
                var model = new User {FirstLogin = null};
                var specification = new Specification<User>(model, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification.For(c => c.FirstLogin, r => r.ValidNullable(n => n.Valid(m =>
                {
                    validCounter++;

                    return false;
                }, "error")));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "FirstLogin", typeof(DateTime?));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidNullableRule<User, DateTime>>(rule.RulesCollection.Rules.Single());

                var validNullableRule = (ValidNullableRule<User, DateTime>)rule.RulesCollection.Rules.Single();
                Assert.NotNull(validNullableRule.MemberValidator);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"FirstLogin"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["FirstLogin"], new[] {"Required"});
            }

            [Fact]
            public void Should_Add_ValidNullableRule_And_ExecuteWithForceIfNull()
            {
                var model = new User {FirstLogin = null};
                var specification = new Specification<User>(model, new VoidValidatorsRepository(), ValidationStrategy.Force, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification.For(c => c.FirstLogin, r => r.ValidNullable(n => n.Valid(m =>
                {
                    validCounter++;

                    return false;
                }, "error")));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "FirstLogin", typeof(DateTime?));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidNullableRule<User, DateTime>>(rule.RulesCollection.Rules.Single());

                var validNullableRule = (ValidNullableRule<User, DateTime>)rule.RulesCollection.Rules.Single();
                Assert.NotNull(validNullableRule.MemberValidator);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"FirstLogin"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["FirstLogin"], new[] {"Required", "error"});
            }

            [Fact]
            public void Should_Add_ValidNullableRule_And_PassCompleteStrategy()
            {
                var model = new User {FirstLogin = DateTime.MaxValue};
                var specification = new Specification<User>(model, new VoidValidatorsRepository(), ValidationStrategy.Complete, 0, new RulesOptionsStub());

                var validCounter = 0;
                var validCounter2 = 0;

                specification.For(c => c.FirstLogin, r => r.ValidNullable(n => n.Valid(m =>
                    {
                        Assert.True(model.FirstLogin.HasValue);
                        Assert.Equal(model.FirstLogin.Value, m);
                        validCounter++;

                        return false;
                    }, "error")
                    .Valid(m =>
                    {
                        Assert.True(model.FirstLogin.HasValue);
                        Assert.Equal(model.FirstLogin.Value, m);
                        validCounter2++;

                        return false;
                    }, "error2")));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "FirstLogin", typeof(DateTime?));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidNullableRule<User, DateTime>>(rule.RulesCollection.Rules.Single());

                var validNullableRule = (ValidNullableRule<User, DateTime>)rule.RulesCollection.Rules.Single();
                Assert.NotNull(validNullableRule.MemberValidator);

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(1, validCounter);
                Assert.Equal(1, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"FirstLogin"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["FirstLogin"], new[] {"error", "error2"});
            }

            [Fact]
            public void Should_Add_ValidNullableRule_And_PassFailFastStrategy()
            {
                var model = new User {FirstLogin = DateTime.MaxValue};
                var specification = new Specification<User>(model, new VoidValidatorsRepository(), ValidationStrategy.FailFast, 0, new RulesOptionsStub());

                var validCounter = 0;
                var validCounter2 = 0;

                specification.For(c => c.FirstLogin, r => r.ValidNullable(n => n.Valid(m =>
                    {
                        Assert.True(model.FirstLogin.HasValue);
                        Assert.Equal(model.FirstLogin.Value, m);
                        validCounter++;

                        return false;
                    }, "error")
                    .Valid(m =>
                    {
                        Assert.True(model.FirstLogin.HasValue);
                        Assert.Equal(model.FirstLogin.Value, m);
                        validCounter2++;

                        return false;
                    }, "error2")));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "FirstLogin", typeof(DateTime?));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidNullableRule<User, DateTime>>(rule.RulesCollection.Rules.Single());

                var validNullableRule = (ValidNullableRule<User, DateTime>)rule.RulesCollection.Rules.Single();
                Assert.NotNull(validNullableRule.MemberValidator);

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(1, validCounter);
                Assert.Equal(0, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"FirstLogin"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["FirstLogin"], new[] {"error"});
            }

            [Fact]
            public void Should_Add_ValidNullableRule_And_PassForceStrategy()
            {
                var model = new User {FirstLogin = DateTime.MaxValue};
                var specification = new Specification<User>(model, new VoidValidatorsRepository(), ValidationStrategy.Force, 0, new RulesOptionsStub());

                var validCounter = 0;
                var validCounter2 = 0;

                specification.For(c => c.FirstLogin, r => r.ValidNullable(n => n.Valid(m =>
                    {
                        Assert.True(model.FirstLogin.HasValue);
                        Assert.Equal(model.FirstLogin.Value, m);
                        validCounter++;

                        return true;
                    }, "error")
                    .Valid(m =>
                    {
                        Assert.True(model.FirstLogin.HasValue);
                        Assert.Equal(model.FirstLogin.Value, m);
                        validCounter2++;

                        return false;
                    }, "error2")));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "FirstLogin", typeof(DateTime?));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidNullableRule<User, DateTime>>(rule.RulesCollection.Rules.Single());

                var validNullableRule = (ValidNullableRule<User, DateTime>)rule.RulesCollection.Rules.Single();
                Assert.NotNull(validNullableRule.MemberValidator);

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"FirstLogin"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["FirstLogin"], new[] {"Required", "error", "error2"});
            }
        }

        public class AddingValidModelRule
        {
            [Theory]
            [InlineData(true, ValidationStrategy.Complete)]
            [InlineData(false, ValidationStrategy.Complete)]
            [InlineData(true, ValidationStrategy.FailFast)]
            [InlineData(false, ValidationStrategy.FailFast)]
            [InlineData(true, ValidationStrategy.Force)]
            [InlineData(false, ValidationStrategy.Force)]
            public void Should_Add_ValidModelRule(bool useRepository, ValidationStrategy validationStrategy)
            {
                var validCounter = 0;

                var model = new User {Address = new Address {Street = ""}};

                var (validator, repository) = GetValidatorObjects<Address>(useRepository, n => n.Valid(m =>
                {
                    Assert.Same(model.Address, m);
                    validCounter++;

                    return false;
                }, "error"));

                var specification = new Specification<User>(model, repository, validationStrategy, 0, new RulesOptionsStub());

                specification.For(c => c.Address, r => r.ValidModel(validator));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "Address", typeof(Address));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidModelRule<Address>>(rule.RulesCollection.Rules.Single());

                var validNullableRule = (ValidModelRule<Address>)rule.RulesCollection.Rules.Single();
                Assert.Equal(validator, validNullableRule.Validator);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(validationStrategy == ValidationStrategy.Force ? 0 : 1, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"], validationStrategy == ValidationStrategy.Force ? new[] {"Required", "error"} : new[] {"error"});
            }

            [Theory]
            [InlineData(true, ValidationStrategy.Complete)]
            [InlineData(false, ValidationStrategy.Complete)]
            [InlineData(true, ValidationStrategy.FailFast)]
            [InlineData(false, ValidationStrategy.FailFast)]
            [InlineData(true, ValidationStrategy.Force)]
            [InlineData(false, ValidationStrategy.Force)]
            public void Should_Add_ValidModelRule_And_GatherInternalMembers(bool useRepository, ValidationStrategy validationStrategy)
            {
                var validCounter = 0;

                var model = new User {Address = new Address {Street = ""}};

                var (validator, repository) = GetValidatorObjects<Address>(useRepository, n => n
                    .For(m => m.Street, x => x.Valid(s =>
                    {
                        Assert.Equal("", s);
                        validCounter++;

                        return false;
                    }, "error internal")));

                var specification = new Specification<User>(model, repository, validationStrategy, 0, new RulesOptionsStub());

                specification.For(c => c.Address, r => r.ValidModel(validator));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "Address", typeof(Address));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidModelRule<Address>>(rule.RulesCollection.Rules.Single());

                var validNullableRule = (ValidModelRule<Address>)rule.RulesCollection.Rules.Single();
                Assert.Equal(validator, validNullableRule.Validator);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(validationStrategy == ValidationStrategy.Force ? 0 : 1, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"], validationStrategy == ValidationStrategy.Force ? new[] {"Required"} : new string[] { });

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["Address"], new[] {"Street"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"].Members["Street"], validationStrategy == ValidationStrategy.Force ? new[] {"Required", "error internal"} : new[] {"error internal"});
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Add_ValidModelRule_And_PassCompleteStrategy(bool useRepository)
            {
                var validCounter = 0;
                var validCounter2 = 0;

                var model = new User {Address = new Address {Street = ""}};

                var (validator, repository) = GetValidatorObjects<Address>(useRepository, n => n
                    .Valid(m =>
                    {
                        Assert.Same(model.Address, m);
                        validCounter++;

                        return false;
                    }, "error")
                    .Valid(m =>
                    {
                        Assert.Same(model.Address, m);
                        validCounter2++;

                        return false;
                    }, "error2"));

                var specification = new Specification<User>(model, repository, ValidationStrategy.Complete, 0, new RulesOptionsStub());

                specification.For(c => c.Address, r => r.ValidModel(validator));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "Address", typeof(Address));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidModelRule<Address>>(rule.RulesCollection.Rules.Single());

                var validNullableRule = (ValidModelRule<Address>)rule.RulesCollection.Rules.Single();
                Assert.Equal(validator, validNullableRule.Validator);

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(1, validCounter);
                Assert.Equal(1, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"], new[] {"error", "error2"});
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Add_ValidModelRule_And_PassFailFastStrategy(bool useRepository)
            {
                var validCounter = 0;
                var validCounter2 = 0;

                var model = new User {Address = new Address {Street = ""}};

                var (validator, repository) = GetValidatorObjects<Address>(useRepository, n => n
                    .Valid(m =>
                    {
                        Assert.Same(model.Address, m);
                        validCounter++;

                        return false;
                    }, "error")
                    .Valid(m =>
                    {
                        Assert.Same(model.Address, m);
                        validCounter2++;

                        return false;
                    }, "error2"));

                var specification = new Specification<User>(model, repository, ValidationStrategy.FailFast, 0, new RulesOptionsStub());

                specification.For(c => c.Address, r => r.ValidModel(validator));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "Address", typeof(Address));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidModelRule<Address>>(rule.RulesCollection.Rules.Single());

                var validNullableRule = (ValidModelRule<Address>)rule.RulesCollection.Rules.Single();
                Assert.Equal(validator, validNullableRule.Validator);

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(1, validCounter);
                Assert.Equal(0, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"], new[] {"error"});
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_Add_ValidModelRule_And_PassForceStrategy(bool useRepository)
            {
                var validCounter = 0;
                var validCounter2 = 0;

                var model = new User {Address = new Address {Street = ""}};

                var (validator, repository) = GetValidatorObjects<Address>(useRepository, n => n
                    .Valid(m =>
                    {
                        Assert.Same(model.Address, m);
                        validCounter++;

                        return true;
                    }, "error")
                    .Valid(m =>
                    {
                        Assert.Same(model.Address, m);
                        validCounter2++;

                        return false;
                    }, "error2"));

                var specification = new Specification<User>(model, repository, ValidationStrategy.Force, 0, new RulesOptionsStub());

                specification.For(c => c.Address, r => r.ValidModel(validator));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "Address", typeof(Address));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidModelRule<Address>>(rule.RulesCollection.Rules.Single());

                var validNullableRule = (ValidModelRule<Address>)rule.RulesCollection.Rules.Single();
                Assert.Equal(validator, validNullableRule.Validator);

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"], new[] {"Required", "error", "error2"});
            }

            [Theory]
            [InlineData(true, ValidationStrategy.Complete)]
            [InlineData(false, ValidationStrategy.Complete)]
            [InlineData(true, ValidationStrategy.FailFast)]
            [InlineData(false, ValidationStrategy.FailFast)]
            public void Should_Add_ValidModelRule_And_NotExecuteIfNull(bool useRepository, ValidationStrategy validationStrategy)
            {
                var validCounter = 0;

                var model = new User {Address = null};

                var (validator, repository) = GetValidatorObjects<Address>(useRepository, n => n.Valid(m =>
                {
                    Assert.Same(model.Address, m);
                    validCounter++;

                    return false;
                }, "error"));

                var specification = new Specification<User>(model, repository, validationStrategy, 0, new RulesOptionsStub());

                specification.For(c => c.Address, r => r.ValidModel(validator));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "Address", typeof(Address));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidModelRule<Address>>(rule.RulesCollection.Rules.Single());

                var validNullableRule = (ValidModelRule<Address>)rule.RulesCollection.Rules.Single();
                Assert.Equal(validator, validNullableRule.Validator);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"], new[] {"Required"});
            }

            [Theory]
            [InlineData(true, ValidationStrategy.Force)]
            [InlineData(false, ValidationStrategy.Force)]
            public void Should_Add_ValidModelRule_And_ExecuteWithForceIfNull_And_ForceStrategy(bool useRepository, ValidationStrategy validationStrategy)
            {
                var validCounter = 0;
                var validCounter2 = 0;

                var model = new User {Address = null};

                var (validator, repository) = GetValidatorObjects<Address>(useRepository, n => n
                    .Valid(m =>
                    {
                        Assert.Same(model.Address, m);
                        validCounter++;

                        return true;
                    }, "error")
                    .Valid(m =>
                    {
                        Assert.Same(model.Address, m);
                        validCounter2++;

                        return false;
                    }, "error2"));

                var specification = new Specification<User>(model, repository, validationStrategy, 0, new RulesOptionsStub());

                specification.For(c => c.Address, r => r.ValidModel(validator));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "Address", typeof(Address));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidModelRule<Address>>(rule.RulesCollection.Rules.Single());

                var validNullableRule = (ValidModelRule<Address>)rule.RulesCollection.Rules.Single();
                Assert.Equal(validator, validNullableRule.Validator);

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"], new[] {"Required", "error", "error2"});
            }

            [Theory]
            [InlineData(true, ValidationStrategy.Complete)]
            [InlineData(false, ValidationStrategy.Complete)]
            [InlineData(true, ValidationStrategy.FailFast)]
            [InlineData(false, ValidationStrategy.FailFast)]
            [InlineData(true, ValidationStrategy.Force)]
            [InlineData(false, ValidationStrategy.Force)]
            public void Should_Add_ValidModelRule_And_PassOptions(bool useRepository, ValidationStrategy validationStrategy)
            {
                var validCounter = 0;

                var model = new User {Address = new Address {Opinions = new[] {null, ""}}};

                var (validator, repository) = GetValidatorObjects<Address>(useRepository, n => n.For(x => x.Opinions,
                    x => x.ValidCollection<Address, IEnumerable<string>, string>(
                        m => m.Valid(z =>
                        {
                            validCounter++;

                            return false;
                        }, "error"))));

                var specification = new Specification<User>(model, repository, validationStrategy, 0, new RulesOptionsStub {RequiredError = new Error("required!"), CollectionForceKey = "[]"});

                specification.For(c => c.Address, r => r.ValidModel(validator));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "Address", typeof(Address));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidModelRule<Address>>(rule.RulesCollection.Rules.Single());

                var validNullableRule = (ValidModelRule<Address>)rule.RulesCollection.Rules.Single();
                Assert.Equal(validator, validNullableRule.Validator);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                if (validationStrategy == ValidationStrategy.Complete)
                {
                    Assert.Equal(1, validCounter);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"], new string[] { });

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["Address"], new[] {"Opinions"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"].Members["Opinions"], new string[] { });

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["Address"].Members["Opinions"], new[] {"0", "1"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"].Members["Opinions"].Members["0"], new[] {"required!"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"].Members["Opinions"].Members["1"], new[] {"error"});
                }
                else if (validationStrategy == ValidationStrategy.FailFast)
                {
                    Assert.Equal(0, validCounter);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"], new string[] { });

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["Address"], new[] {"Opinions"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"].Members["Opinions"], new string[] { });

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["Address"].Members["Opinions"], new[] {"0"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"].Members["Opinions"].Members["0"], new[] {"required!"});
                }
                else if (validationStrategy == ValidationStrategy.Force)
                {
                    Assert.Equal(0, validCounter);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Address"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"], new[] {"required!"});

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["Address"], new[] {"Opinions"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"].Members["Opinions"], new[] {"required!"});

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["Address"].Members["Opinions"], new[] {"[]"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Address"].Members["Opinions"].Members["[]"], new[] {"required!", "error"});
                }
            }
        }

        public class AddingValidCollectionRule
        {
            [Theory]
            [InlineData(true, ValidationStrategy.Complete)]
            [InlineData(false, ValidationStrategy.Complete)]
            [InlineData(true, ValidationStrategy.FailFast)]
            [InlineData(false, ValidationStrategy.FailFast)]
            [InlineData(true, ValidationStrategy.Force)]
            [InlineData(false, ValidationStrategy.Force)]
            public void Should_Add_ValidCollectionRule_And_Validator(bool useRepository, ValidationStrategy validationStrategy)
            {
                var validCounter = 0;

                var model = new User {PastAddresses = new[] {new Address(), new Address()}};

                var (validator, repository) = GetValidatorObjects<Address>(useRepository, n => n.Valid(m =>
                {
                    Assert.Same(model.PastAddresses.ElementAt(validCounter), m);
                    validCounter++;

                    return false;
                }, "error"));

                var specification = new Specification<User>(model, repository, validationStrategy, 0, new RulesOptionsStub());

                specification.For(c => c.PastAddresses, r => r.ValidCollection<User, IEnumerable<Address>, Address>(c => c.ValidModel(validator)));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "PastAddresses", typeof(IEnumerable<Address>));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidCollectionRule<User, Address>>(rule.RulesCollection.Rules.Single());

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                if (validationStrategy == ValidationStrategy.Complete)
                {
                    Assert.Equal(model.PastAddresses.Count(), validCounter);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new string[] { });

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"0", "1"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["0"], new[] {"error"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["1"], new[] {"error"});
                }

                else if (validationStrategy == ValidationStrategy.FailFast)
                {
                    Assert.Equal(1, validCounter);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new string[] { });

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"0"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["0"], new[] {"error"});
                }

                else if (validationStrategy == ValidationStrategy.Force)
                {
                    Assert.Equal(0, validCounter);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new[] {"Required"});

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"*"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["*"], new[] {"Required", "error"});
                }
            }

            [Theory]
            [InlineData(true, ValidationStrategy.Complete)]
            [InlineData(false, ValidationStrategy.Complete)]
            [InlineData(true, ValidationStrategy.FailFast)]
            [InlineData(false, ValidationStrategy.FailFast)]
            public void Should_Add_ValidCollectionRule_And_Validator_And_NotExecuteIfNull(bool useRepository, ValidationStrategy validationStrategy)
            {
                var validCounter = 0;

                var model = new User {PastAddresses = null};

                var (validator, repository) = GetValidatorObjects<Address>(useRepository, n => n.Valid(m =>
                {
                    Assert.Same(model.PastAddresses.ElementAt(validCounter), m);
                    validCounter++;

                    return false;
                }, "error"));

                var specification = new Specification<User>(model, repository, validationStrategy, 0, new RulesOptionsStub());

                specification.For(c => c.PastAddresses, r => r.ValidCollection<User, IEnumerable<Address>, Address>(c => c.ValidModel(validator)));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "PastAddresses", typeof(IEnumerable<Address>));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidCollectionRule<User, Address>>(rule.RulesCollection.Rules.Single());

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new[] {"Required"});
            }

            [Theory]
            [InlineData(true, ValidationStrategy.Force)]
            [InlineData(false, ValidationStrategy.Force)]
            public void Should_Add_ValidCollectionRule_And_Validator_And_ExecuteWithForceIfNull_And_ForceStrategy(bool useRepository, ValidationStrategy validationStrategy)
            {
                var validCounter = 0;

                var model = new User {PastAddresses = null};

                var (validator, repository) = GetValidatorObjects<Address>(useRepository, n => n.Valid(m =>
                {
                    Assert.Same(model.PastAddresses.ElementAt(validCounter), m);
                    validCounter++;

                    return true;
                }, "error"));

                var specification = new Specification<User>(model, repository, validationStrategy, 0, new RulesOptionsStub());

                specification.For(c => c.PastAddresses, r => r.ValidCollection<User, IEnumerable<Address>, Address>(c => c.ValidModel(validator)));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "PastAddresses", typeof(IEnumerable<Address>));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidCollectionRule<User, Address>>(rule.RulesCollection.Rules.Single());

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new[] {"Required"});

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"*"});
                ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["*"], new[] {"Required", "error"});
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_Add_ValidCollectionRule(ValidationStrategy validationStrategy)
            {
                var validCounter = 0;

                var model = new User {PastAddresses = new[] {new Address(), new Address()}};

                var specification = new Specification<User>(model, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                specification.For(c => c.PastAddresses, r => r.ValidCollection<User, IEnumerable<Address>, Address>(c => c.Valid(x =>
                {
                    Assert.Same(x, model.PastAddresses.ElementAt(validCounter));
                    validCounter++;

                    return false;
                }, "error")));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "PastAddresses", typeof(IEnumerable<Address>));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidCollectionRule<User, Address>>(rule.RulesCollection.Rules.Single());

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                if (validationStrategy == ValidationStrategy.Complete)
                {
                    Assert.Equal(model.PastAddresses.Count(), validCounter);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new string[] { });

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"0", "1"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["0"], new[] {"error"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["1"], new[] {"error"});
                }

                else if (validationStrategy == ValidationStrategy.FailFast)
                {
                    Assert.Equal(1, validCounter);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new string[] { });

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"0"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["0"], new[] {"error"});
                }

                else if (validationStrategy == ValidationStrategy.Force)
                {
                    Assert.Equal(0, validCounter);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new[] {"Required"});

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"*"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["*"], new[] {"Required", "error"});
                }
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_Add_ValidCollectionRule_And_PassStrategy(ValidationStrategy validationStrategy)
            {
                var validCounter = 0;
                var validCounter2 = 0;

                var model = new User {PastAddresses = new[] {new Address(), new Address(), null}};

                var specification = new Specification<User>(model, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                specification.For(c => c.PastAddresses, r => r.ValidCollection<User, IEnumerable<Address>, Address>(c => c
                    .Valid(x =>
                    {
                        Assert.Same(x, model.PastAddresses.Where(i => i != null).ElementAt(validCounter));
                        validCounter++;

                        return true;
                    }, "error")
                    .Valid(x =>
                    {
                        Assert.Same(x, model.PastAddresses.Where(i => i != null).ElementAt(validCounter2));
                        validCounter2++;

                        return false;
                    }, "error2"))
                );

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "PastAddresses", typeof(IEnumerable<Address>));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidCollectionRule<User, Address>>(rule.RulesCollection.Rules.Single());

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                if (validationStrategy == ValidationStrategy.Complete)
                {
                    Assert.Equal(2, validCounter);
                    Assert.Equal(2, validCounter2);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new string[] { });

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"0", "1", "2"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["0"], new[] {"error2"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["1"], new[] {"error2"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["2"], new[] {"Required"});
                }

                else if (validationStrategy == ValidationStrategy.FailFast)
                {
                    Assert.Equal(1, validCounter);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new string[] { });

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"0"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["0"], new[] {"error2"});
                }

                else if (validationStrategy == ValidationStrategy.Force)
                {
                    Assert.Equal(0, validCounter);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new[] {"Required"});

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"*"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["*"], new[] {"Required", "error", "error2"});
                }
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_Add_ValidCollectionRule_And_PassOptions(ValidationStrategy validationStrategy)
            {
                var validCounter = 0;
                var validCounter2 = 0;

                var model = new User {PastAddresses = new[] {null, new Address(), new Address()}};

                var specification = new Specification<User>(model, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub {RequiredError = new Error("required!"), CollectionForceKey = "[]"});

                specification.For(c => c.PastAddresses, r => r.ValidCollection<User, IEnumerable<Address>, Address>(c => c
                    .Valid(x =>
                    {
                        Assert.Same(x, model.PastAddresses.Where(i => i != null).ElementAt(validCounter));
                        validCounter++;

                        return true;
                    }, "error")
                    .Valid(x =>
                    {
                        Assert.Same(x, model.PastAddresses.Where(i => i != null).ElementAt(validCounter2));
                        validCounter2++;

                        return false;
                    }, "error2"))
                );

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "PastAddresses", typeof(IEnumerable<Address>));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidCollectionRule<User, Address>>(rule.RulesCollection.Rules.Single());

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                if (validationStrategy == ValidationStrategy.Complete)
                {
                    Assert.Equal(2, validCounter);
                    Assert.Equal(2, validCounter2);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new string[] { });

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"0", "1", "2"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["0"], new[] {"required!"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["1"], new[] {"error2"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["2"], new[] {"error2"});
                }

                else if (validationStrategy == ValidationStrategy.FailFast)
                {
                    Assert.Equal(0, validCounter);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new string[] { });

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"0"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["0"], new[] {"required!"});
                }

                else if (validationStrategy == ValidationStrategy.Force)
                {
                    Assert.Equal(0, validCounter);

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"PastAddresses"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"], new[] {"required!"});

                    ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection.Members["PastAddresses"], new[] {"[]"});
                    ErrorsCollectionTestsHelpers.ExpectErrors(errorsCollection.Members["PastAddresses"].Members["[]"], new[] {"required!", "error", "error2"});
                }
            }
        }

        public class SingleRule
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_AddMemberRule_WithChangedName_When_SingleFor(ValidationStrategy validationStrategy)
            {
                var specification = new Specification<User>(new User {Email = ""}, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification.For(c => c.Email, r => r.WithName("customName")
                    .Valid(m =>
                    {
                        validCounter++;

                        return false;
                    }, "error"));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                Assert.Equal("Email", rule.MemberPropertyInfo.Name);
                Assert.Equal(typeof(string), rule.MemberPropertyInfo.PropertyType);

                Assert.Equal("customName", rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.Equal("error", ((ValidRule<string>)rule.RulesCollection.Rules.Single()).Message);
                Assert.Null(((ValidRule<string>)rule.RulesCollection.Rules.Single()).Arguments);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(validationStrategy == ValidationStrategy.Force ? 0 : 1, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"customName"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["customName"], validationStrategy == ValidationStrategy.Force ? new[] {"Required", "error"} : new[] {"error"});
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_AddMemberRule_When_SingleFor(ValidationStrategy validationStrategy)
            {
                var specification = new Specification<User>(new User {Email = ""}, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification.For(c => c.Email, r => r.Valid(m =>
                {
                    validCounter++;

                    return false;
                }, "error"));

                Assert.Null(specification.SummaryError);
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.Single();

                AssertCompilableRule(rule, "Email", typeof(string));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidRule<string>>(rule.RulesCollection.Rules.Single());

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(validationStrategy == ValidationStrategy.Force ? 0 : 1, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Email"], validationStrategy == ValidationStrategy.Force ? new[] {"Required", "error"} : new[] {"error"});
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_AddMemberRule_When_ForSelf(ValidationStrategy validationStrategy)
            {
                var specification = new Specification<User>(new User {Email = "", FirstLogin = DateTime.MinValue}, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification
                    .Valid(c =>
                    {
                        validCounter++;

                        return false;
                    }, "error");

                Assert.Null(specification.SummaryError);
                Assert.Equal(1, specification.CompilableRules.Count);

                var rule = specification.CompilableRules.ElementAt(0);

                AssertCompilableRule(rule, null, null);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(validationStrategy == ValidationStrategy.Force ? 0 : 1, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new string[] { });
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors(), new[] {"error"});
            }
        }

        public class MultipleRules
        {
            [Fact]
            public void Should_AddMemberRules_When_ForManyMembers()
            {
                var specification = new Specification<User>(new User {Email = "", FirstLogin = DateTime.MinValue}, new VoidValidatorsRepository(), ValidationStrategy.Complete, 0, new RulesOptionsStub());

                var validCounter = 0;
                var validCounter2 = 0;

                specification
                    .For(c => c.Email, r => r.Valid(m =>
                    {
                        validCounter++;

                        return false;
                    }, "error"))
                    .For(c => c.FirstLogin, r => r.Valid(m =>
                    {
                        validCounter2++;

                        return false;
                    }, "error2"));

                Assert.Null(specification.SummaryError);
                Assert.Equal(2, specification.CompilableRules.Count);

                var rule = specification.CompilableRules.ElementAt(0);

                AssertCompilableRule(rule, "Email", typeof(string));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidRule<string>>(rule.RulesCollection.Rules.Single());

                var rule2 = specification.CompilableRules.ElementAt(1);

                AssertCompilableRule(rule2, "FirstLogin", typeof(DateTime?));
                Assert.Null(rule2.RulesCollection.Name);
                Assert.Null(rule2.RulesCollection.SummaryError);
                Assert.Null(rule2.RulesCollection.RequiredError);
                Assert.Equal(1, rule2.RulesCollection.Rules.Count);
                Assert.IsType<ValidRule<DateTime?>>(rule2.RulesCollection.Rules.Single());

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(1, validCounter);
                Assert.Equal(1, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email", "FirstLogin"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Email"], new[] {"error"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["FirstLogin"], new[] {"error2"});
            }

            [Fact]
            public void Should_AddMemberRules_When_ForManyMembers_WithChangedName()
            {
                var specification = new Specification<User>(new User {Email = "", FirstLogin = DateTime.MinValue}, new VoidValidatorsRepository(), ValidationStrategy.Complete, 0, new RulesOptionsStub());

                var validCounter = 0;
                var validCounter2 = 0;

                specification
                    .For(c => c.Email, r => r.Valid(m =>
                    {
                        validCounter++;

                        return false;
                    }, "error"))
                    .For(c => c.FirstLogin, r => r.WithName("Email")
                        .Valid(m =>
                        {
                            validCounter2++;

                            return false;
                        }, "error2"));

                Assert.Null(specification.SummaryError);
                Assert.Equal(2, specification.CompilableRules.Count);

                var rule = specification.CompilableRules.ElementAt(0);

                AssertCompilableRule(rule, "Email", typeof(string));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidRule<string>>(rule.RulesCollection.Rules.Single());

                var rule2 = specification.CompilableRules.ElementAt(1);

                AssertCompilableRule(rule2, "FirstLogin", typeof(DateTime?));
                Assert.Equal("Email", rule2.RulesCollection.Name);
                Assert.Null(rule2.RulesCollection.SummaryError);
                Assert.Null(rule2.RulesCollection.RequiredError);
                Assert.Equal(1, rule2.RulesCollection.Rules.Count);
                Assert.IsType<ValidRule<DateTime?>>(rule2.RulesCollection.Rules.Single());

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(1, validCounter);
                Assert.Equal(1, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Email"], new[] {"error", "error2"});
            }

            [Fact]
            public void Should_AddMemberRules_When_ForMemberAndSelf()
            {
                var specification = new Specification<User>(new User {Email = "", FirstLogin = DateTime.MinValue}, new VoidValidatorsRepository(), ValidationStrategy.Complete, 0, new RulesOptionsStub());

                var validCounter = 0;
                var validCounter2 = 0;

                specification
                    .For(c => c.Email, r => r.Valid(m =>
                    {
                        validCounter++;

                        return false;
                    }, "error"))
                    .Valid(c =>
                    {
                        validCounter2++;

                        return false;
                    }, "error2");

                Assert.Null(specification.SummaryError);
                Assert.Equal(2, specification.CompilableRules.Count);

                var rule = specification.CompilableRules.ElementAt(0);

                AssertCompilableRule(rule, "Email", typeof(string));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidRule<string>>(rule.RulesCollection.Rules.Single());

                var rule2 = specification.CompilableRules.ElementAt(1);

                AssertCompilableRule(rule2, null, null);

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(1, validCounter);
                Assert.Equal(1, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Email"], new[] {"error"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors(), new[] {"error2"});
            }

            [Fact]
            public void Should_AddMemberRules_When_ForSameMember()
            {
                var specification = new Specification<User>(new User {Email = ""}, new VoidValidatorsRepository(), ValidationStrategy.Complete, 0, new RulesOptionsStub());

                var validCounter = 0;
                var validCounter2 = 0;

                specification
                    .For(c => c.Email, r => r.Valid(m =>
                    {
                        validCounter++;

                        return false;
                    }, "error"))
                    .For(c => c.Email, r => r.Valid(m =>
                    {
                        validCounter2++;

                        return false;
                    }, "error2"));

                Assert.Null(specification.SummaryError);
                Assert.Equal(2, specification.CompilableRules.Count);

                var rule = specification.CompilableRules.ElementAt(0);

                AssertCompilableRule(rule, "Email", typeof(string));
                Assert.Null(rule.RulesCollection.Name);
                Assert.Null(rule.RulesCollection.SummaryError);
                Assert.Null(rule.RulesCollection.RequiredError);
                Assert.Equal(1, rule.RulesCollection.Rules.Count);
                Assert.IsType<ValidRule<string>>(rule.RulesCollection.Rules.Single());

                var rule2 = specification.CompilableRules.ElementAt(1);

                AssertCompilableRule(rule2, "Email", typeof(string));
                Assert.Null(rule2.RulesCollection.Name);
                Assert.Null(rule2.RulesCollection.SummaryError);
                Assert.Null(rule2.RulesCollection.RequiredError);
                Assert.Equal(1, rule2.RulesCollection.Rules.Count);
                Assert.IsType<ValidRule<string>>(rule2.RulesCollection.Rules.Single());

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(1, validCounter);
                Assert.Equal(1, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new[] {"Email"});
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors().Members["Email"], new[] {"error", "error2"});
            }

            [Fact]
            public void Should_AddMemberRules_When_ForSelf()
            {
                var specification = new Specification<User>(new User {Email = "", FirstLogin = DateTime.MinValue}, new VoidValidatorsRepository(), ValidationStrategy.Complete, 0, new RulesOptionsStub());

                var validCounter = 0;
                var validCounter2 = 0;

                specification
                    .Valid(c =>
                    {
                        validCounter++;

                        return false;
                    }, "error")
                    .Valid(c =>
                    {
                        validCounter2++;

                        return false;
                    }, "error2");

                Assert.Null(specification.SummaryError);
                Assert.Equal(2, specification.CompilableRules.Count);

                var rule = specification.CompilableRules.ElementAt(0);

                AssertCompilableRule(rule, null, null);

                var rule2 = specification.CompilableRules.ElementAt(1);

                AssertCompilableRule(rule2, null, null);

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(1, validCounter);
                Assert.Equal(1, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new string[] { });
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors(), new[] {"error", "error2"});
            }
        }

        public class WithSummaryError
        {
            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_NotAddAnyError_When_For_And_Valid(ValidationStrategy validationStrategy)
            {
                var specification = new Specification<User>(new User {Email = ""}, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification
                    .For(c => c.Email, r => r.Valid(m =>
                    {
                        validCounter++;

                        return true;
                    }, "error"))
                    .WithSummaryError("summary error");

                AssertError(specification.SummaryError, "summary error", "summary error");
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.ElementAt(0);

                AssertCompilableRule(rule, "Email", typeof(string));

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(1, validCounter);

                Assert.True(errorsCollection.IsEmpty);
                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new string[] { });
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors(), new string[] { });
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_AddOnlySummaryError_When_Null(ValidationStrategy validationStrategy)
            {
                var specification = new Specification<User>(new User(), new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification
                    .For(c => c.Email, r => r.Valid(m =>
                    {
                        validCounter++;

                        return true;
                    }, "error"))
                    .WithSummaryError("summary error");

                AssertError(specification.SummaryError, "summary error", "summary error");
                Assert.Single(specification.CompilableRules);

                var rule = specification.CompilableRules.ElementAt(0);

                AssertCompilableRule(rule, "Email", typeof(string));

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new string[] { });
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors(), new[] {"summary error"});
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_AddOnlySummaryError_When_For_And_Force(bool emailValid)
            {
                var specification = new Specification<User>(new User {Email = ""}, new VoidValidatorsRepository(), ValidationStrategy.Force, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification
                    .For(c => c.Email, r => r.Valid(m =>
                    {
                        validCounter++;

                        return emailValid;
                    }, "error"))
                    .WithSummaryError("summary error");

                AssertError(specification.SummaryError, "summary error", "summary error");

                Assert.Single(specification.CompilableRules);
                AssertCompilableRule(specification.CompilableRules.ElementAt(0), "Email", typeof(string));

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new string[] { });
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors(), new[] {"summary error"});
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_AddOnlySummaryError_When_SingleFor(ValidationStrategy validationStrategy)
            {
                var specification = new Specification<User>(new User {Email = ""}, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification
                    .For(c => c.Email, r => r.Valid(m =>
                    {
                        validCounter++;

                        return false;
                    }, "error"))
                    .WithSummaryError("summary error");

                AssertError(specification.SummaryError, "summary error", "summary error");

                Assert.Single(specification.CompilableRules);
                AssertCompilableRule(specification.CompilableRules.ElementAt(0), "Email", typeof(string));

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(1, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new string[] { });
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors(), new[] {"summary error"});
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_AddOnlySummaryError_When_SingleSelf(ValidationStrategy validationStrategy)
            {
                var specification = new Specification<User>(new User {Email = ""}, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification
                    .Valid(c =>
                    {
                        validCounter++;

                        return false;
                    }, "error")
                    .WithSummaryError("summary error");

                AssertError(specification.SummaryError, "summary error", "summary error");

                Assert.Single(specification.CompilableRules);
                AssertCompilableRule(specification.CompilableRules.ElementAt(0), null, null);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(1, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new string[] { });
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors(), new[] {"summary error"});
            }

            [Theory]
            [InlineData(true)]
            [InlineData(false)]
            public void Should_AddOnlySummaryError_When_Self_And_Force(bool valid)
            {
                var specification = new Specification<User>(new User {Email = ""}, new VoidValidatorsRepository(), ValidationStrategy.Force, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification
                    .Valid(c =>
                    {
                        validCounter++;

                        return valid;
                    }, "error")
                    .WithSummaryError("summary error");

                AssertError(specification.SummaryError, "summary error", "summary error");

                Assert.Single(specification.CompilableRules);
                AssertCompilableRule(specification.CompilableRules.ElementAt(0), null, null);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new string[] { });
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors(), new[] {"summary error"});
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            public void Should_NotAddAnyError_When_SingleSelf_And_Valid(ValidationStrategy validationStrategy)
            {
                var specification = new Specification<User>(new User {Email = ""}, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification
                    .Valid(c =>
                    {
                        validCounter++;

                        return true;
                    }, "error")
                    .WithSummaryError("summary error");

                AssertError(specification.SummaryError, "summary error", "summary error");

                Assert.Single(specification.CompilableRules);
                AssertCompilableRule(specification.CompilableRules.ElementAt(0), null, null);

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(1, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new string[] { });
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors(), new string[] { });
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete, false, false)]
            [InlineData(ValidationStrategy.Complete, true, false)]
            [InlineData(ValidationStrategy.Complete, false, true)]
            [InlineData(ValidationStrategy.FailFast, false, false)]
            [InlineData(ValidationStrategy.FailFast, true, false)]
            [InlineData(ValidationStrategy.FailFast, false, true)]
            public void Should_AddOnlySummaryError_When_ManyErrors(ValidationStrategy validationStrategy, bool emailValid, bool stringValid)
            {
                var specification = new Specification<User>(new User {Email = ""}, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;
                var validCounter2 = 0;

                specification
                    .For(c => c.Email, r => r.Valid(m =>
                    {
                        validCounter++;

                        return emailValid;
                    }, "error"))
                    .For(c => c.FirstLogin, r => r.Valid(m =>
                    {
                        validCounter2++;

                        return stringValid;
                    }, "error2"))
                    .WithSummaryError("summary error");

                AssertError(specification.SummaryError, "summary error", "summary error");

                Assert.Equal(2, specification.CompilableRules.Count);
                AssertCompilableRule(specification.CompilableRules.ElementAt(0), "Email", typeof(string));
                AssertCompilableRule(specification.CompilableRules.ElementAt(1), "FirstLogin", typeof(DateTime?));

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(1, validCounter);
                Assert.Equal(0, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new string[] { });
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors(), new[] {"summary error"});
            }

            [Theory]
            [InlineData(true, true)]
            [InlineData(true, false)]
            [InlineData(false, true)]
            [InlineData(false, false)]
            public void Should_AddOnlySummaryError_When_ManyErrors_And_Force(bool emailValid, bool firstLoginValid)
            {
                var specification = new Specification<User>(new User {Email = ""}, new VoidValidatorsRepository(), ValidationStrategy.Force, 0, new RulesOptionsStub());

                var validCounter = 0;
                var validCounter2 = 0;

                specification
                    .For(c => c.Email, r => r.Valid(m =>
                    {
                        validCounter++;

                        return emailValid;
                    }, "error"))
                    .For(c => c.FirstLogin, r => r.Valid(m =>
                    {
                        validCounter2++;

                        return firstLoginValid;
                    }, "error2"))
                    .WithSummaryError("summary error");

                AssertError(specification.SummaryError, "summary error", "summary error");

                Assert.Equal(2, specification.CompilableRules.Count);
                AssertCompilableRule(specification.CompilableRules.ElementAt(0), "Email", typeof(string));
                AssertCompilableRule(specification.CompilableRules.ElementAt(1), "FirstLogin", typeof(DateTime?));

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(0, validCounter);
                Assert.Equal(0, validCounter2);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new string[] { });
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors(), new[] {"summary error"});
            }

            [Theory]
            [InlineData(ValidationStrategy.Complete)]
            [InlineData(ValidationStrategy.FailFast)]
            [InlineData(ValidationStrategy.Force)]
            public void Should_AddOnlySummaryError_WithArgs(ValidationStrategy validationStrategy)
            {
                var specification = new Specification<User>(new User {Email = ""}, new VoidValidatorsRepository(), validationStrategy, 0, new RulesOptionsStub());

                var validCounter = 0;

                specification
                    .For(c => c.Email, r => r.Valid(m =>
                    {
                        validCounter++;

                        return false;
                    }, "error"))
                    .WithSummaryError("summary error {arg1} {arg2|format=0000.00}", new IMessageArg[] {new MessageArg("arg1", "value1"), new NumberArg("arg2", 123)});

                AssertError(specification.SummaryError, "summary error {arg1} {arg2|format=0000.00}", "summary error value1 0123.00", new[] {"arg1", "arg2"});

                Assert.Equal(1, specification.CompilableRules.Count);
                AssertCompilableRule(specification.CompilableRules.ElementAt(0), "Email", typeof(string));

                Assert.Equal(0, validCounter);

                var errorsCollection = specification.GetErrors();

                Assert.Equal(validationStrategy == ValidationStrategy.Force ? 0 : 1, validCounter);

                ErrorsCollectionTestsHelpers.ExpectMembers(errorsCollection, new string[] { });
                ErrorsCollectionTestsHelpers.ExpectErrors(specification.GetErrors(), new[] {"summary error value1 0123.00"});
            }
        }

        public class ExceedingMaxDepth
        {
            [Theory]
            [InlineData(0, true)]
            [InlineData(1, true)]
            [InlineData(2, true)]
            [InlineData(3, false)]
            [InlineData(4, false)]
            public void Should_AllowOnlyForMaxDepth_UsingDefinedValidator(int maxDepth, bool expectException)
            {
                var specification = new Specification<Item0>(new Item0(), new VoidValidatorsRepository(), ValidationStrategy.Complete, 0, new RulesOptionsStub {MaxDepth = maxDepth});

                var item3Validator = new Validator<Item3>(m => m.Valid(e => true, "error"));
                var item2Validator = new Validator<Item2>(m => m.For(i => i.Item3, be => be.ValidModel(item3Validator)));
                var item1Validator = new Validator<Item1>(m => m.For(i => i.Item2, be => be.ValidModel(item2Validator)));

                specification
                    .For(c => c.Item1, be => be.ValidModel(item1Validator));

                if (expectException)
                {
                    Assert.Throws<MaxDepthExceededException>(() => { specification.GetErrors(); });
                }
                else
                {
                    specification.GetErrors();
                }
            }

            [Theory]
            [InlineData(0, true)]
            [InlineData(1, true)]
            [InlineData(2, true)]
            [InlineData(3, false)]
            [InlineData(4, false)]
            public void Should_AllowOnlyForMaxDepth_UsingRepositoryValidator(int maxDepth, bool expectException)
            {
                var item3Validator = new Validator<Item3>(m => m.Valid(e => true, "error"));
                var item2Validator = new Validator<Item2>(m => m.For(i => i.Item3, be => be.ValidModel()));
                var item1Validator = new Validator<Item1>(m => m.For(i => i.Item2, be => be.ValidModel()));

                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock.Setup(be => be.Get<Item1>()).Returns(item1Validator);
                validatorsRepositoryMock.Setup(be => be.Get<Item2>()).Returns(item2Validator);
                validatorsRepositoryMock.Setup(be => be.Get<Item3>()).Returns(item3Validator);

                var specification = new Specification<Item0>(new Item0(), validatorsRepositoryMock.Object, ValidationStrategy.Complete, 0, new RulesOptionsStub {MaxDepth = maxDepth});

                specification.For(c => c.Item1, be => be.ValidModel());

                if (expectException)
                {
                    Assert.Throws<MaxDepthExceededException>(() => { specification.GetErrors(); });
                }
                else
                {
                    specification.GetErrors();
                }
            }

            [Theory]
            [InlineData(0)]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(5)]
            [InlineData(15)]
            public void Should_AllowOnlyForMaxDepth_When_Loop(int maxDepth)
            {
                var validatorsRepositoryMock = new Mock<IValidatorsRepository>();

                validatorsRepositoryMock
                    .Setup(r => r.Get<ItemLoop>())
                    .Returns(c => c.For(m => m.Nested, m => m.ValidModel()));

                var specification = new Specification<ItemLoop>(new ItemLoop(), validatorsRepositoryMock.Object, ValidationStrategy.Complete, 0, new RulesOptionsStub {MaxDepth = maxDepth});

                specification.For(c => c.Nested, be => be.ValidModel());

                var rule = new ValidModelRule<ItemLoop>();

                Assert.Throws<MaxDepthExceededException>(() =>
                {
                    rule.Compile(new object[]
                    {
                        new ItemLoop(),
                        validatorsRepositoryMock.Object,
                        ValidationStrategy.Complete,
                        0,
                        new RulesOptionsStub
                        {
                            MaxDepth = maxDepth
                        }
                    });
                });
            }

            public class Item0
            {
                public Item1 Item1 { get; } = new Item1();
            }

            public class Item1
            {
                public Item2 Item2 { get; } = new Item2();
            }

            public class Item2
            {
                public Item3 Item3 { get; } = new Item3();
            }

            public class Item3
            {
            }

            public class ItemLoop
            {
                public ItemLoop()
                {
                    Nested = this;
                }

                public ItemLoop Nested { get; }
            }
        }
    }
}