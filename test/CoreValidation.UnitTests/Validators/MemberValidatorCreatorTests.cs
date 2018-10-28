using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Errors.Args;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;
using CoreValidation.Validators;
using Xunit;

namespace CoreValidation.UnitTests.Validators
{
    public class MemberValidatorCreatorTests
    {
        public static IEnumerable<object[]> ValidErrorAndArgsCombinations_Data()
        {
            var args = new IMessageArg[] {NumberArg.Create("test1", 1), new TextArg("test2", "two")};

            yield return new object[] {"message", args};
            yield return new object[] {"message", null};
        }

        public class SetSingleError
        {
            [Fact]
            public void Should_SetSingleError_BeAdded()
            {
                var validator = MemberValidatorCreator.Create<object, object>(b => b
                    .SetSingleError("message")
                );

                Assert.Equal("message", validator.SingleError.Message);
                Assert.Null(validator.SingleError.Arguments);
            }

            [Fact]
            public void Should_SetSingleError_NotBeAdded_When_NoMethod()
            {
                var validator = MemberValidatorCreator.Create<object, object>(b => b);

                Assert.Null(validator.SingleError);
            }

            [Fact]
            public void Should_ThrowException_When_SingleError_Added_MultipleTimes()
            {
                Assert.Throws<InvalidCommandDuplicationException>(() =>
                {
                    MemberValidatorCreator.Create<object, object>(b => b
                        .SetSingleError("message1")
                        .SetSingleError("message2"));
                });
            }

