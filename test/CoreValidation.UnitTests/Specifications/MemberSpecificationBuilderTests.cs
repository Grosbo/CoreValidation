using System;
using System.Collections.Generic;
using System.Linq;
using CoreValidation.Specifications;
using CoreValidation.Specifications.Commands;
using Xunit;

namespace CoreValidation.UnitTests.Specifications
{
    public class MemberSpecificationBuilderTests
    {
        public class SetSingleError
        {
            [Fact]
            public void Should_Add_SetSingleErrorCommand_When_SetSingleError()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                builder.SetSingleError("message");

                Assert.Single(builder.Commands);
                Assert.IsType<SetSingleErrorCommand>(builder.Commands.Single());

                var command = (SetSingleErrorCommand)builder.Commands.Single();

                Assert.Equal("message", command.Message);
                Assert.Equal("SetSingleError", command.Name);
            }

            [Fact]
            public void Should_ThrowException_When_SetSingleError_With_NullMessage()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                Assert.Throws<ArgumentNullException>(() => { builder.SetSingleError(null); });
            }
        }

        public class SetOptional
        {
            [Fact]
            public void Should_Add_SetOptionalCommand_When_SetOptional()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                builder.SetOptional();

                Assert.Single(builder.Commands);
                Assert.IsType<SetOptionalCommand>(builder.Commands.Single());

                var command = (SetOptionalCommand)builder.Commands.Single();

                Assert.Equal("SetOptional", command.Name);
            }
        }

        public class SetRequired
        {
            [Fact]
            public void Should_Add_SetRequiredCommand_When_SetRequired()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                builder.SetRequired("message");

                Assert.Single(builder.Commands);
                Assert.IsType<SetRequiredCommand>(builder.Commands.Single());

                var command = (SetRequiredCommand)builder.Commands.Single();

                Assert.Equal("SetRequired", command.Name);
            }


            [Fact]
            public void Should_ThrowException_When_SetRequired_With_NullMessage()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                Assert.Throws<ArgumentNullException>(() => { builder.SetRequired(null); });
            }
        }

        public class WithMessage
        {
            [Fact]
            public void Should_Add_WithMessageCommand_When_WithMessage()
            {
                var builder = new MemberSpecificationBuilder<object, object>();

                builder.WithMessage("message");

                Assert.Single(builder.Commands);
                Assert.IsType<WithMessageCommand>(builder.Commands.Single());

                var command = (WithMessageCommand)builder.Commands.Single();

                Assert.Equal("WithMessage", command.Name);
                Assert.Equal("message", command.Message);
            }

            [Fact]
            public void Should_ThrowException_When_WithMessage_And_NullMessage()
            {
                var builder = new MemberSpecificationBuilder<object, int>();

                Assert.Throws<ArgumentNullException>(() => { builder.WithMessage(null); });
            }
        }

        public class Valid
        {
            [Fact]
            public void Should_Add_ValidRule_When_Valid()
            {
                var builder = new MemberSpecificationBuilder<object, int>();

                Predicate<int> isValid = c => true;

                builder.Valid(isValid);

                Assert.Single(builder.Commands);
                Assert.IsType<ValidRule<int>>(builder.Commands.Single());

                var command = (ValidRule<int>)builder.Commands.Single();

                Assert.Equal("Valid", command.Name);
                Assert.Null(command.RuleSingleError);
                Assert.Null(command.Error);
                Assert.Same(isValid, command.IsValid);
            }

            [Fact]
            public void Should_Add_ValidRule_When_Valid_With_Message()
            {
                var builder = new MemberSpecificationBuilder<object, int>();

                Predicate<int> isValid = c => true;

                builder.Valid(isValid, "message");

                Assert.Single(builder.Commands);
                Assert.IsType<ValidRule<int>>(builder.Commands.Single());

                var command = (ValidRule<int>)builder.Commands.Single();

                Assert.Equal("Valid", command.Name);
                Assert.Null(command.RuleSingleError);
                Assert.NotNull(command.Error);
                Assert.Equal("message", command.Error.Message);
                Assert.Null(command.Error.Arguments);
                Assert.Same(isValid, command.IsValid);
            }

            [Fact]
            public void Should_Add_ValidRule_When_Valid_With_MessageAndArgs()
            {
                var builder = new MemberSpecificationBuilder<object, int>();

                Predicate<int> isValid = c => true;
                var args = new[] {Arg.Text("n", "v")};

                builder.Valid(isValid, "message", args);

                Assert.Single(builder.Commands);
                Assert.IsType<ValidRule<int>>(builder.Commands.Single());

                var command = (ValidRule<int>)builder.Commands.Single();

                Assert.Equal("Valid", command.Name);
                Assert.Null(command.RuleSingleError);
                Assert.NotNull(command.Error);
                Assert.Equal("message", command.Error.Message);
                Assert.Same(args, command.Error.Arguments);
                Assert.Same(isValid, command.IsValid);
            }

            [Fact]
            public void Should_ThrowException_When_Valid_And_ArgsWithoutMessage()
            {
                var builder = new MemberSpecificationBuilder<object, int>();

                Predicate<int> isValid = c => true;
                var args = new[] {Arg.Text("n", "v")};

                Assert.Throws<ArgumentNullException>(() => { builder.Valid(isValid, null, args); });
            }

            [Fact]
            public void Should_ThrowException_When_Valid_And_NullPredicate()
            {
                var builder = new MemberSpecificationBuilder<object, int>();

                Assert.Throws<ArgumentNullException>(() => { builder.Valid(null); });
            }

            [Fact]
            public void Should_ThrowException_When_Valid_WithMessageAndArgs_And_NullPredicate()
            {
                var builder = new MemberSpecificationBuilder<object, int>();

                var args = new[] {Arg.Text("n", "v")};

                Assert.Throws<ArgumentNullException>(() => { builder.Valid(null, "message", args); });
            }
        }

        public class AsRelative
        {
            [Fact]
            public void Should_Add_AsRelativeRule_When_AsRelative()
            {
                var builder = new MemberSpecificationBuilder<object, int>();

                Predicate<object> isValid = c => true;

                builder.AsRelative(isValid);

                Assert.Single(builder.Commands);
                Assert.IsType<AsRelativeRule<object>>(builder.Commands.Single());

                var command = (AsRelativeRule<object>)builder.Commands.Single();

                Assert.Equal("AsRelative", command.Name);
                Assert.Null(command.RuleSingleError);
                Assert.Null(command.Error);
                Assert.Same(isValid, command.IsValid);
            }

            [Fact]
            public void Should_Add_AsRelativeRule_When_AsRelative_And_NullPredicate()
            {
                var builder = new MemberSpecificationBuilder<object, int>();

                Assert.Throws<ArgumentNullException>(() => { builder.AsRelative(null); });
            }
        }

        public class AsModel
        {
            [Fact]
            public void Should_Add_AsModelRule_When_AsModel_And_SpecificationDefined()
            {
                var builder = new MemberSpecificationBuilder<object, MemberClass>();

                var specification = new Specification<MemberClass>(x => x);

                builder.AsModel(specification);

                Assert.Single(builder.Commands);
                Assert.IsType<AsModelRule<MemberClass>>(builder.Commands.Single());

                var command = (AsModelRule<MemberClass>)builder.Commands.Single();

                Assert.Equal("AsModel", command.Name);
                Assert.Null(command.RuleSingleError);
                Assert.Same(specification, command.Specification);
                Assert.Equal(specification.GetHashCode().ToString(), command.SpecificationId);
            }

            [Fact]
            public void Should_Add_AsModelRule_When_AsModel_And_SpecificationFromRepository()
            {
                var builder = new MemberSpecificationBuilder<object, MemberClass>();

                builder.AsModel();

                Assert.Single(builder.Commands);
                Assert.IsType<AsModelRule<MemberClass>>(builder.Commands.Single());

                var command = (AsModelRule<MemberClass>)builder.Commands.Single();

                Assert.Equal("AsModel", command.Name);
                Assert.Null(command.RuleSingleError);
                Assert.Null(command.Specification);
                Assert.Null(command.SpecificationId);
            }
        }

        public class AsNullable
        {
            [Fact]
            public void Should_Add_AsNullableRule_When_AsNullable()
            {
                var builder = new MemberSpecificationBuilder<object, int?>();

                var memberSpecification = new MemberSpecification<object, int>(x => x);

                builder.AsNullable(memberSpecification);

                Assert.Single(builder.Commands);
                Assert.IsType<AsNullableRule<object, int>>(builder.Commands.Single());

                var command = (AsNullableRule<object, int>)builder.Commands.Single();

                Assert.Equal("AsNullable", command.Name);
                Assert.Null(command.RuleSingleError);
                Assert.Same(memberSpecification, command.MemberSpecification);
                Assert.NotNull(command.MemberSpecification);
            }

            [Fact]
            public void Should_Add_AsNullableRule_When_AsNullable_And_AdvancedSpecification()
            {
                var builder = new MemberSpecificationBuilder<object, int?>();

                Predicate<int> innerPredicate = z => true;

                var memberSpecification = new MemberSpecification<object, int>(x => x.Valid(innerPredicate).SetSingleError("single_message"));

                builder.AsNullable(memberSpecification);

                Assert.Single(builder.Commands);
                Assert.IsType<AsNullableRule<object, int>>(builder.Commands.Single());

                var command = (AsNullableRule<object, int>)builder.Commands.Single();

                Assert.Equal("AsNullable", command.Name);
                Assert.Null(command.RuleSingleError);
                Assert.Same(memberSpecification, command.MemberSpecification);
                Assert.NotNull(command.MemberSpecification);
                Assert.Equal("single_message", command.MemberValidator.SingleError.Message);
                Assert.Same(innerPredicate, ((ValidRule<int>)command.MemberValidator.Rules.Single()).IsValid);
            }

            [Fact]
            public void Should_ThrowException_When_AsNullable_And_NullMemberSpecification()
            {
                var builder = new MemberSpecificationBuilder<object, int?>();

                Assert.Throws<ArgumentNullException>(() => { builder.AsNullable(null); });
            }
        }

        public class AsCollection
        {
            [Fact]
            public void Should_Add_AsCollectionRule_When_AsCollection()
            {
                var builder = new MemberSpecificationBuilder<object, IEnumerable<int>>();

                var itemSpecification = new MemberSpecification<object, int>(x => x);

                builder.AsCollection<object, IEnumerable<int>, int>(itemSpecification);

                Assert.Single(builder.Commands);
                Assert.IsType<AsCollectionRule<object, int>>(builder.Commands.Single());

                var command = (AsCollectionRule<object, int>)builder.Commands.Single();

                Assert.Equal("AsCollection", command.Name);
                Assert.Null(command.RuleSingleError);
                Assert.Same(itemSpecification, command.ItemSpecification);
                Assert.NotNull(command.ItemSpecification);
            }

            [Fact]
            public void Should_Add_AsCollectionRule_When_AsCollection_And_AdvancedSpecification()
            {
                var builder = new MemberSpecificationBuilder<object, IEnumerable<int>>();

                Predicate<int> innerPredicate = z => true;

                var itemSpecification = new MemberSpecification<object, int>(x => x.Valid(innerPredicate).SetSingleError("single_message"));

                builder.AsCollection<object, IEnumerable<int>, int>(itemSpecification);

                Assert.Single(builder.Commands);
                Assert.IsType<AsCollectionRule<object, int>>(builder.Commands.Single());

                var command = (AsCollectionRule<object, int>)builder.Commands.Single();

                Assert.Equal("AsCollection", command.Name);
                Assert.Null(command.RuleSingleError);
                Assert.Same(itemSpecification, command.ItemSpecification);
                Assert.NotNull(command.ItemSpecification);
                Assert.False(command.MemberValidator.IsOptional);
                Assert.Equal("single_message", command.MemberValidator.SingleError.Message);
                Assert.Same(innerPredicate, ((ValidRule<int>)command.MemberValidator.Rules.Single()).IsValid);
            }

            [Fact]
            public void Should_ThrowException_When_AsCollection_And_NullMemberSpecification()
            {
                var builder = new MemberSpecificationBuilder<object, IEnumerable<int>>();

                Assert.Throws<ArgumentNullException>(() => { builder.AsCollection<object, IEnumerable<int>, int>(null); });
            }
        }

        public class AsModelsCollection
        {
            [Fact]
            public void Should_Add_AsCollectionRule_With_AsModelRuleForItems_When_AsModelsCollection_And_SpecificationDefined()
            {
                var builder = new MemberSpecificationBuilder<object, IEnumerable<MemberClass>>();

                var itemModelSpecification = new Specification<MemberClass>(x => x);

                builder.AsModelsCollection<object, IEnumerable<MemberClass>, MemberClass>(itemModelSpecification);

                Assert.Single(builder.Commands);
                Assert.IsType<AsCollectionRule<object, MemberClass>>(builder.Commands.Single());

                var command = (AsCollectionRule<object, MemberClass>)builder.Commands.Single();

                Assert.Equal("AsCollection", command.Name);
                Assert.Null(command.RuleSingleError);
                Assert.NotNull(command.ItemSpecification);
                Assert.False(command.MemberValidator.IsOptional);

                Assert.Same(itemModelSpecification, ((AsModelRule<MemberClass>)command.MemberValidator.Rules.Single()).Specification);
                Assert.Equal(itemModelSpecification.GetHashCode().ToString(), ((AsModelRule<MemberClass>)command.MemberValidator.Rules.Single()).SpecificationId);
            }

            [Fact]
            public void Should_Add_AsCollectionRule_With_AsModelRuleForItems_When_AsModelsCollection_And_SpecificationFromRepository()
            {
                var builder = new MemberSpecificationBuilder<object, IEnumerable<MemberClass>>();

                builder.AsModelsCollection<object, IEnumerable<MemberClass>, MemberClass>();

                Assert.Single(builder.Commands);
                Assert.IsType<AsCollectionRule<object, MemberClass>>(builder.Commands.Single());

                var command = (AsCollectionRule<object, MemberClass>)builder.Commands.Single();

                Assert.Equal("AsCollection", command.Name);
                Assert.Null(command.RuleSingleError);
                Assert.NotNull(command.MemberValidator);
                Assert.NotNull(command.ItemSpecification);
                Assert.False(command.MemberValidator.IsOptional);

                Assert.Null(((AsModelRule<MemberClass>)command.MemberValidator.Rules.Single()).Specification);
                Assert.Null(((AsModelRule<MemberClass>)command.MemberValidator.Rules.Single()).SpecificationId);
            }
        }

        public class MemberClass
        {
        }

        [Fact]
        public void Should_AddMultipleCommands()
        {
            var builder = new MemberSpecificationBuilder<object, MemberClass>();

            builder
                .SetOptional()
                .SetRequired("required")
                .Valid(x => false)
                .WithMessage("message")
                .AsRelative(x => true)
                .AsModel();

            Assert.Equal(6, builder.Commands.Count);
            Assert.IsType<SetOptionalCommand>(builder.Commands.ElementAt(0));
            Assert.IsType<SetRequiredCommand>(builder.Commands.ElementAt(1));
            Assert.IsType<ValidRule<MemberClass>>(builder.Commands.ElementAt(2));
            Assert.IsType<WithMessageCommand>(builder.Commands.ElementAt(3));
            Assert.IsType<AsRelativeRule<object>>(builder.Commands.ElementAt(4));
            Assert.IsType<AsModelRule<MemberClass>>(builder.Commands.ElementAt(5));
        }

        [Fact]
        public void Should_HaveNoCommandsInitially()
        {
            var builder = new MemberSpecificationBuilder<object, object>();

            Assert.Empty(builder.Commands);
        }

        [Fact]
        public void Should_ThrowException_When_NullCommand()
        {
            var builder = new MemberSpecificationBuilder<object, MemberClass>();

            Assert.Throws<ArgumentNullException>(() => { builder.AddCommand(null); });
        }
    }
}