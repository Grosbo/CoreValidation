using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors.Args;
using CoreValidation.Exceptions;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;
using CoreValidation.Validators;
using Xunit;

namespace CoreValidation.UnitTests.Validators
{
    public class ValidatorCreatorTests
    {
        public class SetSingleError
        {
            [Fact]
            public void Should_SetSingleError_BeAdded()
            {
                var validator = ValidatorCreator.Create<MemberClass>(b => b
                    .SetSingleError("message")
                );

                Assert.Equal("message", validator.SingleError.Message);
                Assert.Null(validator.SingleError.Arguments);
            }

            [Fact]
            public void Should_SetSingleError_NotBeAdded_When_NoMethod()
            {
                var validator = ValidatorCreator.Create<MemberClass>(b => b);

                Assert.Null(validator.SingleError);
            }

            [Fact]
            public void Should_ThrowException_When_SingleError_Added_MultipleTimes()
            {
                Assert.Throws<InvalidCommandDuplicationException>(() =>
                {
                    ValidatorCreator.Create<MemberClass>(b => b
                        .SetSingleError("message1")
                        .SetSingleError("message2"));
                });
            }

            [Fact]
            public void Should_ThrowException_When_SingleError_Added_With_NullMessage()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    ValidatorCreator.Create<MemberClass>(b => b
                        .SetSingleError(null));
                });
            }
        }

        public class Valid
        {
            [Fact]
            public void Should_AddModelScope_When_Valid()
            {
                Predicate<MemberClass> isValid = c => true;

                var validator = ValidatorCreator.Create<MemberClass>(b => b
                    .Valid(isValid)
                );

                Assert.IsType<ModelScope<MemberClass>>(validator.Scopes.Single());

                var modelScope = (ModelScope<MemberClass>)validator.Scopes.Single();

                Assert.Same(isValid, modelScope.Rule.IsValid);
                Assert.Null(modelScope.RuleSingleError);
                Assert.Null(modelScope.Rule.Error);
            }

            [Fact]
            public void Should_ThrowException_When_Valid_And_NullPredicate()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    ValidatorCreator.Create<MemberClass>(b => b
                        .Valid(null)
                    );
                });
            }
        }

        public class Member
        {
            [Fact]
            public void Should_AddMemberScope_When_Member()
            {
                Predicate<string> isValid = c => true;
                var args = new IMessageArg[] {new MessageArg("test", "test123")};

                MemberSpecification<MemberClass, string> memberSpecification = c => c.Valid(isValid, "message", args);

                var validator = ValidatorCreator.Create<MemberClass>(b => b
                    .Member(m => m.MemberField, memberSpecification)
                );

                Assert.IsType<MemberScope<MemberClass, string>>(validator.Scopes.Single());

                var memberScope = (MemberScope<MemberClass, string>)validator.Scopes.Single();

                Assert.Null(memberScope.RuleSingleError);

                Assert.Equal("MemberField", memberScope.MemberPropertyInfo.Name);
                Assert.Equal(typeof(string), memberScope.MemberPropertyInfo.PropertyType);

                Assert.IsType<ValidRule<string>>(memberScope.MemberValidator.Rules.Single());

                var memberRule = (ValidRule<string>)memberScope.MemberValidator.Rules.Single();

                Assert.Same(isValid, memberRule.IsValid);
                Assert.Equal("message", memberRule.Error.Message);
                Assert.Same(args, memberRule.Error.Arguments);
                Assert.Null(memberRule.RuleSingleError);
            }

            [Fact]
            public void Should_AddMemberScope_When_Member_WithoutSpecification()
            {
                var validator = ValidatorCreator.Create<MemberClass>(b => b
                    .Member(m => m.MemberField)
                );

                Assert.IsType<MemberScope<MemberClass, string>>(validator.Scopes.Single());

                var memberScope = (MemberScope<MemberClass, string>)validator.Scopes.Single();

                Assert.Null(memberScope.RuleSingleError);

                Assert.Equal("MemberField", memberScope.MemberPropertyInfo.Name);
                Assert.Equal(typeof(string), memberScope.MemberPropertyInfo.PropertyType);

                Assert.False(memberScope.MemberValidator.IsOptional);
                Assert.Null(memberScope.MemberValidator.RequiredError);
                Assert.Empty(memberScope.MemberValidator.Rules);
            }

            [Fact]
            public void Should_ThrowException_When_Member_And_NullMemberSelector()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    ValidatorCreator.Create<MemberClass>(b => b
                        .Member<string>(null, c => c)
                    );
                });
            }
        }

        public class WithMessage
        {
            private void AssertSingleRuleSet<T>(IValidator<T> validator, IReadOnlyCollection<ICommand> commands, int expectedCommands = 2) where T : class
            {
                Assert.Equal(expectedCommands, commands.Count);

                Assert.IsAssignableFrom<ISingleErrorHolder>(commands.ElementAt(0));
                Assert.IsType<WithMessageCommand>(commands.ElementAt(1));

                Assert.Equal(1, validator.Scopes.Count);

                Assert.IsAssignableFrom<ISingleErrorHolder>(validator.Scopes.Single());

                var singleErrorHolder = (ISingleErrorHolder)validator.Scopes.Single();

                Assert.NotNull(singleErrorHolder.RuleSingleError);
                Assert.Equal("message", singleErrorHolder.RuleSingleError.Message);
                Assert.Null(singleErrorHolder.RuleSingleError.Arguments);

                // ReSharper disable once UnusedVariable
                var void1 = commands;
                // ReSharper disable once UnusedVariable
                var void2 = expectedCommands;
            }

            [Fact]
            public void Should_ThrowException_When_WithMessage_AtTheBeginning()
            {
                Assert.Throws<InvalidCommandOrderException>(() =>
                {
                    ValidatorCreator.Create<MemberClass>(b => b
                        .WithMessage("message")
                    );
                });
            }

            [Fact]
            public void Should_WithMessage_ApplyLastMessage_When_MultipleInRow()
            {
                var validator = ValidatorCreator.Create<MemberClass>(b => b
                        .Member(m => m.MemberField)
                        .WithMessage("message3")
                        .WithMessage("message2")
                        .WithMessage("message1")
                        .WithMessage("message"),
                    out var commands
                );

                AssertSingleRuleSet(validator, commands, 5);
            }

            [Fact]
            public void Should_WithMessage_SetRuleSingleError_In_MemberScope()
            {
                var validator = ValidatorCreator.Create<MemberClass>(b => b
                        .Member(m => m.MemberField)
                        .WithMessage("message"),
                    out var commands
                );

                AssertSingleRuleSet(validator, commands);
            }

            [Fact]
            public void Should_WithMessage_SetRuleSingleError_In_ModelScope()
            {
                var validator = ValidatorCreator.Create<MemberClass>(b => b
                        .Valid(x => false)
                        .WithMessage("message"),
                    out var commands
                );

                AssertSingleRuleSet(validator, commands);
            }
        }

        public class MemberClass
        {
            // ReSharper disable once UnassignedGetOnlyAutoProperty
            public string MemberField { get; }
        }

        [Fact]
        public void Should_HaveEmptyInitialValues()
        {
            var validator = ValidatorCreator.Create<MemberClass>(b => b);

            Assert.Null(validator.SingleError);
            Assert.Empty(validator.Scopes);
        }

        [Fact]
        public void Should_OutputCommands()
        {
            ValidatorCreator.Create<MemberClass>(b => b
                    .Valid(m => true)
                    .WithMessage("message")
                    .SetSingleError("single")
                    .Member(m => m.MemberField)
                , out var commands);

            Assert.Equal(4, commands.Count);
            Assert.IsType<ModelScope<MemberClass>>(commands.ElementAt(0));
            Assert.IsType<WithMessageCommand>(commands.ElementAt(1));
            Assert.IsType<SetSingleErrorCommand>(commands.ElementAt(2));
            Assert.IsType<MemberScope<MemberClass, string>>(commands.ElementAt(3));
        }

        [Fact]
        public void Should_ThrowException_When_ReferenceChanged()
        {
            Assert.Throws<InvalidProcessedReferenceException>(() => { ValidatorCreator.Create<MemberClass>(b => { return new SpecificationBuilder<MemberClass>(); }); });
        }
    }
}