            [Fact]
            public void Should_ThrowException_When_SingleError_Added_With_NullMessage()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    MemberValidatorCreator.Create<object, object>(b => b
                        .SetSingleError(null));
                });
            }
        }

        public class SetRequired
        {
            [Fact]
            public void Should_RequiredError_NotBeAdded_When_NoMethod()
            {
                var validator = MemberValidatorCreator.Create<object, object>(b => b);

                Assert.Null(validator.RequiredError);
            }

            [Fact]
            public void Should_SetRequired_BeAdded()
            {
                var validator = MemberValidatorCreator.Create<object, object>(b => b
                    .SetRequired("message")
                );

                Assert.Equal("message", validator.RequiredError.Message);
                Assert.Null(validator.RequiredError.Arguments);
            }

            [Fact]
            public void Should_ThrowException_When_RequiredError_Added_MultipleTimes()
            {
                Assert.Throws<InvalidCommandDuplicationException>(() =>
                {
                    MemberValidatorCreator.Create<object, object>(b => b
                        .SetRequired("message1")
                        .SetRequired("message2")
                    );
                });
            }

            [Fact]
            public void Should_ThrowException_When_RequiredError_Added_With_NullMessage()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    MemberValidatorCreator.Create<object, object>(b => b
                        .SetRequired(null)
                    );
                });
            }
        }

        public class SetOptional
        {
            [Fact]
            public void Should_SetSetOptional()
            {
                var validator = MemberValidatorCreator.Create<object, IEnumerable<MemberClass>>(b => b
                    .SetOptional()
                );

                Assert.True(validator.IsOptional);
            }

            [Fact]
            public void Should_ThrowException_When_DuplicateSetOptional()
            {
                Assert.Throws<InvalidCommandDuplicationException>(() =>
                {
                    MemberValidatorCreator.Create<object, IEnumerable<MemberClass>>(b => b
                        .SetOptional().SetOptional()
                    );
                });
            }
        }

        public class Valid
        {
            [Theory]
            [MemberData(nameof(ValidErrorAndArgsCombinations_Data), MemberType = typeof(MemberValidatorCreatorTests))]
            public void Should_AddValidRule_WithParameters(string message, IMessageArg[] args)
            {
                Predicate<object> isValid = c => true;

                var validator = MemberValidatorCreator.Create<object, object>(b => b
                    .Valid(isValid, message, args)
                );

                Assert.IsType<ValidRule<object>>(validator.Rules.Single());

                var memberRule = (ValidRule<object>)validator.Rules.Single();

                Assert.Same(isValid, memberRule.IsValid);

                if (message != null)
                {
                    Assert.Equal(message, memberRule.Error.Message);
                    Assert.Same(args, memberRule.Error.Arguments);
                }
                else
                {
                    Assert.Null(memberRule.Error);
                }
            }

            [Fact]
            public void Should_AddValidRule()
            {
                Predicate<object> isValid = c => true;

                var validator = MemberValidatorCreator.Create<object, object>(b => b
                    .Valid(isValid)
                );

                Assert.IsType<ValidRule<object>>(validator.Rules.Single());

                var memberRule = (ValidRule<object>)validator.Rules.Single();

                Assert.Same(isValid, memberRule.IsValid);

                Assert.Null(memberRule.Error);
            }

            [Fact]
            public void Should_ThrowException_When_Valid_And_ArgsWithoutMessage()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    MemberValidatorCreator.Create<object, object>(b => b
                        .Valid(c => true, null, new IMessageArg[] {NumberArg.Create("test", 1)})
                    );
                });
            }

            [Fact]
            public void Should_ThrowException_When_Valid_And_NullPredicate()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    MemberValidatorCreator.Create<object, object>(b => b
                        .Valid(null)
                    );
                });
            }

            [Fact]
            public void Should_ThrowException_When_Valid_WithParameters_And_NullPredicate()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    MemberValidatorCreator.Create<object, object>(b => b
                        .Valid(null, "test")
                    );
                });
            }
        }

        public class AsRelative
        {
            [Fact]
            public void Should_AddAsRelativeRule()
            {
                Predicate<object> isValid = c => true;

                var validator = MemberValidatorCreator.Create<object, object>(b => b
                    .AsRelative(isValid)
                );

                Assert.IsType<AsRelativeRule<object>>(validator.Rules.Single());

                var memberRule = (AsRelativeRule<object>)validator.Rules.Single();

                Assert.Same(isValid, memberRule.IsValid);
                Assert.Null(memberRule.Error);
            }


            [Fact]
            public void Should_ThrowException_When_AsRelative_And_NullPredicate()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    MemberValidatorCreator.Create<object, object>(b => b
                        .AsRelative(null)
                    );
                });
            }
        }

        public class AsNullable
        {
            [Fact]
            public void Should_AddAsNullableRule_When_AsNullable()
            {
                MemberSpecification<object, int> memberSpecification = c => c;

                var validator = MemberValidatorCreator.Create<object, int?>(b => b
                    .AsNullable(memberSpecification)
                );

                Assert.IsType<AsNullableRule<object, int>>(validator.Rules.Single());

                var asNullableRule = (AsNullableRule<object, int>)validator.Rules.Single();

                Assert.Same(memberSpecification, asNullableRule.MemberSpecification);
            }

            [Fact]
            public void Should_ThrowException_When_AsNullable_And_NullMemberSpecification()
            {
                Assert.Throws<ArgumentNullException>(() =>
                {
                    MemberValidatorCreator.Create<object, int?>(b => b
                        .AsNullable(null)
                    );
                });
            }
        }

        public class AsModel
        {
            [Fact]
            public void Should_AddAsModelRule_WithoutSpecification_When_AsModel_And_NullSpecification()
            {
                var validator = MemberValidatorCreator.Create<object, MemberClass>(b => b
                    .AsModel()
                );

                Assert.IsType<AsModelRule<MemberClass>>(validator.Rules.Single());

                var asModelRule = (AsModelRule<MemberClass>)validator.Rules.Single();

                Assert.Null(asModelRule.Specification);
            }

            [Fact]
            public void Should_AddAsModelRule_WithSpecification_When_AsModel()
            {
                Specification<MemberClass> specification = c => c;

                var validator = MemberValidatorCreator.Create<object, MemberClass>(b => b
                    .AsModel(specification)
                );

                Assert.IsType<AsModelRule<MemberClass>>(validator.Rules.Single());

                var asModelRule = (AsModelRule<MemberClass>)validator.Rules.Single();

                Assert.Same(specification, asModelRule.Specification);
            }
        }

        public class AsCollection
        {
            public static IEnumerable<object[]> Should_AddAsCollectionRule_When_AsModelsCollection_Data()
            {
                yield return new object[] {new Specification<MemberClass>(c => c)};

                yield return new object[] {null};
            }

            [Theory]
            [MemberData(nameof(Should_AddAsCollectionRule_When_AsModelsCollection_Data))]
            public void Should_AddAsCollectionRule_When_AsModelsCollection(Specification<MemberClass> specification)
            {
                var validator = MemberValidatorCreator.Create<object, IEnumerable<MemberClass>>(b => b
                    .AsModelsCollection<object, IEnumerable<MemberClass>, MemberClass>(specification)
                );

                Assert.IsType<AsCollectionRule<object, MemberClass>>(validator.Rules.Single());

                var validCollectionRule = (AsCollectionRule<object, MemberClass>)validator.Rules.Single();

                Assert.NotNull(validCollectionRule.ItemSpecification);

                var itemValidator = MemberValidatorCreator.Create(validCollectionRule.ItemSpecification);

                Assert.IsType<AsModelRule<MemberClass>>(itemValidator.Rules.Single());
                Assert.False(itemValidator.IsOptional);

                var asModelRule = (AsModelRule<MemberClass>)itemValidator.Rules.Single();

                Assert.Same(specification, asModelRule.Specification);
            }

            [Fact]
            public void Should_AddAsCollectionRule_When_AsCollection()
            {
                MemberSpecification<object, MemberClass> itemSpecification = c => c;

                var validator = MemberValidatorCreator.Create<object, IEnumerable<MemberClass>>(b => b
                    .AsCollection<object, IEnumerable<MemberClass>, MemberClass>(itemSpecification)
                );

                Assert.IsType<AsCollectionRule<object, MemberClass>>(validator.Rules.Single());

                var validCollectionRule = (AsCollectionRule<object, MemberClass>)validator.Rules.Single();

                Assert.Equal(itemSpecification, validCollectionRule.ItemSpecification);
            }
        }

        public class WithMessage
        {
            private void AssertSingleRuleSet(IMemberValidator validator, IReadOnlyCollection<ICommand> commands, int expectedCommandsCount = 2)
            {
                Assert.Equal(expectedCommandsCount, commands.Count);

                Assert.IsAssignableFrom<ISingleErrorHolder>(commands.ElementAt(0));
                Assert.IsType<WithMessageCommand>(commands.ElementAt(1));

                Assert.Equal(1, validator.Rules.Count);

                Assert.IsAssignableFrom<ISingleErrorHolder>(validator.Rules.Single());

                var singleErrorHolder = (ISingleErrorHolder)validator.Rules.Single();

                Assert.NotNull(singleErrorHolder.RuleSingleError);
                Assert.Equal("message", singleErrorHolder.RuleSingleError.Message);
                Assert.Null(singleErrorHolder.RuleSingleError.Arguments);

                // ReSharper disable once UnusedVariable
                var void1 = commands;
                // ReSharper disable once UnusedVariable
                var void2 = expectedCommandsCount;
            }

            [Fact]
            public void Should_ThrowException_When_WithMessage_AtTheBeginning()
            {
                Assert.Throws<InvalidCommandOrderException>(() =>
                {
                    MemberValidatorCreator.Create<object, object>(b => b
                        .WithMessage("message")
                    );
                });
            }

            [Fact]
            public void Should_WithMessage_ApplyLastMessage_When_MultipleInRow()
            {
                var validator = MemberValidatorCreator.Create<object, MemberClass>(b => b
                        .Valid(x => false)
                        .WithMessage("message3")
                        .WithMessage("message2")
                        .WithMessage("message1")
                        .WithMessage("message"),
                    out var commands
                );

                AssertSingleRuleSet(validator, commands, 5);
            }

            [Fact]
            public void Should_WithMessage_SetRuleSingleError_In_AsCollection()
            {
                var validator = MemberValidatorCreator.Create<object, IEnumerable<MemberClass>>(b => b
                        .AsCollection<object, IEnumerable<MemberClass>, MemberClass>(x => x)
                        .WithMessage("message"),
                    out var commands
                );

                AssertSingleRuleSet(validator, commands);
            }

            [Fact]
            public void Should_WithMessage_SetRuleSingleError_In_AsModel()
            {
                var validator = MemberValidatorCreator.Create<object, MemberClass>(b => b
                        .AsModel()
                        .WithMessage("message"),
                    out var commands
                );

                AssertSingleRuleSet(validator, commands);
            }

            [Fact]
            public void Should_WithMessage_SetRuleSingleError_In_AsModelsCollection()
            {
                var validator = MemberValidatorCreator.Create<object, IEnumerable<MemberClass>>(b => b
                        .AsModelsCollection<object, IEnumerable<MemberClass>, MemberClass>(x => x)
                        .WithMessage("message"),
                    out var commands
                );

                AssertSingleRuleSet(validator, commands);
            }

            [Fact]
            public void Should_WithMessage_SetRuleSingleError_In_AsRelative()
            {
                var validator = MemberValidatorCreator.Create<object, MemberClass>(b => b
                        .AsRelative(x => false)
                        .WithMessage("message"),
                    out var commands
                );

                AssertSingleRuleSet(validator, commands);
            }

            [Fact]
            public void Should_WithMessage_SetRuleSingleError_In_Valid()
            {
                var validator = MemberValidatorCreator.Create<object, MemberClass>(b => b
                        .Valid(x => false)
                        .WithMessage("message"),
                    out var commands
                );

                AssertSingleRuleSet(validator, commands);
            }
        }

        public class MemberClass
        {
        }

        [Fact]
        public void Should_HaveEmptyInitialValues()
        {
            var memberValidator = MemberValidatorCreator.Create<object, object>(b => b);

            Assert.False(memberValidator.IsOptional);
            Assert.Null(memberValidator.RequiredError);
            Assert.Null(memberValidator.SingleError);
            Assert.Empty(memberValidator.Rules);
        }

        [Fact]
        public void Should_OutputCommands()
        {
            MemberValidatorCreator.Create<object, MemberClass>(b => b
                    .SetOptional()
                    .SetRequired("required")
                    .Valid(x => false)
                    .WithMessage("message")
                    .AsRelative(x => true)
                    .AsModel()
                    .SetSingleError("single")
                , out var commands);

            Assert.Equal(7, commands.Count);
            Assert.IsType<SetOptionalCommand>(commands.ElementAt(0));
            Assert.IsType<SetRequiredCommand>(commands.ElementAt(1));
            Assert.IsType<ValidRule<MemberClass>>(commands.ElementAt(2));
            Assert.IsType<WithMessageCommand>(commands.ElementAt(3));
            Assert.IsType<AsRelativeRule<object>>(commands.ElementAt(4));
            Assert.IsType<AsModelRule<MemberClass>>(commands.ElementAt(5));
            Assert.IsType<SetSingleErrorCommand>(commands.ElementAt(6));
        }
    }
}